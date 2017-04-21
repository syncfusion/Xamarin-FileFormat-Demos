using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using SampleBrowser;
using SampleBrowser.Droid;

[assembly: Dependency(typeof(AndroidVersionDependencyService))]
namespace SampleBrowser.Droid
{
    public class AndroidVersionDependencyService : IAndroidVersionDependencyService
    {
        public int GetAndroidVersion()
        {
            return (int)Android.OS.Build.VERSION.SdkInt;
        }
    }
}