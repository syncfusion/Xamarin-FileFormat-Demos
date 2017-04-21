using System;
using Xamarin.Forms;

namespace SampleBrowser.Common
{
    public class MasterSamplePageWP : ContentPage
    {
        Grid contentRootLayout;

        SamplePage samplePage;

        ListView listView;

        SampleDetails sampleDetails;

        Image button;

        StackLayout settingLayout;
        public MasterSamplePageWP(MasterSample sampleList)
        {
            contentRootLayout = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = GridLength.Auto },
                }
            };

            settingLayout = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand, Padding = new Thickness(15) };

            if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
            {
                settingLayout.Padding = new Thickness(5,0,0,5);
                settingLayout.HeightRequest = 30;
            }

            button = new Image();
            if (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone)
                NavigationPage.SetHasNavigationBar(this, false);
            Title = Device.OS == TargetPlatform.Android ? "  " + sampleList.Title : sampleList.Title;


            listView = new ListView
            {
                ItemsSource = sampleList.Samples,
                ItemTemplate = new DataTemplate(typeof(SampleListCell)),
                RowHeight = 45
            };

            if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                listView.RowHeight = 35;

            var contentLayout = new StackLayout { Children = { listView } };
            if (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone)
                contentLayout.Padding = new Thickness(0, 0, 0, 60);
            var master = new ContentPage { Title = "Sample List", Content = contentLayout };


            samplePage = Activator.CreateInstance(Type.GetType(sampleList.Samples[0].Type)) as SamplePage;


            listView.ItemSelected += (sender, args) =>
            {
                if (listView.SelectedItem == null) return;

                sampleDetails = args.SelectedItem as SampleDetails;
                App.SelectedSample = sampleDetails.Title;
                var type = Type.GetType(sampleDetails.Type);
                if (type == null)
                {
                    ChangeSample(new EmptyContent().ContentView);
                }
                else
                {
                    samplePage = Activator.CreateInstance(type) as SamplePage;

                    if (samplePage.ToolbarItems.Count > 0)
                    {
                        Image toolbarItem = new Image {StyleId = "Settings" };
                        toolbarItem.Source = App.IsDark ? "Assets/Setting_Light.png" : "Assets/Setting.png" ;
                        if (!settingLayout.Children.Contains(toolbarItem) && settingLayout.Children.Count <= 1)
                            settingLayout.Children.Add(toolbarItem);
                       
                        var tapGesture1 = new TapGestureRecognizer();
                        tapGesture1.Tapped += (sender1, args1) =>
                        {
                            if (toolbarItem.StyleId == "Settings")
                            {
                                samplePage.ShowSettingsView();
                                toolbarItem.StyleId = "Apply";
                                toolbarItem.Source = App.IsDark ? "Assets/Apply_Light.png" : "Assets/Apply.png";
                                button.IsVisible = false;
                            }
                            else
                            {
                                samplePage.HideSettingsView();
                                toolbarItem.StyleId = "Settings";
                                toolbarItem.Source = App.IsDark ? "Assets/Setting_Light.png" : "Assets/Setting.png";
                                if (sampleList.Samples.Count != 1)
                                    button.IsVisible = true;
                            }
                        };
                        toolbarItem.GestureRecognizers.Add(tapGesture1);
                    }
                    else
                    {
                        if (settingLayout.Children.Count > 1)
                            settingLayout.Children.RemoveAt(1);
                    }
                    ChangeSample(samplePage.Content);
                }
                //listView.SelectedItem = null;
             //   button.IsVisible = true;
            };

            if (sampleList.Samples.Count > 0)
            {
                listView.SelectedItem = sampleList.Samples[0];
            }
          
            contentRootLayout.Children.Add(samplePage.Content);
            button = new Image() { StyleId = "Samples List" };
            var tapGesture = new TapGestureRecognizer();

            tapGesture.Tapped += (sender1, args1) =>
            {
                OnSampleChanged(samplePage.Content);
            };
            button.GestureRecognizers.Add(tapGesture);
            button.Source = App.IsDark ? "Assets/Controls_Light.png" : "Assets/Controls.png";
            settingLayout.Children.Insert(0, button);

            if (sampleList.Samples.Count == 1)
                button.IsVisible = false;

            contentRootLayout.Children.Add(settingLayout, 0, 1);
            Content = contentRootLayout;
        }

        private void OnSampleChanged(View obj)
        {
            if (contentRootLayout.Children.Count > 2)
                contentRootLayout.Children.RemoveAt(2);
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                this.samplePage.OnDisappear();
            settingLayout.IsVisible = false;
            ChangeSample(listView);
        }

        void ChangeSample(View view)
        {
            if (view == null) return;
          
            if (!(view is ListView))
            {
                settingLayout.IsVisible = true;
                isSampleList = false;
            }
            else
            {
                isSampleList = true;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                if (contentRootLayout.Children.Count > 1)
                    contentRootLayout.Children.RemoveAt(0);
                contentRootLayout.Children.Insert(0, view);
            });
        }

        bool isSampleList;
        protected override bool OnBackButtonPressed()
        {
            if (isSampleList)
            {
                listView.SelectedItem = null;
                listView.SelectedItem = sampleDetails;
                return true;
            }
            return false;
        }
    }
}