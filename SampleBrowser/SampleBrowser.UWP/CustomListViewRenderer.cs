using SampleBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using System.Collections.Specialized;
using System.ComponentModel;
using Windows.Foundation;

//[assembly: ExportRenderer(typeof(CustomListView), typeof(SampleBrowser.UWP.CustomListViewRenderer))]
namespace SampleBrowser.UWP
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        internal Windows.UI.Xaml.Controls.ListView NativeListView
        {
            get { return this.Control as Windows.UI.Xaml.Controls.ListView; }
        }


        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (e.NewElement.ItemsSource != null)
                {
                    (e.NewElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += CustomListViewRenderer_CollectionChanged;
                }
            }
        }

        private void CustomListViewRenderer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.NativeListView != null)
            {
                var itemsource = this.NativeListView.ItemsSource;
                this.NativeListView.ItemsSource = null;
                this.NativeListView.ItemsSource = itemsource;
            }
        }
    }
}
