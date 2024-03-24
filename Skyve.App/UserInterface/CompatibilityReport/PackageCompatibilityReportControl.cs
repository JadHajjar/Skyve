using Skyve.App.UserInterface.Generic;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.CompatibilityReport;
public class PackageCompatibilityReportControl : SmartPanel
{
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly INotifier _notifier;
	private readonly Dictionary<ReportType, CompatibilitySectionPanel> _panels = [];

	public IPackageIdentity Package { get; }
	public ICompatibilityInfo? Report { get; private set; }

	public PackageCompatibilityReportControl(IPackageIdentity package)
	{
		ServiceCenter.Get(out _notifier, out _compatibilityManager);

		Package = package;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		foreach (ReportType item in Enum.GetValues(typeof(ReportType)))
		{
			Controls.Add(_panels[item] = new CompatibilitySectionPanel(item));
		}

		_notifier.CompatibilityReportProcessed += CentralManager_PackageInformationUpdated;

		Reset();
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

				foreach (var panel in _panels)
				{
					var reportItems = Report.ReportItems.AllWhere(x => x.Type == panel.Key);

					if (reportItems.SequenceEqual(panel.Value.ReportItems))
						continue;

					panel.Value.ReportItems = reportItems;
					panel.Value.Controls.Clear(true);

					if (panel.Value.ReportItems.Count != 0)
					{
						panel.Value.Controls.AddRange(panel.Value.ReportItems.Reverse<ICompatibilityItem>().ToArray(x => new CompatibilityMessageControl(this, panel.Key, x)));
					}
				}
			}
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
			.OrderBy(x => x.Key is not ReportType.Stability)
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
}
