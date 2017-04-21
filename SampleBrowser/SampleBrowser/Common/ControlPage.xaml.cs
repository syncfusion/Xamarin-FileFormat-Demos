using System;
using System.Linq;
using System.Threading.Tasks;
using SampleBrowser.Common;
using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class ControlPage : ContentPage
    {
        private readonly Label aboutContent = new Label();
        private readonly ListView rootList = new ListView();
        private readonly Grid rootGrid = new Grid();
        private readonly ControlListViewModel controlList;
        private readonly Label dummyContent;
        private readonly Grid rootLayout;
        private readonly StackLayout rootStackLayout;

        private readonly Label indicator = new Label
        {
            IsVisible = false,
            Text = "Loading...",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone)
                NavigationPage.SetHasNavigationBar(this, false);

            indicator.IsVisible = false;
			if (Device.OS == TargetPlatform.iOS)
			{
				Title = "Essential Studio";
			}
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                rootGrid.IsVisible = true;
            }
            else
            {
                rootList.IsVisible = true;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                int maxColumnCount;
                int maxRowCount;
                if (width > height)
                {
                    //Landscape
                    maxColumnCount = DeviceExt.OnPlatform(5, 6, 0);
                    maxRowCount = DeviceExt.OnPlatform(4, 3, 0);
                    if (Device.OS == TargetPlatform.Android)
                        rootGrid.HeightRequest = 800;
                }
                else
                {
                    //Portrait
                    maxColumnCount = 5;
                    maxRowCount = 5;
                    if (Device.OS == TargetPlatform.Android)
                        rootGrid.HeightRequest = 1200;
                }

                var currentColumn = 0;
                var currentRow = 0;

                Grid.SetRow(dummyContent, maxRowCount - 1);
                Grid.SetColumn(dummyContent, maxColumnCount - 1);

                foreach (var child in rootGrid.Children.Where(child => !(child is Label)))
                {
                    Grid.SetColumn(child, currentColumn);
                    Grid.SetRow(child, currentRow);

                    currentColumn++;
                    if (currentColumn != maxColumnCount) continue;
                    currentColumn = 0;
                    currentRow++;
                }
            }

            base.OnSizeAllocated(width, height);
        }

        public ControlPage()
        {
            rootLayout = new Grid();
            
            
            dummyContent = new Label();

            controlList = new ControlListViewModel();

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                
                Title = DeviceExt.OnPlatform("Essential Studio", "  Essential Studio", "Essential Studio");
                var sampleList = controlList.MasterSampleLists;
                var count = sampleList.Count;

                rootGrid = new Grid();
                rootLayout.Children.Add(rootGrid);
                rootGrid.Padding = new Thickness(10);

                for (var i = 0; i < count; i++)
                {
                    var control = sampleList[i];

					StackLayout content;

					if (Device.OS == TargetPlatform.iOS)
					{
						 content = new StackLayout { Padding = new Thickness(20, 20, 20, 20), StyleId = i.ToString() };
					}
					else
					{
						 content = new StackLayout { Padding = new Thickness(20, 40, 20, 20), StyleId = i.ToString() };
					}

                    var tapGestue = new TapGestureRecognizer();
                    content.GestureRecognizers.Add(tapGestue);
                    tapGestue.Tapped += TapGestue_Tapped;

                    var absoluteLayout = new AbsoluteLayout {HeightRequest = 76, WidthRequest = 76};
                    var controlIcon = new Image
                    {
                        HeightRequest = 76,
                        WidthRequest = 76,
                        Aspect = Aspect.AspectFit,
                    };
                    if (Device.OS == TargetPlatform.Windows)
                        controlIcon.Source = ImageSource.FromFile("chart.png");
                    else
                        controlIcon.Source = ImageSource.FromResource("SampleBrowser.Icons." + control.ImageID);
                    var sampleName = new Label
                    {
                        Text = control.Title,
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    content.Children.Add(absoluteLayout);
                    content.Children.Add(sampleName);
                    absoluteLayout.Children.Add(controlIcon);

                    absoluteLayout.HorizontalOptions = LayoutOptions.Center;

                    rootGrid.Children.Add(content);
                }

                if (Device.OS != TargetPlatform.iOS)
                    rootGrid.Children.Add(dummyContent);

                if (Device.OS == TargetPlatform.Android)
                    Content = new ScrollView() { Content = rootLayout };
                else
                    Content = rootLayout;
            }
            else
            {
                Title = "Essential Studio";

                rootList = new ListView();
                rootStackLayout = new StackLayout();

                if (!(Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS))
                {
                    Label title = new Label() { Text = " Essential Studio", FontSize = 30, TextColor = Color.Black };
                    rootStackLayout.Children.Add(title);
                    rootStackLayout.Children.Add(rootList);
                    rootLayout.Children.Add(rootStackLayout);
                }
                else
                {
                    rootLayout.Children.Add(rootList);
                }
                rootList.BackgroundColor = Color.White;
                rootLayout.BackgroundColor = Color.White;
                aboutContent.Text =
                    "Syncfusion Essential Studio is a collection of user interface and file format manipulation components that can be used to build line-of-business mobile applications.";
                aboutContent.TranslationX = 10;
                aboutContent.FontSize = 26;

                Title = Device.OS == TargetPlatform.Android ? "  Essential Studio" : "Essential Studio";
                rootList.ItemsSource = controlList.MasterSampleLists;
                rootList.ItemSelected += listview_ItemSelected;
                rootList.SeparatorColor = Color.FromHex("#B2B2B2");
                rootList.SeparatorVisibility = DeviceExt.OnPlatform(SeparatorVisibility.Default, SeparatorVisibility.Default, SeparatorVisibility.None);

                rootList.RowHeight = DeviceExt.OnPlatform(50, 67, 80);

                if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                    rootList.RowHeight = 55;

                rootList.ItemTemplate = new DataTemplate(typeof(ControlListCellMobile));
                Content = rootLayout;
            }
            rootLayout.Children.Add(indicator);
        }

        private void TapGestue_Tapped(object sender, EventArgs e)
        {
            ListViewItemsChanged(controlList.MasterSampleLists[int.Parse(((StackLayout) sender).StyleId)]);
        }

        private void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListViewItemsChanged(e.SelectedItem as MasterSample);
        }

        private async void ListViewItemsChanged(MasterSample item)
        {
            indicator.IsVisible = true;
            if (rootGrid != null)
                rootGrid.IsVisible = false;
            if (rootList != null)
                rootList.IsVisible = false;
            
            await Task.Delay(5);

			if (Device.OS == TargetPlatform.iOS)
			{
				Title = "Back";
			}

            if (item == null) return;

            if (Device.OS == TargetPlatform.WinPhone || (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows))
            {
                await Navigation.PushAsync(new MasterSamplePageWP(item));
            }
            else
            {
                await Navigation.PushAsync(new MasterSamplePage(item));
            }

            rootList.SelectedItem = null;
        }
    }

	internal class ControlListCellMobile : ViewCell
    {
        private readonly Image controlIcon;

        private readonly Grid rootLayout;

        public string Type
        {
            get { return (string) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly BindableProperty TypeProperty = BindableProperty.Create("Type", typeof(string), typeof(ControlListCellMobile), "", propertyChanged:OnTypePropertyChanged);

        private static void OnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
                ImageSource color = null;
                switch ((String)newValue)
                {
                    case "New":
                        color = ImageSource.FromResource("SampleBrowser.Icons.New.png");
                        break;
                    case "Preview":
                        color = ImageSource.FromResource("SampleBrowser.Icons.Preview.png");
                        break;
                    case "Updated":
                        color = ImageSource.FromResource("SampleBrowser.Icons.Updated.png");
                        break;
                }
                var icon = new Image
                {
                    Source = color,
                    HeightRequest = DeviceExt.OnPlatform(55, 20, 30),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.End,
                    WidthRequest = DeviceExt.OnPlatform(70, 100, 100),
                    Aspect = Aspect.AspectFit,

                };
                ((ControlListCellMobile)bindable).rootLayout.Children.Add(icon, 2, 0);
            
        }

        public string ImageID
        {
            get { return (string) GetValue(ImageIDProperty); }
            set { SetValue(ImageIDProperty, value); }
        }

        public static readonly BindableProperty ImageIDProperty = BindableProperty.Create("ImageID", typeof(string), typeof(ControlListCellMobile), null, propertyChanged: OnImageIDPropertyChanged);

        private static void OnImageIDPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ControlListCellMobile) bindable).UpdateImage();
        }

        private void UpdateImage()
        {
            if (controlIcon != null)
            {
                if (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone)
                    controlIcon.Source = ImageSource.FromFile("Icons/" + ImageID);
                else
                    controlIcon.Source = ImageSource.FromResource("SampleBrowser.Icons." + ImageID);

                if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                    controlIcon.Source = ImageSource.FromResource("SampleBrowser.Icons." + ImageID);
            }
        }

        public ControlListCellMobile()
        {
            controlIcon = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = DeviceExt.OnPlatform(32, 40, 70),
                WidthRequest = DeviceExt.OnPlatform(32, 40, 70),
                Aspect = Aspect.AspectFill
            };


            if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
            {
                controlIcon.HeightRequest = 50;
                controlIcon.WidthRequest = 50;
            }

            this.SetBinding(ImageIDProperty, "ImageID");
            this.SetBinding(TypeProperty, "Type");

            var controlName = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                TextColor =   Color.FromHex("#333D47"),
            };

            controlName.FontSize = DeviceExt.OnPlatform(14, 14, 18);

            if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                controlName.FontSize = 14;

            controlName.SetBinding(Label.TextProperty, "Title");

            rootLayout = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ColumnDefinitions = 
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) }
                }
            };

            rootLayout.Children.Add(controlIcon, 0, 0);
            rootLayout.Children.Add(controlName, 1, 0);

			rootLayout.Padding = DeviceExt.OnPlatform(new Thickness(15, 5, 15, 5), new Thickness(15),
                new Thickness(4, 3, 5, 4));

            rootLayout.BackgroundColor = Color.White;

            View = rootLayout;
        }
    }

    public static class Ext
    {
        public static bool IsWinPhone(){
            return (Device.OS == TargetPlatform.WinPhone || (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone));
        }
    }

//
//    internal class ControlListCellTablet : ViewCell
//    {
//        private readonly Image controlIcon;
//
//        public string ImageID
//        {
//            get { return (string)GetValue(ImageIDProperty); }
//            set { SetValue(ImageIDProperty, value); }
//        }
//
//        public static readonly BindableProperty ImageIDProperty =
//            BindableProperty.Create<ControlListCellTablet, string>(p => p.ImageID, null, BindingMode.Default,
//                null, OnImageIDPropertyChanged);
//
//        private static void OnImageIDPropertyChanged(BindableObject bindable, string oldValue, string newValue)
//        {
//            ((ControlListCellTablet)bindable).UpdateImage();
//        }
//
//        private void UpdateImage()
//        {
//            if (controlIcon != null)
//            {
//                controlIcon.Source = ImageSource.FromResource("SampleBrowser.Icons." + ImageID);
//            }
//        }
//
//        public ControlListCellTablet()
//        {
//            controlIcon = new Image
//            {
//                HeightRequest = 130,
//                WidthRequest = 130, 
//                Aspect = Aspect.AspectFill
//            };
//
//            this.SetBinding(ImageIDProperty, "ImageID");
//
//            var controlLabel = new StackLayout { Orientation = StackOrientation.Vertical };
//            var controlName = new Label();
//            controlName.SetBinding(Label.TextProperty, "Title");
//            controlName.FontSize = 24;
//            controlLabel.Children.Add(controlName);
//
//            var descriptionLabel = new Label
//            {
//                FontSize = 15
//            };
//            descriptionLabel.SetBinding(Label.TextProperty, "Description");
//            controlLabel.Children.Add(descriptionLabel);
//
//            var rootLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
//            //rootLayout.Padding = new Thickness(20);
//            rootLayout.Children.Add(controlIcon);
//            rootLayout.Children.Add(controlLabel);
//
//            rootLayout.Padding = new Thickness(5, 5, 5, 5);
//
//            rootLayout.VerticalOptions = LayoutOptions.Center;
//            View = rootLayout;
//        }
}