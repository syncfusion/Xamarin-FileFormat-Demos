//using Syncfusion.SfChart.XForms.UWP;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SampleBrowser.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
			SampleBrowser.App.isUWP = true;
            this.InitializeComponent();
			SampleBrowser.App.ScreenWidth = Window.Current.Bounds.Width;
            SampleBrowser.App.ScreenHeight = Window.Current.Bounds.Height;
            if(IsMobile)
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            LoadApplication(new SampleBrowser.App());
        }

        public static bool IsMobile
        {
            get
            {
                var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
                return (qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "Mobile");
            }
        }
    }
}
