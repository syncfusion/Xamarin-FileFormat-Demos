using System;
using Xamarin.Forms;

namespace SampleBrowser
{
    public class SamplePage : ContentPage
    {
        internal View Master { get; set; }

        internal bool IsPropertyViewVisible { get; set; }

        private readonly AbsoluteLayout contentLayout;

        private AbsoluteLayout propertyLayout;

        private ToolbarItem sampleListToolbarItem;

        private bool isSampleListVisible;

        private ToolbarItem toolbarItem;

        private View sampleList;

        private BoxView touchView;

        const double sampleListWidth_Phone = 240; // Width/100*40;
		const double sampleListWidth_Tablet = 280;

		Grid rootLayout;

        internal View SampleList
        {
            get { return sampleList; }
            set
            {
                sampleList = value;

                if (contentLayout.Children.Contains(sampleList)) return;
                contentLayout.Children.Add(sampleList);
                sampleList.IsVisible = false;
            }
        }

        public View ContentView
        {
            get { return (View)GetValue(ContentViewProperty); }
            set { SetValue(ContentViewProperty, value); }
        }

        public static readonly BindableProperty ContentViewProperty = BindableProperty.Create("ContentView", typeof(View), typeof(SamplePage), null, propertyChanged: OnContentViewChanged);

        private static void OnContentViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SamplePage)bindable).OnContentViewChanged();
        }

        private void OnContentViewChanged()
        {
            if (!Ext.IsWinPhone())
                touchView = new BoxView { BackgroundColor = Color.Gray, Opacity = 0.5, IsVisible = false };
            contentLayout.HeightRequest = 400;
            contentLayout.Children.Add(ContentView);

            if (touchView != null)
            {
                contentLayout.Children.Add(touchView);

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += tapGestureRecognizer_Tapped;
                touchView.GestureRecognizers.Add(tapGestureRecognizer);
            }

            if (Device.OS == TargetPlatform.Android)
                contentLayout.BackgroundColor = Color.White;
            if ((Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows) || Device.OS == TargetPlatform.WinPhone)
            {
                ContentView.HeightRequest = App.ScreenHeight - 120;
                ContentView.WidthRequest = App.ScreenWidth - 10;
            }

            if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
            {
                ContentView.WidthRequest = App.ScreenWidth - 20; 
                ContentView.HeightRequest = App.ScreenHeight - 65;
                contentLayout.Padding = new Thickness(5, 5, 0, 0);
            }
            rootLayout.Children.Clear();
            rootLayout.Children.Add(contentLayout);
            Content = rootLayout;
            
        }

        private void tapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!isSampleListVisible) return;
            if (touchView != null)
                touchView.IsVisible = false;
            HideSampleList();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            var margin = Device.Idiom == TargetIdiom.Tablet ? 25 : 10;
            var spacing = Device.OS == TargetPlatform.iOS ? 1 : 2;

            double toolbarItemHeight = (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone) ? 55 : 0;
            AbsoluteLayout.SetLayoutBounds(ContentView,
                new Rectangle(((Device.OS == TargetPlatform.iOS) ? margin / 2 : margin), margin,
                    contentLayout.Width - margin * spacing, contentLayout.Height - margin * spacing - toolbarItemHeight));

            if (touchView != null)
                AbsoluteLayout.SetLayoutBounds(touchView, new Rectangle(0, 0, width, height));

            if (PropertyView != null)
                AbsoluteLayout.SetLayoutBounds(PropertyView,
                    new Rectangle(margin, margin, contentLayout.Width - margin * spacing,
                        contentLayout.Height - margin * spacing));

            if (isSampleListVisible)
                PositionSampleList(width, height);
        }

        public void ShowSampleList()
        {
            if (sampleList == null) return;
            if (touchView != null)
                touchView.IsVisible = true;
            sampleList.IsVisible = true;
            sampleList.BackgroundColor = Color.White;
            PositionSampleList(Width, Height);
        }

        private void PositionSampleList(double width, double height)
        {
			if (Device.Idiom == TargetIdiom.Phone)
			{
				AbsoluteLayout.SetLayoutBounds(sampleList,
				                               new Rectangle(width - sampleListWidth_Phone, 0, sampleListWidth_Phone, height));
			}
			else
			{
				AbsoluteLayout.SetLayoutBounds(sampleList,
				                               new Rectangle(width - sampleListWidth_Tablet, 0, sampleListWidth_Tablet, height));
			}
        }

        public void HideSampleList()
        {
            if (sampleList == null) return;
            if (touchView != null)
                touchView.IsVisible = false;
            sampleList.IsVisible = false;
            isSampleListVisible = false;
        }

        public View PropertyView
        {
            get { return (View)GetValue(PropertyViewProperty); }
            set { SetValue(PropertyViewProperty, value); }
        }

        public static readonly BindableProperty PropertyViewProperty = BindableProperty.Create("PropertyView", typeof(View), typeof(SamplePage), null, propertyChanged: OnPropertyViewChanged);

        private static void OnPropertyViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SamplePage)bindable).AddSettingToolbar();
            if ((Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows) || Device.OS == TargetPlatform.WinPhone)
            {
                if (!(App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone))
                {
                    ((SamplePage)bindable).PropertyView.HeightRequest = App.ScreenHeight - 120;
                    ((SamplePage)bindable).PropertyView.WidthRequest = App.ScreenWidth - 10;
                }
                else if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                    ((SamplePage)bindable).PropertyView.WidthRequest = App.ScreenWidth - 20;
            }
        }

        public SamplePage()
        {
            contentLayout = new AbsoluteLayout() { VerticalOptions = LayoutOptions.FillAndExpand };

			if (Device.OS == TargetPlatform.iOS)
				Padding = new Thickness (0, 0, 0, 74);

            rootLayout = new Grid() { VerticalOptions = LayoutOptions.FillAndExpand };
        }

        private void toolbarItem_Clicked(object sender, EventArgs e)
        {
            if (toolbarItem.Text == "Settings")
            {
                ShowSettingsView();
            }
            else
            {
                HideSettingsView();
            }
        }

        private void AddSettingToolbar()
        {
            if (toolbarItem != null) return;
            toolbarItem = new ToolbarItem { Text = "Settings", Icon = "Setting.png" };
            toolbarItem.Clicked += toolbarItem_Clicked;
            ToolbarItems.Add(toolbarItem);

            propertyLayout = new AbsoluteLayout();

            if (App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                propertyLayout.Padding = new Thickness( 5, 0, 0, 0);

            if (PropertyView != null)
            {
                propertyLayout.Children.Add(PropertyView);
                if (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone)
                {
                    if (PropertyView is Layout<View>)
                        UpdatePicker(PropertyView as Layout<View>);
                }
            }
        }

        void UpdatePicker(Layout<View> layout)
        {
            foreach (var child in layout.Children)
            {
                if (child is Layout<View>)
                {
                    UpdatePicker(child as Layout<View>);
                }
                else if (child is Picker)
                {
                    (child as Picker).VerticalOptions = LayoutOptions.FillAndExpand;
                }
            }
        }

        internal void ShowSettingsView()
        {
            if (sampleListToolbarItem != null)
                ToolbarItems.Remove(sampleListToolbarItem);
            IsPropertyViewVisible = true;
            rootLayout.Children.Clear();
            rootLayout.Children.Add(propertyLayout);
            if (toolbarItem != null) { 
                toolbarItem.Text = "Apply";
                toolbarItem.Icon = "Apply.png";
             }
            HideSampleList();
        }

        internal void HideSettingsView()
        {
            if (sampleListToolbarItem != null)
                ToolbarItems.Insert(1, sampleListToolbarItem);
            rootLayout.Children.Clear();
            rootLayout.Children.Add(contentLayout);
            IsPropertyViewVisible = false;
            toolbarItem.Text = "Settings";
            toolbarItem.Icon = "Setting.png";
        }

        internal void UpdateSampleList()
        {
            sampleListToolbarItem = new ToolbarItem("List", "Controls.png", ValidateSampleList);
            ToolbarItems.Add(sampleListToolbarItem);
        }

        private void ValidateSampleList()
        {
            if (!isSampleListVisible)
            {
                isSampleListVisible = true;
                SampleList = Master;
                ShowSampleList();
            }
            else
            {
                isSampleListVisible = false;
                HideSampleList();
            }
        }

        internal void OnDisappear()
        {
            this.OnDisappearing();
        }
    }
}