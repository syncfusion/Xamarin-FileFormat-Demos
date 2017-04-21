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
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FormsView), typeof(FormsViewRenderer))]
namespace SampleBrowser.Droid
{
    internal class FormsViewRenderer:ViewRenderer
    {
        public FormsView FormsView
        {
            get { return this.Element as FormsView; }
        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Visibility")
            {
                if (this.FormsView.Visibility)
                    this.Visibility = ViewStates.Visible;
                else
                    this.Visibility = ViewStates.Invisible;
            }
        }
    }
}