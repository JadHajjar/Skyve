using Skyve.App.UserInterface.Generic;
using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Systems.Compatibility.Domain;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.CompatibilityReport;
public class PackageCompatibilityReportControl : SmartPanel
{
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly INotifier _notifier;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly Dictionary<ReportType, CompatibilitySectionPanel> _panels = [];
	private ReportItem? reviewStatusItem;

	public IPackageIdentity Package { get; }
	public ICompatibilityInfo? Report { get; private set; }

	public PackageCompatibilityReportControl(IPackageIdentity package)
	{
		ServiceCenter.Get(out _notifier, out _compatibilityManager, out _skyveDataManager);

		Package = package;
	}

	protected override async void OnCreateControl()
	{
		base.OnCreateControl();

		foreach (ReportType item in Enum.GetValues(typeof(ReportType)))
		{
			Controls.Add(_panels[item] = new CompatibilitySectionPanel(item));
		}

		_notifier.CompatibilityReportProcessed += CentralManager_PackageInformationUpdated;

		Reset();

		var reviewStatus = await _skyveDataManager.GetReviewStatus(Package);

		if (reviewStatus != null)
		{
			reviewStatusItem = new ReportItem
			{
				LocaleKey = reviewStatus.Message,
				LocaleParams = [Package.CleanName()],
				PackageId = reviewStatus.PackageId,
				PackageName = Package.CleanName(),
				Type = ReportType.RequestReview,
				Status = new GenericPackageStatus
				{
					Action = reviewStatus.RequestUpdate ? StatusAction.RequestReview : reviewStatus.Message is "ReviewPending" ? default : StatusAction.MarkAsRead,
					Notification = NotificationType.ReviewRequest
				}
			};

			_panels[ReportType.RequestReview].ReportItems.Add(reviewStatusItem);
			_panels[ReportType.RequestReview].Controls.Add(new CompatibilityMessageControl(this, ReportType.RequestReview, reviewStatusItem) { LinkToFollow = reviewStatus.Link });

			this.TryInvoke(Reset);
		}
	}

	protected override void Dispose(bool disposing)
	{
		_notifier.CompatibilityReportProcessed -= CentralManager_PackageInformationUpdated;

		base.Dispose(disposing);
	}

	private void CentralManager_PackageInformationUpdated()
	{
		this.TryInvoke(Reset);
	}

	public void Reset()
	{
		try
		{
			this.SuspendDrawing();

			lock (this)
			{
				Report = _compatibilityManager.GetCompatibilityInfo(Package, true);

				foreach (var panel in _panels.Where(x => x.Key is not ReportType.RequestReview))
				{
					var reportItems = Report.ReportItems.AllWhere(x => x.Type == panel.Key && (reviewStatusItem is null || x.Status.Action != StatusAction.RequestReview));

					if (reportItems.SequenceEqual(panel.Value.ReportItems))
					{
						continue;
					}

					panel.Value.ReportItems = reportItems;
					panel.Value.Controls.Clear(true);

					if (panel.Value.ReportItems.Count != 0)
					{
						panel.Value.Controls.AddRange(panel.Value.ReportItems.Reverse<ICompatibilityItem>().ToArray(x => new CompatibilityMessageControl(this, panel.Key, x)));
					}
				}
			}

			PerformLayout();
		}
		finally
		{
			this.ResumeDrawing();
		}
	}

	protected override void OnLayout(LayoutEventArgs levent)
	{
		const int preferredSize = 450;
		var columns = (int)Math.Max(1, Math.Floor((Width - Padding.Horizontal) / (preferredSize * UI.FontScale)));
		var columnWidth = (Width - Padding.Horizontal) / columns;
		var currentY = new int[columns];
		var index = 0;

		foreach (var panel in _panels
			.OrderBy(x => x.Value.ReportItems.All(_compatibilityManager.IsSnoozed))
			.ThenByDescending(x => x.Key == ReportType.RequestReview)
			.ThenByDescending(x => x.Value.ReportItems.Max(y => y.Status?.Notification))
			.ThenByDescending(x => x.Value.ReportItems.Sum(y => y.Packages.Count())))
		{
			if (panel.Value.ReportItems.Count == 0)
			{
				panel.Value.Visible = false;
				panel.Value.Location = default;

				continue;
			}

			var bounds = new Rectangle(Padding.Left + (index * columnWidth), currentY[index] + Padding.Top, columnWidth, panel.Value.Height + panel.Value.Margin.Vertical).Pad(panel.Value.Margin);

			if (panel.Value.Bounds != bounds)
			{
				panel.Value.Bounds = bounds;
			}

			panel.Value.Visible = true;

			currentY[index] += Padding.Top + panel.Value.Height + panel.Value.Margin.Vertical;

			index = (index + 1) % columns;
		}

		base.OnLayout(levent);
	}

	internal void RemoveReviewInfo()
	{
		reviewStatusItem = null;

		_panels[ReportType.RequestReview].ReportItems.Clear();
		_panels[ReportType.RequestReview].Controls.Clear(true);

		Reset();
	}
}
