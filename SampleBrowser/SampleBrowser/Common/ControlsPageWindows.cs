using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleBrowser
{
    public class ControlsPageWindows : MultiPage<ContentPage>
    {
        private readonly ContentPage homePage;

        private readonly ControlListViewModel controlList;

        public ControlsPageWindows()
        {
            homePage = new ContentPage();

            BackgroundColor = Color.White;
            controlList = new ControlListViewModel();

            var mainContent = new StackLayout();

            mainContent.Children.Add(GetHeaderLayout());
            mainContent.Children.Add(GetControlLayout());

            homePage.Content = mainContent;
            Children.Add(homePage);
        }

        private static View GetHeaderLayout()
        {
            var stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#FF1196CD")
            };

            var controlIcon = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Icons/logo.png")
            };
            stackLayout.Padding = new Thickness(70, 0, 0, 0);
            stackLayout.Children.Add(controlIcon);

            return stackLayout;
        }

        private View GetControlLayout()
        {
            var sampleList = controlList.MasterSampleLists;
            var count = sampleList.Count;

            var rootGrid = new Grid();

            int singleColumnWidth = 0;
            int boxViewWidth = 0;
            int boxViewHeight = 0;
            if (App.Platform == Platforms.Windows81)
            {
                singleColumnWidth = 250;
                boxViewHeight = 166;
                boxViewWidth = 166;
            }
            else
            {
                singleColumnWidth = 200;
                boxViewHeight = 130;
                boxViewWidth = 150;
            }

            const double spacing = 3;
            rootGrid.VerticalOptions = LayoutOptions.CenterAndExpand;
            const int totalRow = 3;
            var totalColumn = count/totalRow;
            rootGrid.Padding = new Thickness(10);

            for (var i = 0; i < totalRow; i++)
            {
                rootGrid.RowDefinitions.Add(new RowDefinition { Height = boxViewHeight });
            }

            for (var i = 0; i <= totalColumn; i++)
            {
                rootGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = boxViewWidth });
            }

            var currentRow = -1;
            var currentColumn = 0;
            rootGrid.WidthRequest = (totalColumn*singleColumnWidth) + (totalColumn*spacing);

            for (var i = 0; i < count; i++)
            {
                currentRow++;

                var control = sampleList[i];

                var content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    WidthRequest = singleColumnWidth,
                    HeightRequest = 50,
                    StyleId = i.ToString()
                };

                var tapGestue = new TapGestureRecognizer();
                content.GestureRecognizers.Add(tapGestue);
                tapGestue.Tapped += TapGestue_Tapped;

                Image typeIcon;

                if (control.Type != null)
                {
                    typeIcon = new Image
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromFile("Icons/" + control.Type + ".png")
                    };
                }
                else
                {
                    typeIcon = new Image
                    {
                        Opacity = 0,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromFile("Icons/new.png")
                    };
                }

                var controlIcon = new Image
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Aspect = Aspect.AspectFit,
                    Source = ImageSource.FromFile("Icons/" + control.ImageID)
                };

                if (Device.Idiom == TargetIdiom.Desktop)
                    controlIcon.HeightRequest = 50;

                var controlName = new Label
                {
                    TextColor = Color.White,
                    Text = control.Title,
                    FontSize = 12,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                content.BackgroundColor = Color.FromHex("#FF86BA35");

                content.Children.Add(typeIcon);

                content.Children.Add(controlIcon);
                content.Children.Add(controlName);

                content.HorizontalOptions = LayoutOptions.Center;

                rootGrid.Children.Add(content);

                Grid.SetRow(content, currentRow);
                Grid.SetColumn(content, currentColumn);

                if (currentRow != 2) continue;
                currentRow = -1;
                currentColumn++;
            }

            rootGrid.Padding = new Thickness(80, 100, -90, 100);

            var controlScroller = new CustomScrolView
            {
                Orientation = ScrollOrientation.Horizontal,
                Content = rootGrid,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var verticalScroller = new CustomScrolView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = controlScroller,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
           
            return verticalScroller;
        }

        private void TapGestue_Tapped(object sender, EventArgs e)
        {
            ListViewItemsChanged(controlList.MasterSampleLists[int.Parse(((StackLayout) sender).StyleId)]);
        }

        private async void ListViewItemsChanged(MasterSample item)
        {
            await Task.Delay(5);

            if (Device.OS == TargetPlatform.iOS)
            {
                Title = "Back";
            }

            if (item == null) return;

            Children.Clear();
            Children.Add(new MasterSamplePageWindows(item, this, homePage));
        }

        protected override ContentPage CreateDefault(object item)
        {
            return null;
        }
    }
}