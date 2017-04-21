using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Syncfusion.DataSource.Extensions;
using Syncfusion.DataSource;

namespace SampleBrowser
{
    public class CustomListView : ListView
    {

        public CustomListView()
        {
            this.BackgroundColor = Color.White;
        }

        protected override void SetupContent(Cell content, int index)
        {
            var viewCell = content as ViewCell;
            if ((this.ItemsSource as DisplayItems)[index] is GroupResult)
            {
                viewCell.View = new Header() { WidthRequest = this.Width };
            }
            else if (viewCell.View is Header)
            {
                viewCell.View = this.ItemTemplate.CreateContent() as View;
            }
            base.SetupContent(viewCell, index);
        }
    }

    public class Header : ContentView
    {
        Label label;
        public Header()
        {
            this.BackgroundColor = Color.FromHex("#D9D9D9");
            this.Padding = new Thickness(10, 0, 0, 0);
            label = new Label();
            label.TextColor = Color.Black;
            label.FontSize = 18;
            label.VerticalTextAlignment = TextAlignment.Center;
            label.HorizontalOptions = LayoutOptions.Start;
            label.VerticalOptions = LayoutOptions.Center;
            label.FontAttributes = FontAttributes.Bold;
            label.HeightRequest = 30;
            this.Content = label;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }
        protected override void OnBindingContextChanged()
        {
            if ((this.BindingContext is GroupResult))
            {
                var groupresult = this.BindingContext as GroupResult;
                label.Text += groupresult.Key.ToString();
            }
            base.OnBindingContextChanged();
        }
    }

    public class Row : StackLayout
    {
        protected override void OnSizeAllocated(double width, double height)
        {
            foreach (var child in this.Children)
            {
                child.WidthRequest = this.Width / 4;
                child.HeightRequest = 50;
            }
            base.OnSizeAllocated(width, height);
        }
    }

    public class CustomGrid : Grid
    {
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
        }
    }

    public class DisplayLabel : Label
    {
        public DisplayLabel()
        {
            if(Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                this.FontSize = 30;
            }
            else if(Device.OS == TargetPlatform.Android)
            {
                if (Device.Idiom == TargetIdiom.Phone)
                    this.FontSize = 30;
                else
                    this.FontSize = 32;
            }
            else
            {
                if (Device.Idiom == TargetIdiom.Phone)
                    this.FontSize = 30;
                else
                    this.FontSize = 32;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(50, 50);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(50, 50));
        }
    }

    public class ContentLabel : Label
    {
        public ContentLabel()
        {
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                this.FontSize = 16;
                this.HeightRequest = 20;
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                if (Device.Idiom == TargetIdiom.Phone)
                    this.FontSize = 16;
                else
                    this.FontSize = 18;
                this.HeightRequest = 20;
            }
            else
            {
                if (Device.Idiom == TargetIdiom.Phone)
                    this.FontSize = 16;
                else
                    this.FontSize = 18;
                this.HeightRequest = 20;
            }
        }
    }
}
