﻿using Skyve.App;
using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Forms;
using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public partial class ItemListControl<T> : SlickStackedListControl<T, ItemListControl<T>.Rectangles> where T : IPackageIdentity
{
	private PackageSorting sorting;
	private Rectangle PopupSearchRect1;
	private Rectangle PopupSearchRect2;
	private bool _compactList;

	public event Action<NotificationType>? CompatibilityReportSelected;
	public event Action<DownloadStatus>? DownloadStatusSelected;
	public event Action<DateTime>? DateSelected;
	public event Action<ITag>? TagSelected;
	public event Action<IUser>? AuthorSelected;
	public event Action<bool>? FilterByIncluded;
	public event Action<bool>? FilterByEnabled;
	public event Action<string>? AddToSearch;
	public event Action<T>? PackageSelected;
	public event Action? OpenWorkshopSearch;
	public event Action? OpenWorkshopSearchInBrowser;
	public event EventHandler? FilterRequested;

	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IModLogicManager _modLogicManager;
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IModUtil _modUtil;
	private readonly ITagsService _tagsService;

	protected ItemListControl(SkyvePage page)
	{
		ServiceCenter.Get(out _settings, out _tagsService, out _notifier, out _compatibilityManager, out _modLogicManager, out _subscriptionsManager, out _packageUtil, out _modUtil);

		SeparateWithLines = true;
		EnableSelection = true;

		if (!_notifier.IsContentLoaded)
		{
			Loading = true;

			_notifier.ContentLoaded += () => Loading = false;
		}

		if (_settings.UserSettings.PageSettings.ContainsKey(page))
		{
			sorting = (PackageSorting)_settings.UserSettings.PageSettings[page].Sorting;
			SortDescending = _settings.UserSettings.PageSettings[page].DescendingSort;
			GridView = _settings.UserSettings.PageSettings[page].GridView;
			CompactList = _settings.UserSettings.PageSettings[page].Compact;
		}
		else
		{
			CompactList = false;
		}
	}

	public bool SortDescending { get; private set; }
	public bool IsPackagePage { get; set; }
	public bool IsTextSearchNotEmpty { get; set; }
	public bool IsGenericPage { get; set; }
	public bool IsSelection { get => !EnableSelection; set => EnableSelection = !value; }
	public bool CompactList
	{
		get => _compactList; set
		{
			_compactList = value;
			baseHeight = _compactList ? 24 : 54;

			if (Live)
			{
				UIChanged();
			}
		}
	}

	public void DoFilterChanged()
	{
		base.FilterChanged();

		AutoInvalidate = !Loading && Items.Any() && !SafeGetItems().Any();
	}

	public void SetSorting(PackageSorting packageSorting, bool desc)
	{
		if (sorting == packageSorting && SortDescending == desc)
		{
			return;
		}

		SortDescending = desc;
		sorting = packageSorting;

		if (!IsHandleCreated)
		{
			SortingChanged();
		}
		else
		{
			new BackgroundAction(() => SortingChanged()).Run();
		}
	}

	public override void FilterChanged()
	{
		if (!IsHandleCreated)
		{
			base.FilterChanged();
		}
		else
		{
			FilterRequested?.Invoke(this, EventArgs.Empty);
		}
	}

	protected override void OnViewChanged()
	{
		if (GridView)
		{
			HighlightOnHover = false;
			Padding = UI.Scale(new Padding(6), UI.UIScale);
			GridPadding = UI.Scale(new Padding(4), UI.UIScale);
		}
		else
		{
			HighlightOnHover = false;
			Padding = new Padding((int)Math.Floor((CompactList ? 1.5 : 2.5) * UI.FontScale));
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		OnViewChanged();

		StartHeight = _compactList ? (int)(24 * UI.FontScale) : 0;
	}

	protected override IEnumerable<DrawableItem<T, Rectangles>> OrderItems(IEnumerable<DrawableItem<T, Rectangles>> items)
	{
		items = sorting switch
		{
			PackageSorting.FileSize => items
				.OrderBy(x => x.Item.GetLocalPackage()?.FileSize),

			PackageSorting.Name => items
				.OrderBy(x => x.Item.ToString()),

			PackageSorting.Author => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.Author?.Name ?? string.Empty),

			PackageSorting.Status => items
				.OrderBy(x => _packageUtil.GetStatus(x.Item.GetLocalPackage(), out _)),

			PackageSorting.UpdateTime => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.ServerTime ?? x.Item.GetLocalPackage()?.LocalTime),

			PackageSorting.SubscribeTime => items
				.OrderBy(x => x.Item.GetLocalPackage()?.LocalTime),

			PackageSorting.Mod => items
				.OrderBy(x => Path.GetFileName(x.Item.GetLocalPackage()?.FilePath ?? string.Empty)),

			PackageSorting.None => items,

			PackageSorting.CompatibilityReport => items
				.OrderBy(x => x.Item.GetCompatibilityInfo().GetNotification()),

			PackageSorting.Subscribers => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.Subscribers),

			PackageSorting.Votes => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.VoteCount),

			PackageSorting.LoadOrder => items
				.OrderBy(x => !x.Item.GetLocalPackage()?.IsIncluded())
				.ThenByDescending(x => x.Item.GetPackage() is IPackage package ? _modUtil.GetLoadOrder(package) : 0)
				.ThenBy(x => x.Item.ToString()),

			_ => items
				.OrderBy(x => !(x.Item.GetLocalPackage() is ILocalPackageData lp && (lp.IsIncluded(out var partial) || partial)))
				.ThenBy(x => x.Item.GetPackage()?.IsLocal)
				.ThenBy(x => !x.Item.GetPackage()?.IsCodeMod)
				.ThenBy(x => x.Item.GetLocalPackage()?.CleanName() ?? x.Item.CleanName())
		};

		if (SortDescending)
		{
			return items.Reverse();
		}

		return items;
	}

	protected override async void OnItemMouseClick(DrawableItem<T, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (e.Button == MouseButtons.Right)
		{
			ShowRightClickMenu(item.Item);
			return;
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		var rects = item.Rectangles;
		var filter = ModifierKeys.HasFlag(Keys.Alt) != _settings.UserSettings.FlipItemCopyFilterAction;

#if CS2
		if (rects.IncludedRect.Contains(e.Location))
		{
			var isIncluded = item.Item.IsIncluded(out _, out var partialIncluded) || partialIncluded;
			var isEnabled = item.Item.IsEnabled(out _);

			if (!isIncluded)
			{
				await _subscriptionsManager.Subscribe([item.Item]);
			}
			else
			{
				_packageUtil.SetEnabled(item.Item.GetLocalPackageIdentity()!, !isEnabled);
			}
		}
#else

		if (rects.IncludedRect.Contains(e.Location))
		{
			if (item.Item.GetLocalPackage() is not ILocalPackageData localPackage)
			{
				if (item.Item.GetPackage()?.IsLocal != true)
				{
					_subscriptionsManager.Subscribe(new IPackageIdentity[] { item.Item });
				}

				return;
			}

			if (ModifierKeys.HasFlag(Keys.Alt))
			{
				FilterByIncluded?.Invoke(_packageUtil.IsIncluded(localPackage));
			}
			else
			{
				_packageUtil.SetIncluded(localPackage, !_packageUtil.IsIncluded(localPackage));
			}

			return;
		}

		if (rects.EnabledRect.Contains(e.Location) && item.Item.GetLocalPackageIdentity() is ILocalPackageIdentity packageIdentity)
		{
			if (ModifierKeys.HasFlag(Keys.Alt))
			{
				FilterByEnabled?.Invoke(_packageUtil.IsEnabled(packageIdentity));
			}
			else
			{
				_packageUtil.SetEnabled(packageIdentity, !_packageUtil.IsEnabled(packageIdentity));
			}

			return;
		}
#endif

			if (rects.FolderRect.Contains(e.Location))
		{
			PlatformUtil.OpenFolder(item.Item.GetLocalPackage()?.FilePath);
			return;
		}

		if (rects.SteamRect.Contains(e.Location) && item.Item.GetWorkshopInfo()?.Url is string url)
		{
			PlatformUtil.OpenUrl(url);
			return;
		}

		if (rects.GithubRect.Contains(e.Location) && _compatibilityManager.GetPackageInfo(item.Item)?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
		{
			PlatformUtil.OpenUrl(gitLink.Url);
			return;
		}

		if (rects.AuthorRect.Contains(e.Location) && item.Item.GetWorkshopInfo()?.Author is IUser user)
		{
			if (filter)
			{
				AuthorSelected?.Invoke(user);
			}
			else
			{
				var pc = new PC_UserPage(user);

				(FindForm() as BasePanelForm)?.PushPanel(null, pc);
			}

			return;
		}

		if (rects.FolderNameRect.Contains(e.Location) && item.Item.GetPackage()?.IsLocal == true)
		{
			if (filter)
			{
				AddToSearch?.Invoke(Path.GetFileName(item.Item.GetLocalPackage()?.Folder ?? string.Empty));
			}
			else
			{
				Clipboard.SetText(Path.GetFileName(item.Item.GetLocalPackage()?.Folder ?? string.Empty));

			}

			return;
		}

		if (rects.SteamIdRect.Contains(e.Location))
		{
			if (filter)
			{
				AddToSearch?.Invoke(item.Item.Id.ToString());
			}
			else
			{
				Clipboard.SetText(item.Item.Id.ToString());
			}

			return;
		}

		if (rects.CompatibilityRect.Contains(e.Location))
		{
			if (filter)
			{
				CompatibilityReportSelected?.Invoke(item.Item.GetCompatibilityInfo().GetNotification());
			}
			else
			{
				var pc = new PC_PackagePage(item.Item, true);

				(FindForm() as BasePanelForm)?.PushPanel(null, pc);

				if (_settings.UserSettings.ResetScrollOnPackageClick)
				{
					ScrollTo(item.Item);
				}
			}
			return;
		}

		if (rects.DownloadStatusRect.Contains(e.Location) && filter)
		{
			DownloadStatusSelected?.Invoke(_packageUtil.GetStatus(item.Item.GetLocalPackageIdentity(), out _));

			return;
		}

		if (rects.VersionRect.Contains(e.Location) && item.Item.GetLocalPackage() is ILocalPackageData localPackageData)
		{
			Clipboard.SetText(localPackageData.Version);
		}

		if (rects.ScoreRect.Contains(e.Location))
		{
#if CS1
			new RatingInfoForm { Icon = Program.MainForm?.Icon }.ShowDialog(Program.MainForm);
#else
			await ServiceCenter.Get<IWorkshopService>().ToggleVote(item.Item);
#endif
			return;
		}

		if (rects.CenterRect.Contains(e.Location) || rects.IconRect.Contains(e.Location))
		{
			if (IsPackagePage)
			{
				return;
			}

			if (IsSelection)
			{
				PackageSelected?.Invoke(item.Item);
				return;
			}

			(FindForm() as BasePanelForm)?.PushPanel(null, item.Item.GetWorkshopInfo()?.IsCollection == true ? new PC_ViewCollection(item.Item.GetWorkshopPackage()!) : new PC_PackagePage(item.Item));

			if (_settings.UserSettings.ResetScrollOnPackageClick)
			{
				ScrollTo(item.Item);
			}

			return;
		}

		if (rects.DateRect.Contains(e.Location))
		{
			var date = item.Item.GetWorkshopInfo()?.ServerTime ?? item.Item.GetLocalPackage()?.LocalTime;

			if (date.HasValue)
			{
				if (filter)
				{
					DateSelected?.Invoke(date.Value);
				}
				else
				{
					Clipboard.SetText(date.Value.ToString("g"));
				}
			}

			return;
		}

		foreach (var tag in rects.TagRects)
		{
			if (tag.Value.Contains(e.Location))
			{
				if (filter)
				{
					TagSelected?.Invoke(tag.Key);
				}
				else
				{
					Clipboard.SetText(tag.Key.Value);
				}

				return;
			}
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left && PopupSearchRect1.Contains(e.Location))
		{
			OpenWorkshopSearch?.Invoke();
		}

		if (e.Button == MouseButtons.Left && PopupSearchRect2.Contains(e.Location))
		{
			OpenWorkshopSearchInBrowser?.Invoke();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			PopupSearchRect1 = PopupSearchRect2 = Rectangle.Empty;

			if (Loading)
			{
				base.OnPaint(e);
			}
			else if (!Items.Any())
			{
				e.Graphics.DrawString(Locale.NoLocalPackagesFound + "\r\n" + Locale.CheckFolderInOptions, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(FormDesign.Design.LabelColor), ClientRectangle, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			}
			else if (!SafeGetItems().Any())
			{
				if (!IsTextSearchNotEmpty)
				{
					e.Graphics.DrawString(Locale.NoPackagesMatchFilters, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(FormDesign.Design.LabelColor), ClientRectangle.Pad(0, 0, 0, Height / 3), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
					return;
				}

				CursorLocation = PointToClient(Cursor.Position);

				e.Graphics.DrawString(Locale.NoPackagesMatchFilters, UI.Font(9.75F, FontStyle.Italic), new SolidBrush(FormDesign.Design.LabelColor), ClientRectangle.Pad(0, 0, 0, Height / 3), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

				using var icon1 = IconManager.GetIcon("I_Steam");
				using var icon2 = IconManager.GetIcon("I_Link");

				var buttonSize1 = SlickButton.GetSize(e.Graphics, icon1, Locale.SearchWorkshop, UI.Font(9.75F));
				var buttonSize2 = SlickButton.GetSize(e.Graphics, icon2, Locale.SearchWorkshopBrowser, UI.Font(9.75F));
				PopupSearchRect1 = ClientRectangle.Pad(0, Height / 3, 0, 0).CenterR(buttonSize1);

				SlickButton.GetColors(out var fore, out var back, PopupSearchRect1.Contains(CursorLocation) ? HoverState : HoverState.Normal);

				if (!PopupSearchRect1.Contains(CursorLocation))
				{
					back = Color.Empty;
				}

				SlickButton.DrawButton(e, PopupSearchRect1.Location, PopupSearchRect1.Size, Locale.SearchWorkshop, UI.Font(9.75F), back, fore, icon1, UI.Scale(new Padding(7), UI.UIScale), true, PopupSearchRect1.Contains(CursorLocation) ? HoverState : HoverState.Normal, ColorStyle.Active);

				PopupSearchRect2 = new Rectangle(PopupSearchRect1.X, PopupSearchRect1.Bottom + (Padding.Vertical * 2), buttonSize2.Width, buttonSize2.Height);

				SlickButton.GetColors(out fore, out back, PopupSearchRect2.Contains(CursorLocation) ? HoverState : HoverState.Normal);

				if (!PopupSearchRect2.Contains(CursorLocation))
				{
					back = Color.Empty;
				}

				SlickButton.DrawButton(e, PopupSearchRect2.Location, PopupSearchRect2.Size, Locale.SearchWorkshopBrowser, UI.Font(9.75F), back, fore, icon2, UI.Scale(new Padding(7), UI.UIScale), true, PopupSearchRect2.Contains(CursorLocation) ? HoverState : HoverState.Normal, ColorStyle.Active);

				Cursor = PopupSearchRect1.Contains(CursorLocation) || PopupSearchRect2.Contains(CursorLocation) ? Cursors.Hand : Cursors.Default;
			}
			else
			{
				base.OnPaint(e);
			}
		}
		catch (Exception ex) { if (System.Diagnostics.Debugger.IsAttached) MessagePrompt.Show(ex); }
	}

	public void ShowRightClickMenu(T item)
	{
		var items = ServiceCenter.Get<ICustomPackageService>().GetRightClickMenuItems((SelectedItems.Count > 0 ? SelectedItems.Select(x => x.Item) : new T[] { item }).Cast<IPackage>());

		this.TryBeginInvoke(() => SlickToolStrip.Show(FindForm() as SlickForm, items));
	}

	private bool GetStatusDescriptors(T mod, out string text, out DynamicIcon? icon, out Color color)
	{
		switch (_packageUtil.GetStatus(mod.GetLocalPackageIdentity(), out text))
		{
			case DownloadStatus.Unknown:
				text = Locale.StatusUnknown;
				icon = "I_Question";
				color = FormDesign.Design.YellowColor;
				return true;
			case DownloadStatus.OutOfDate:
				text = Locale.OutOfDate;
				icon = "I_OutOfDate";
				color = FormDesign.Design.YellowColor;
				return true;
			case DownloadStatus.PartiallyDownloaded:
				text = Locale.PartiallyDownloaded;
				icon = "I_Broken";
				color = FormDesign.Design.RedColor;
				return true;
			case DownloadStatus.Removed:
				text = Locale.RemovedByAuthor;
				icon = "I_ContentRemoved";
				color = FormDesign.Design.RedColor;
				return true;
		}

		icon = null;
		color = Color.White;
		return false;
	}

	public class Rectangles : IDrawableItemRectangles<T>
	{
		public Dictionary<ITag, Rectangle> TagRects = new();
		public Rectangle IncludedRect;
		public Rectangle EnabledRect;
		public Rectangle FolderRect;
		public Rectangle IconRect;
		public Rectangle TextRect;
		public Rectangle SteamRect;
		public Rectangle SteamIdRect;
		public Rectangle CenterRect;
		public Rectangle AuthorRect;
		public Rectangle VersionRect;
		public Rectangle CompatibilityRect;
		public Rectangle DownloadStatusRect;
		public Rectangle DateRect;
		public Rectangle ScoreRect;
		public Rectangle GithubRect;
		public Rectangle FolderNameRect;

		public T Item { get; set; }

		public Rectangles(T item)
		{
			Item = item;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				IncludedRect.Contains(location) ||
				EnabledRect.Contains(location) ||
				FolderRect.Contains(location) ||
				SteamRect.Contains(location) ||
				AuthorRect.Contains(location) ||
				FolderNameRect.Contains(location) ||
				((CenterRect.Contains(location) || IconRect.Contains(location)) && !(instance as ItemListControl<T>)!.IsPackagePage) ||
				DownloadStatusRect.Contains(location) ||
				ScoreRect.Contains(location) ||
				CompatibilityRect.Contains(location) ||
				DateRect.Contains(location) ||
				GithubRect.Contains(location) ||
				(VersionRect.Contains(location) && Item?.GetPackage()?.IsCodeMod == true) ||
				TagRects.Any(x => x.Value.Contains(location)) ||
				SteamIdRect.Contains(location);
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (IncludedRect.Contains(location))
			{
				if (Item.GetLocalPackage() is null)
				{
					if (!Item.GetPackage()!.IsLocal)
					{
						text = Locale.SubscribeToItem;
						point = IncludedRect.Location;
						return true;
					}
					else
					{
						text = string.Empty;
						point = default;
						return false;
					}
				}

				if (Item.GetLocalPackageIdentity().IsIncluded())
				{
					text = $"{Locale.ExcludePackage.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByIncluded.ToString().ToLower())}";
				}
				else
				{
					text = $"{Locale.IncludePackage.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByExcluded.ToString().ToLower())}";
				}

				point = IncludedRect.Location;
				return true;
			}

			if (EnabledRect.Contains(location) && Item.GetLocalPackage().IsCodeMod)
			{
				if (Item.GetLocalPackageIdentity().IsEnabled())
				{
					text = $"{Locale.DisablePackage.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByEnabled.ToString().ToLower())}";
				}
				else
				{
					text = $"{Locale.EnablePackage.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByDisabled.ToString().ToLower())}";
				}

				point = EnabledRect.Location;
				return true;
			}

			if (SteamRect.Contains(location))
			{
				text = Locale.ViewOnSteam;
				point = SteamRect.Location;
				return true;
			}

			if (GithubRect.Contains(location))
			{
				text = Locale.ViewOnGithub;
				point = GithubRect.Location;
				return true;
			}

			if (SteamIdRect.Contains(location))
			{
				text = getFilterTip(string.Format(Locale.CopyToClipboard, Item.Id), string.Format(Locale.AddToSearch, Item.Id));
				point = SteamIdRect.Location;
				return true;
			}

			if (FolderNameRect.Contains(location))
			{
				var folder = Path.GetFileName(Item.GetLocalPackageIdentity()?.Folder ?? string.Empty);
				text = getFilterTip(string.Format(Locale.CopyToClipboard, folder), string.Format(Locale.AddToSearch, folder));
				point = FolderNameRect.Location;
				return true;
			}

			if (AuthorRect.Contains(location))
			{
				text = getFilterTip(Locale.OpenAuthorPage, Locale.FilterByThisAuthor.Format(Item.GetWorkshopInfo()?.Author?.Name ?? "this author"));
				point = AuthorRect.Location;
				return true;
			}

			if (FolderRect.Contains(location))
			{
				text = Locale.OpenLocalFolder;
				point = FolderRect.Location;
				return true;
			}

			if (CompatibilityRect.Contains(location))
			{
				text = getFilterTip(Locale.ViewPackageCR, Locale.FilterByThisReportStatus);
				point = CompatibilityRect.Location;
				return true;
			}

			if (DownloadStatusRect.Contains(location))
			{
				var packageUtil = ServiceCenter.Get<IPackageUtil>();
				packageUtil.GetStatus(Item.GetLocalPackageIdentity(), out var reason);

				if (ServiceCenter.Get<ISettings>().UserSettings.FlipItemCopyFilterAction)
				{
					text = Locale.FilterByThisPackageStatus + "\r\n\r\n" + reason;
					point = DownloadStatusRect.Location;
					return true;
				}
				else
				{
					text = reason + "\r\n\r\n" + string.Format(Locale.AltClickTo, Locale.FilterByThisPackageStatus.ToString().ToLower());
					point = DownloadStatusRect.Location;
					return true;
				}
			}

			if (ScoreRect.Contains(location))
			{
				var workshopInfo = Item.GetWorkshopInfo();
				if (workshopInfo is not null)
				{
					text = string.Format(Locale.RatingCount, workshopInfo.VoteCount.ToString("N0"), $"({workshopInfo.VoteCount}%)") + "\r\n" + string.Format(Locale.SubscribersCount, workshopInfo.Subscribers.ToString("N0"));
					point = ScoreRect.Location;
					return true;
				}
			}

			if (VersionRect.Contains(location) && Item.GetLocalPackage()?.Version is not null)
			{
				text = Locale.CopyVersionNumber;
				point = VersionRect.Location;
				return true;
			}

			if ((CenterRect.Contains(location) || IconRect.Contains(location)) && !(instance as ItemListControl<T>)!.IsPackagePage)
			{
				if ((instance as ItemListControl<T>)!.IsSelection)
				{
					text = Locale.SelectThisPackage;
				}
				else
				{
					text = Locale.OpenPackagePage;
				}

				point = CenterRect.Location;
				return true;
			}

			if (DateRect.Contains(location))
			{
				var date = Item.GetWorkshopInfo()?.ServerTime ?? Item.GetLocalPackage()?.LocalTime;
				if (date.HasValue)
				{
					text = getFilterTip(string.Format(Locale.CopyToClipboard, date.Value.ToString("g")), Locale.FilterSinceThisDate);
					point = DateRect.Location;
					return true;
				}
			}

			foreach (var tag in TagRects)
			{
				if (tag.Value.Contains(location))
				{
					text = getFilterTip(string.Format(Locale.CopyToClipboard, tag.Key), string.Format(Locale.FilterByThisTag, tag.Key));
					point = tag.Value.Location;
					return true;
				}
			}

			text = string.Empty;
			point = default;
			return false;

			static string getFilterTip(string? text, string? alt)
			{
				var tip = string.Empty;

				if (ServiceCenter.Get<ISettings>().UserSettings.FlipItemCopyFilterAction)
				{
					ExtensionClass.Swap(ref text, ref alt);
				}

				if (text is not null)
				{
					tip += text + "\r\n\r\n";
				}

				if (alt is not null)
				{
					tip += string.Format(Locale.AltClickTo, alt.ToLower());
				}

				return tip.Trim();
			}
		}
	}
}
