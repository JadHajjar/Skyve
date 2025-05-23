﻿using Skyve.App;
using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;
using Skyve.App.Utilities;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public partial class ItemListControl : SlickStackedListControl<IPackageIdentity, ItemListControl.Rectangles>
{
	private PackageSorting sorting;
	private Rectangle PopupSearchRect1;
	private Rectangle PopupSearchRect2;
	private bool _compactList;
	private bool settingItems;
	private readonly Dictionary<Columns, (int X, int Width)> _columnSizes = [];
	public readonly List<ulong> _selectionList = [];

	public event Action<NotificationType>? CompatibilityReportSelected;
	public event Action<DownloadStatus>? DownloadStatusSelected;
	public event Action<DateTime>? DateSelected;
	public event Action<ITag>? TagSelected;
	public event Action<IUser>? AuthorSelected;
	public event Action<bool>? FilterByIncluded;
	public event Action<bool>? FilterByEnabled;
	public event Action<string>? AddToSearch;
	public event Action<IPackageIdentity>? PackageSelected;
	public event Action<IPackageIdentity>? PackageUnSelected;
	public event Action? OpenWorkshopSearch;
	public event Action? OpenWorkshopSearchInBrowser;
	public event EventHandler? FilterRequested;

	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IModLogicManager _modLogicManager;
	private readonly IUserService _userService;
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly IPlaysetManager _playsetManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IModUtil _modUtil;
	private readonly ITagsService _tagsService;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly IWorkshopService _workshopService;
	private readonly SkyvePage _page;

	private enum Columns
	{
		PackageName,
		Version,
		UpdateTime,
		Author,
		Tags,
		Status,
		Buttons
	}

	public IEnumerable<IPackageIdentity> GetSelectedItems()
	{
		return SelectedItems.Select(x => x.Item);
	}

	protected ItemListControl(SkyvePage page, IPackageUtil? customPackageUtil = null)
	{
		_page = page;
		ServiceCenter.Get(out _settings, out _tagsService, out _notifier, out _workshopService, out _compatibilityManager, out _skyveDataManager, out _modLogicManager, out _userService, out _playsetManager, out _subscriptionsManager, out _packageUtil, out _modUtil);

		if (customPackageUtil is not null)
		{
			_packageUtil = customPackageUtil;
		}

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

		AltStateChanged += AltKeyStateChanged;
	}

	public int UsageFilteredOut { get; set; }
	public int? SelectedPlayset { get; set; }
	public bool SortDescending { get; private set; }
	public bool IsPackagePage { get; set; }
	public bool IsTextSearchNotEmpty { get; set; }
	public bool IsGenericPage { get; set; }
	public bool IsSelection { get => !EnableSelection; set => EnableSelection = !value; }
	public bool CompactList
	{
		get => _compactList;
		set
		{
			_compactList = value;

			baseHeight = _settings.UserSettings.ComplexListUI ? _compactList ? 24 : 60 : _compactList ? 20 : 48;

			if (Live)
			{
				UIChanged();
			}
		}
	}

	public override void SetItems(IEnumerable<IPackageIdentity> items)
	{
		settingItems = true;
		base.SetItems(items);
		settingItems = false;
	}

	public void DoFilterChanged()
	{
		UsageFilteredOut = 0;

		base.FilterChanged();
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
		if (!IsHandleCreated || settingItems)
		{
			UsageFilteredOut = 0;

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
		}
		else
		{
			HighlightOnHover = false;

			if (CompactList)
			{
				Padding = new Padding((int)Math.Round(2.5 * UI.FontScale), (int)Math.Round(1.5 * UI.FontScale), (int)Math.Round(2.5 * UI.FontScale), (int)Math.Round(1.5 * UI.FontScale));
			}
			else
			{
				Padding = new Padding((int)Math.Round(3 * UI.FontScale), (int)Math.Round(2 * UI.FontScale), (int)Math.Round(3 * UI.FontScale), (int)Math.Round(2 * UI.FontScale));
			}
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		OnViewChanged();

		StartHeight = _compactList ? UI.Scale(24) : 0;
	}

	protected override void CanDrawItemInternal(CanDrawItemEventArgs<IPackageIdentity> args)
	{
		if (_playsetManager.CurrentCustomPlayset?.Usage > 0)
		{
			if (!(args.Item.GetPackageInfo()?.Usage.HasFlag(_playsetManager.CurrentCustomPlayset.Usage) ?? true))
			{
				UsageFilteredOut++;
				args.DrawableItem.Tag = true;
			}
			else
			{
				args.DrawableItem.Tag = null;
			}

			if (_page is not SkyvePage.Workshop)
			{
				base.CanDrawItemInternal(args);
			}

			return;
		}

		if (_skyveDataManager.IsBlacklisted(args.Item))
		{
			args.DoNotDraw = true;

			args.DrawableItem.Bounds = Rectangle.Empty;
			args.DrawableItem.Hidden = args.DoNotDraw;

			if (args.DoNotDraw)
			{
				SelectedItems.Remove(args.DrawableItem);
			}
		}
		else if (_page is SkyvePage.Workshop)
		{
			OnCanDrawItem(args);

			args.DrawableItem.Tag = args.DoNotDraw ? true : null;
		}
		else
		{
			base.CanDrawItemInternal(args);
		}
	}

	protected override IEnumerable<IDrawableItem<IPackageIdentity>> OrderItems(IEnumerable<IDrawableItem<IPackageIdentity>> items)
	{
		if (sorting > PackageSorting.WorkshopSorting)
		{
			return items;
		}

		items = sorting switch
		{
			PackageSorting.FileSize => items
				.OrderBy(x => x.Item.GetLocalPackage()?.FileSize),

			PackageSorting.Name => items
				.OrderBy(x => x.Item.CleanName()),

			PackageSorting.Author => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.Author?.Name ?? string.Empty),

			PackageSorting.Status => items
				.OrderBy(x => _packageUtil.GetStatus(x.Item.GetLocalPackage(), out _)),

			PackageSorting.UpdateTime => items
				.OrderBy(x => (x.Item.GetWorkshopInfo()?.ServerTime).If(y => y is null || y.Value == DateTime.MinValue, _ => x.Item.GetLocalPackage()?.LocalTime ?? DateTime.UtcNow, y => y ?? DateTime.Now)),

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
				.ThenBy(x => x.Item.GetPackage() is IPackage package ? _modUtil.GetLoadOrder(package) : 0)
				.ThenBy(x => x.Item.CleanName()),

			_ => items
				.OrderBy(x => !_packageUtil.IsIncluded(x.Item, out var partial) || partial)
				.ThenBy(x => !_packageUtil.IsEnabled(x.Item, SelectedPlayset))
				.ThenBy(x => x.Item.IsLocal())
				.ThenBy(x => !x.Item.IsCodeMod())
				.ThenBy(x => x.Item.CleanName())
		};

		return SortDescending ? items.Reverse() : items;
	}

	private void AltKeyStateChanged(object sender, EventArgs e)
	{
		Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		AltStateChanged -= AltKeyStateChanged;

		base.Dispose(disposing);
	}

	protected override async void OnItemMouseClick(DrawableItem<IPackageIdentity, Rectangles> item, MouseEventArgs e)
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

		if (rects.DotsRect.Contains(e.Location))
		{
			ShowRightClickMenu(item.Item);
			return;
		}

#if CS2
		if (rects.IncludedRect.Contains(e.Location))
		{
			var isIncluded = _packageUtil.IsIncluded(item.Item, out var partialIncluded, SelectedPlayset) && !partialIncluded;

			Loading = item.Loading = true;

			if (!isIncluded || (ModifierKeys.HasFlag(Keys.Alt) && !_modLogicManager.IsRequired(item.Item.GetLocalPackageIdentity(), _modUtil, SelectedPlayset)))
			{
				await _packageUtil.SetIncluded(item.Item, !isIncluded, SelectedPlayset);
			}
			else
			{
				var enable = !_packageUtil.IsEnabled(item.Item, SelectedPlayset);

				if (enable || !_modLogicManager.IsRequired(item.Item.GetLocalPackageIdentity(), _modUtil, SelectedPlayset))
				{
					await _packageUtil.SetEnabled(item.Item, enable, SelectedPlayset);
				}
			}

			item.Loading = false;
			Invalidate(item.Item);
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
			PlatformUtil.OpenFolder(item.Item.GetLocalPackageIdentity()?.FilePath);
			return;
		}

		if (rects.WorkshopRect.Contains(e.Location) && item.Item.GetWorkshopInfo()?.Url is string url)
		{
			PlatformUtil.OpenUrl(url);
			return;
		}

		if (rects.GithubRect.Contains(e.Location) && item.Item.GetPackageInfo()?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
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
				Program.MainForm.PushPanel(new PC_UserPage(user));
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
				ServiceCenter.Get<IInterfaceService>().OpenPackagePage(item.Item.GetPackage() ?? item.Item, true);

				if (_settings.UserSettings.ResetScrollOnPackageClick)
				{
					ScrollTo(item.Item);
				}
			}

			return;
		}

		if (rects.DownloadStatusRect.Contains(e.Location) && filter)
		{
			DownloadStatusSelected?.Invoke(_packageUtil.GetStatus(item.Item, out _));

			return;
		}

		if (rects.VersionRect.Contains(e.Location) && item.Item.GetLocalPackage() is ILocalPackageData localPackageData)
		{
			Clipboard.SetText(localPackageData.VersionName);
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
				if (_selectionList.Contains(item.Item.Id))
				{
					_selectionList.Remove(item.Item.Id);
					PackageUnSelected?.Invoke(item.Item);
				}
				else
				{
					_selectionList.Add(item.Item.Id);
					PackageSelected?.Invoke(item.Item);
				}

				return;
			}

			ServiceCenter.Get<IInterfaceService>().OpenPackagePage(item.Item.GetPackage() ?? item.Item, false);
			//(FindForm() as BasePanelForm)?.PushPanel(/*item.Item.GetWorkshopInfo()?.IsCollection == true ? new PC_ViewCollection(item.Item.GetWorkshopPackage()!) :*/ new PC_PackagePage(item.Item.GetPackage() ?? item.Item));

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
					Clipboard.SetText(date.Value.ToLocalTime().ToString("g"));
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
					Clipboard.SetText(tag.Key.ToString());
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

	protected override void InvalidateForLoading()
	{
		var items = SafeGetItems();

		items.RemoveAll(x => !x.Loading || x.Rectangles is null);

		if (items.Count == 0)
		{
			Invalidate();
			return;
		}

		foreach (var item in items)
		{
			Invalidate(((Rectangles)item.Rectangles).IncludedRect);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (!AnyVisibleItems())
		{
			Invalidate();
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);

		if (!AnyVisibleItems())
		{
			Invalidate();
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Alt)
		{
			Invalidate();
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		if (ItemCount > 0)
		{
			var loading = false;

			foreach (var item in SafeGetItems())
			{
				loading |= !item.Hidden & (item.Loading = _subscriptionsManager.IsSubscribing(item.Item) || _modUtil.IsEnabling(item.Item));
			}

			if (Loading != loading)
			{
				Loading = loading;
				Invalidate();
			}
		}

		base.OnPaintBackground(e);
	}

	protected override void OnPaintItem(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
	{
		if (IsSelection)
		{
			e.IsSelected = _selectionList.Contains(e.Item.Id);
		}

		base.OnPaintItem(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			base.OnPaint(e);

			PopupSearchRect1 = PopupSearchRect2 = Rectangle.Empty;

			if (Loading || AnyVisibleItems())
			{
				if (ItemCount > 5 && _page == SkyvePage.Workshop && scrollVisible)
				{
					var itemList = SafeGetItems();

					var maxIndex = GetMaxScrollIndex(itemList);
					var displayedRows = GetDisplayedRows();

					if (ScrollIndex < maxIndex)
					{
						var size = (int)(UI.Scale(150) * Math.Min(1, (maxIndex - ScrollIndex) / (displayedRows / 3)));
						var rect = new Rectangle(0, Height - size, Width, size + 1);

						using var gradientBrush = new LinearGradientBrush(rect, Color.FromArgb(0, BackColor), BackColor, 90f);

						e.Graphics.FillRectangle(gradientBrush, rect);
					}
				}

				return;
			}

			e.Graphics.ResetClip();

			using var font = UI.Font(9.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.LabelColor);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			if (ItemCount == 0 && _page != SkyvePage.Workshop)
			{
#if CS1
				e.Graphics.DrawString(Locale.NoLocalPackagesFound + "\r\n" + Locale.CheckFolderInOptions, font, brush, ClientRectangle, stringFormat);
#else
				e.Graphics.DrawString(Locale.NoLocalPackagesFound, font, brush, ClientRectangle, stringFormat);
#endif

				DrawWorkshopSearchButtons(e, true);
			}
			else
			{
				e.Graphics.DrawString(Locale.NoPackagesMatchFilters, font, brush, ClientRectangle.Pad(0, 0, 0, Height / 3), stringFormat);

				DrawWorkshopSearchButtons(e, false);
			}
		}
		catch (Exception ex)
		{
			if (System.Diagnostics.Debugger.IsAttached)
			{
				MessagePrompt.Show(ex);
			}
		}
	}

	private void DrawWorkshopSearchButtons(PaintEventArgs e, bool emptySearch)
	{
		if (_page == SkyvePage.Workshop)
		{
			return;
		}

		CursorLocation = PointToClient(Cursor.Position);

		using var font = UI.Font(9.75F);

		PopupSearchRect1 = SlickButton.AlignAndDraw(e.Graphics, new Rectangle(0, Height * 2 / 3, Width, 0), ContentAlignment.TopCenter, new ButtonDrawArgs
		{
			Text = emptySearch ? Locale.DiscoverWorkshop : Locale.SearchWorkshop,
#if CS2
			Icon = "Paradox",
#else
			Icon = "Steam",
#endif
			Font = font,
			Padding = UI.Scale(new Padding(7), UI.UIScale),
			Cursor = CursorLocation,
			HoverState = HoverState,
			ButtonType = ButtonType.Hidden
		}).Rectangle;

		PopupSearchRect2 = emptySearch ? default : SlickButton.AlignAndDraw(e.Graphics, new Rectangle(0, PopupSearchRect1.Bottom + (Padding.Vertical * 2), Width, 0), ContentAlignment.TopCenter, new ButtonDrawArgs
		{
			Text = Locale.SearchWorkshopBrowser,
			Icon = "Link",
			Font = font,
			Padding = UI.Scale(new Padding(7), UI.UIScale),
			Cursor = CursorLocation,
			HoverState = HoverState,
			ButtonType = ButtonType.Hidden
		}).Rectangle;

		Cursor = PopupSearchRect1.Contains(CursorLocation) || PopupSearchRect2.Contains(CursorLocation) ? Cursors.Hand : Cursors.Default;
	}

	public void ShowRightClickMenu(IPackageIdentity item)
	{
		var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(SelectedItems.Count > 0 ? SelectedItems.Select(x => x.Item) : new IPackageIdentity[] { item });

		this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, items));
	}

	private bool GetStatusDescriptors(IPackageIdentity mod, out string text, out DynamicIcon? icon, out Color color)
	{
		switch (_packageUtil.GetStatus(mod, out text))
		{
			case DownloadStatus.Unknown:
				text = Locale.StatusUnknown;
				icon = "Question";
				color = FormDesign.Design.YellowColor;
				return true;
			case DownloadStatus.OutOfDate:
				text = Locale.OutOfDate;
				icon = "OutOfDate";
				color = FormDesign.Design.YellowColor;
				return true;
			case DownloadStatus.PartiallyDownloaded:
				text = Locale.PartiallyDownloaded;
				icon = "Broken";
				color = FormDesign.Design.RedColor;
				return true;
			case DownloadStatus.Removed:
				text = Locale.RemovedByAuthor;
				icon = "ContentRemoved";
				color = FormDesign.Design.RedColor;
				return true;
		}

		icon = null;
		color = Color.White;
		return false;
	}

	public class Rectangles : IDrawableItemRectangles<IPackageIdentity>
	{
		public Dictionary<ITag, Rectangle> TagRects = [];
		public Rectangle IncludedRect;
		public Rectangle EnabledRect;
		public Rectangle FolderRect;
		public Rectangle IconRect;
		public Rectangle TextRect;
		public Rectangle WorkshopRect;
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
		public Rectangle DotsRect;

		public IPackageIdentity Item { get; set; }

		public Rectangles(IPackageIdentity item)
		{
			Item = item;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				(IncludedRect.Contains(location) && !(instance as ItemListControl)!.IsGenericPage) ||
				EnabledRect.Contains(location) ||
				FolderRect.Contains(location) ||
				WorkshopRect.Contains(location) ||
				AuthorRect.Contains(location) ||
				FolderNameRect.Contains(location) ||
				((CenterRect.Contains(location) || IconRect.Contains(location)) && !(instance as ItemListControl)!.IsPackagePage) ||
				ScoreRect.Contains(location) ||
				DotsRect.Contains(location) ||
				CompatibilityRect.Contains(location) ||
				DateRect.Contains(location) ||
				GithubRect.Contains(location) ||
				(VersionRect.Contains(location) && Item != null && Item.IsCodeMod()) ||
				TagRects.Any(x => x.Value.Contains(location)) ||
				SteamIdRect.Contains(location);
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			var listControl = (instance as ItemListControl)!;

			if (IncludedRect.Contains(location))
			{
				point = IncludedRect.Location;

				var isIncluded = listControl._packageUtil.IsIncluded(Item, out var partialIncluded, listControl.SelectedPlayset) && !partialIncluded;

				if (!isIncluded || ModifierKeys.HasFlag(Keys.Alt))
				{
					if (!isIncluded)
					{
						text = Locale.IncludeItem;
					}
					else if (!listControl._modLogicManager.IsRequired(Item.GetLocalPackageIdentity(), listControl._modUtil, listControl.SelectedPlayset))
					{
						text = Locale.ExcludeItem;
					}
					else
					{
						text = Locale.ThisModIsRequiredYouCantDisableIt;
					}
				}
				else
				{
					var isEnabled = listControl._packageUtil.IsEnabled(Item, listControl.SelectedPlayset);

					if (!isEnabled)
					{
						text = Locale.EnableItem + "\r\n\r\n" + Locale.AltClickTo.Format(Locale.ExcludeItem.ToLower());
					}
					else if (!listControl._modLogicManager.IsRequired(Item.GetLocalPackageIdentity(), listControl._modUtil, listControl.SelectedPlayset))
					{
						text = Locale.DisableItem + "\r\n\r\n" + Locale.AltClickTo.Format(Locale.ExcludeItem.ToLower());
					}
					else
					{
						text = Locale.ThisModIsRequiredYouCantDisableIt;
					}
				}

				return true;
			}
#if CS1
			if (EnabledRect.Contains(location) && Item.IsCodeMod())
			{
				text = Item.IsEnabled()
					? $"{Locale.DisablePackage.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByEnabled.ToString().ToLower())}"
					: $"{Locale.EnablePackage.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByDisabled.ToString().ToLower())}";

				point = EnabledRect.Location;
				return true;
			}
#endif
			if (WorkshopRect.Contains(location))
			{
				text = Locale.ViewOnWorkshop;
				point = WorkshopRect.Location;
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
				packageUtil.GetStatus(Item, out var reason);

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
					text = LocaleHelper.GetGlobalText(!workshopInfo.HasVoted ? "VoteMod" : "UnVoteMod");//					string.Format(Locale.RatingCount, workshopInfo.VoteCount.ToString("N0"), $"({workshopInfo.VoteCount}%)") + "\r\n" + string.Format(Locale.SubscribersCount, workshopInfo.Subscribers.ToString("N0"));
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

			if ((CenterRect.Contains(location) || IconRect.Contains(location)) && !(instance as ItemListControl)!.IsPackagePage)
			{
				text = (instance as ItemListControl)!.IsSelection ? (string)Locale.SelectThisPackage : (string)Locale.OpenPackagePage;

				point = (instance as ItemListControl)!.GridView ? IconRect.Location : CenterRect.Location;

				return true;
			}

			if (DateRect.Contains(location))
			{
				var date = Item.GetWorkshopInfo()?.ServerTime ?? Item.GetLocalPackage()?.LocalTime;
				if (date.HasValue)
				{
					text = getFilterTip(string.Format(Locale.CopyToClipboard, date.Value.ToLocalTime().ToString("g")), Locale.FilterSinceThisDate);
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
