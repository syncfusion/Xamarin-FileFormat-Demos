using System;
using Syncfusion.SfChart.XForms.iOS;
using UIKit;
using CoreGraphics;
using Syncfusion.SfChart.iOS;
using Syncfusion.SfChart.XForms;

namespace SampleBrowser_Forms.iOS
{
	public class TooltipCustomDelegate: ChartDelegate
	{
		public TooltipCustomDelegate(SfChart chart):base(chart)
		{
			
		}

		public override void WillShowTooltip (SFChart chart, SFChartTooltip tooltipView)
		{
			UIView customView = new UIView ();
			customView.Frame = new CGRect (0,0,80,40);

			UIImageView imageView = new UIImageView ();
			imageView.Frame = new CGRect (0, 0, 40, 40);
			imageView.Image = UIImage.FromBundle ("grain.png");

			UILabel xLabel = new UILabel ();
			xLabel.Frame = new CGRect (47,0,35,18);
			xLabel.TextColor = UIColor.Orange;
			xLabel.Font = UIFont.FromName("Helvetica", 12f);
			xLabel.Text = tooltipView.DataPoint.XValue.ToString ();

			UILabel yLabel = new UILabel ();
			yLabel.Frame = new CGRect (47, 20, 35, 18);
			yLabel.TextColor = UIColor.White;
			yLabel.Font = UIFont.FromName("Helvetica", 15f);
			yLabel.Text = tooltipView.Text;

			customView.AddSubview (imageView);
			customView.AddSubview (xLabel);
			customView.AddSubview (yLabel);
			tooltipView.CustomView = customView;
		}
	}
}

