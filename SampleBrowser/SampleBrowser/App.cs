using System;
using Xamarin.Forms;

namespace SampleBrowser
{
    public class App : Application
    {
        public static double ScreenWidth;
        public static double ScreenHeight;
        public static double Density;
        public static bool isUWP;
        public static Platforms Platform;
        public static bool IsDark;
        public static string SelectedSample;
        public static ControlsPageWindows controlsExplorer;
        public static NavigationPage page;
        public App()
        {
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                MainPage = controlsExplorer = new ControlsPageWindows();
                
            }
            else
            {
                page = new NavigationPage(new ControlPage());
                page.BarBackgroundColor = Color.FromHex("#168DDB");
                page.BarTextColor = Color.White;
                MainPage = page;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public enum Platforms
    {
        UWP,
        Windows81,
        Android,
        iOS,
        WindowsPhone8,
        WindowsPhone81,
    }
}