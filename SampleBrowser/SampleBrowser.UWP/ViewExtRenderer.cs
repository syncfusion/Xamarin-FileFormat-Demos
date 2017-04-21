using Xamarin.Forms;
using System.Reflection;
using System.Collections;
using SampleBrowser;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SampleBrowser.UWP;
[assembly: ExportRenderer(typeof(ViewExt), typeof(ViewExtRenderer))]
namespace SampleBrowser.UWP
{
    public class ViewExtRenderer : ViewRenderer<ViewExt, StackPanel>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ViewExt> e)
        {
            UIElement native = null;

            IList items = e.NewElement.ItemsSource;

            StackPanel stackLayout = new StackPanel();

            stackLayout.Orientation = Windows.UI.Xaml.Controls.Orientation.Horizontal;
            double totalWidth = 0;
            if (e.NewElement.TotalWidth > 500)
            {
                totalWidth = e.NewElement.TotalWidth / 7;
            }


            foreach (var item in items)
            {
                var templateLayout = e.NewElement.ItemTemplate.CreateContent() as Xamarin.Forms.View;

                templateLayout.BindingContext = item;

                var renderer = Convert(templateLayout, (VisualElement)e.NewElement.Parent);

                native = renderer as UIElement;

                var size = templateLayout.GetSizeRequest(0, 0);

                templateLayout.Layout(new Rectangle(0, 0, totalWidth == 0 ? size.Request.Width : totalWidth, size.Request.Height));


                stackLayout.Children.Add(native);
            }

            SetNativeControl(stackLayout);
        }

        public IVisualElementRenderer Convert(Xamarin.Forms.View source, Xamarin.Forms.VisualElement valid)
        {
            IVisualElementRenderer render = source.GetOrCreateRenderer();
            var properties = typeof(VisualElement).GetRuntimeProperties();

            foreach (var item in properties)
            {
                if (item.Name == "IsPlatformEnabled")
                {
                    item.SetValue(source, true);
                }
            }

            properties = typeof(Element).GetRuntimeProperties();

            foreach (var item in properties)
            {
                if (item.Name == "Platform")
                {
                    object value = item.GetValue(valid);
                    item.SetValue(source, value);
                }
            }

            return render;
        }

        private static PropertyInfo _platform;
        public static PropertyInfo PlatformProperty
        {
            get
            {
                return _platform ?? (
              _platform = typeof(VisualElement).GetProperty("Platform", BindingFlags.NonPublic | BindingFlags.Instance));
            }
        }

        private static PropertyInfo _isplatformenabledprop;
        public static PropertyInfo IsPlatformEnabledProperty
        {
            get
            {
                return _isplatformenabledprop ?? (
              _isplatformenabledprop = typeof(VisualElement).GetProperty("IsPlatformEnabled", BindingFlags.NonPublic | BindingFlags.Instance));
            }
        }
    }
}
