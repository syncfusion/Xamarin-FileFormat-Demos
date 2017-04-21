#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using SampleBrowser;
//using Syncfusion.SfChart.XForms.Droid;
//using Syncfusion.SfBusyIndicator.XForms.Droid;
//using Syncfusion.SfGauge.XForms.Droid;
//using Syncfusion.SfRangeSlider.XForms.Droid;
//using Syncfusion.RangeNavigator.XForms.Droid;

namespace SampleBrowser.Droid
{
    [Activity(Label = "Sample Browser", Theme = "@android:style/Theme.Holo.Light", MainLauncher =true, Icon = "@drawable/AppIcon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
		internal static MainActivity MainPageActivity; 
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			MainPageActivity=this;
            App.ScreenWidth = (Resources.DisplayMetrics.WidthPixels) * 0.75;
            App.Platform = Platforms.Android;
			App.ScreenHeight = (Resources.DisplayMetrics.HeightPixels) * 0.75;
            App.Density = Resources.DisplayMetrics.Density;
            Xamarin.Forms.Forms.Init(this, bundle);
            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);
            
            LoadApplication(new App());
            //SetContentView(new Button(this));
            
   //         new SfChartRenderer();

			//new SfBusyIndicatorRenderer ();

			//new SfDigitalGaugeRenderer ();

			//new SfLinearGaugeRenderer ();

			//new SfRangeSliderRenderer ();

			//new SfRangeNavigatorRenderer();

            ActionBar.SetDisplayOptions(ActionBarDisplayOptions.ShowHome, ActionBarDisplayOptions.ShowHome);

            ActionBar.SetIcon(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
        }
    }
}