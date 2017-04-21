using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SampleBrowser
{
    public class MasterSamplePageWindows : ContentPage
    {
        private readonly AbsoluteLayout rootContentView = new AbsoluteLayout();
        private View sampleView;
        private ListView listView;
        private Image optionsImage;
        private View headerView;
        private bool isPropertyVisible;
        private View propertyView;
        private StackLayout propertyStackLayout;

        public MasterSamplePageWindows(MasterSample sample, MultiPage<ContentPage> mainPage, ContentPage rootPage)
        {
            propertyStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,

                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#FFEDEDEB"),
                Padding = new Thickness(10,0,10,10)
            };

            var headerStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            var optionsLabel = new Label
            {
                TextColor = Color.FromHex("#1196CD"),
                Text = "Options",
                
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            headerStackLayout.Children.Add(optionsLabel);

            //AbsoluteLayout.SetLayoutBounds(optionsLabel,);

            var optionsHeaderImage = new Image
            {

                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Icons/back.png")
            };


            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += tapGesture_Tapped;
            optionsHeaderImage.GestureRecognizers.Add(tapGesture);
            headerStackLayout.Children.Add(optionsHeaderImage);

            propertyStackLayout.Children.Add(headerStackLayout);

            var mainContent = new StackLayout { Spacing = 0 };
            headerView = GetHeaderLayout(sample, mainPage, rootPage);
            mainContent.Children.Add(headerView);
            mainContent.Children.Add(GetControlLayout(sample));
            Content = mainContent;
        }

        private View GetHeaderLayout(MasterSample sample, MultiPage<ContentPage> mainPage, ContentPage rootPage)
        {
            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#FF1196CD"),
            };

            var controlIcon = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                //Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Icons/back.png")
            };
            stackLayout.Children.Add(controlIcon);

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, args) =>
            {
                mainPage.Children.Clear();
                mainPage.Children.Add(rootPage);
                if(listView != null)
                    listView.SelectedItem = sample.Samples[0];
            };
            controlIcon.GestureRecognizers.Add(tapGesture);

            stackLayout.Children.Add(new Label
            {
                TextColor = Color.White,
                Text = sample.Title,
                FontSize = 40,
                VerticalOptions = LayoutOptions.Center
            });

            optionsImage = new Image
            {
              
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Icons/options.png")
            };
            

            tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += tapGesture_Tapped;
            optionsImage.GestureRecognizers.Add(tapGesture);

            return stackLayout;
        }

        private void tapGesture_Tapped(object sender, EventArgs e)
        {
            if (propertyView == null) return;
            if (isPropertyVisible)
            {
                if (listView != null)
                    rootContentView.Children.Add(listView);
                isPropertyVisible = false;
                propertyView.IsVisible = false;
            }
            else
            {
                if (listView != null)
                    rootContentView.Children.Remove(listView);
                isPropertyVisible = true;
                propertyView.IsVisible = true;
            }
        }

        private View GetControlLayout(MasterSample sampleList)
        {
            if (sampleList.Samples.Count == 1)
            {
                var type = Type.GetType(sampleList.Samples[0].Type);
                if (type == null) return rootContentView;
                var samplePage = Activator.CreateInstance(type) as SamplePage;
                SelectSample(samplePage);
            }
            else if (sampleList.Samples.Count > 1)
            {
                listView = new ListView
                {
                    ItemsSource = sampleList.Samples,
                    RowHeight = 40,
                    ItemTemplate = new DataTemplate(typeof (SampleListCell)),
                    BackgroundColor = Color.FromHex("#FFEDEDEB")
                };

                rootContentView.Children.Add(listView);
                SampleDetails sampleDetails = null;
                SamplePage previouseSeletedSample = null;
                listView.ItemSelected += (sender, args) =>
                {
                    if (listView.SelectedItem == null)
                        return;
                    if (previouseSeletedSample != null)
                        previouseSeletedSample.OnDisappear();
                    if (sampleDetails != null)
                        sampleDetails.IsSelected = false;
                    sampleDetails = args.SelectedItem as SampleDetails;
                    App.SelectedSample = sampleDetails.Title;
                    sampleDetails.IsSelected = true;
                    var type = Type.GetType(sampleDetails.Type);
                    if (type != null)
                    {
                        var samplePage = Activator.CreateInstance(type) as SamplePage;
                        SelectSample(samplePage);
                        previouseSeletedSample = samplePage;                                      
                    }
                    listView.SelectedItem = null;
                };

                listView.SelectedItem = sampleList.Samples[0];
            }

            return rootContentView;
        }

        private void SelectSample(SamplePage samplePage)
        {
            if (sampleView != null)
                rootContentView.Children.Remove(sampleView);
            sampleView = samplePage.ContentView;
            rootContentView.Children.Add(sampleView);

            if (samplePage.PropertyView == null)
            {
                if (propertyStackLayout != null && propertyStackLayout.Children.Count > 1)
                    propertyStackLayout.Children.RemoveAt(1);
                if(headerView is StackLayout)
                    (headerView as StackLayout).Children.Remove(optionsImage);
            }
            else
            {
				if (propertyStackLayout.Children.Count > 1)
                    propertyStackLayout.Children.RemoveAt(1);
                propertyStackLayout.Children.Add(samplePage.PropertyView);
                propertyView = propertyStackLayout;

                if(headerView is StackLayout)
                    (headerView as StackLayout).Children.Add(optionsImage);

                rootContentView.Children.Add(propertyView);
                propertyView.IsVisible = false;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            height = height - 100;

            double currentLeft = 0;
            if (listView != null)
            {
                if (!isPropertyVisible)
                {
                    AbsoluteLayout.SetLayoutBounds(listView, new Rectangle(0, 0, 300, height));
                    currentLeft += 300;
                }
                else
                {
                    AbsoluteLayout.SetLayoutBounds(listView, new Rectangle(-300, 0, 0, height));
                }
            }

            if (propertyView != null)
            {
                if (isPropertyVisible)
                {
                    AbsoluteLayout.SetLayoutBounds(propertyView,
                        new Rectangle(width - 300, 0, 300, height - 20));
                }
                else
                {
                    AbsoluteLayout.SetLayoutBounds(propertyView, new Rectangle(0, 0, 0, height));
                }
            }

            if (sampleView != null)
                AbsoluteLayout.SetLayoutBounds(sampleView,
                    isPropertyVisible
                        ? new Rectangle(currentLeft + 20, 20, width - currentLeft - 40 - 300, height - 20)
                        : new Rectangle(currentLeft + 20, 20, width - currentLeft - 40, height - 20));

            base.OnSizeAllocated(width, height);
        }
    }
}