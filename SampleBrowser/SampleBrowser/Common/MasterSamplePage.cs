using System;
//using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace SampleBrowser
{
    public class MasterSamplePage : SampleDetailsPage
    {
        ListView listView;
        MasterSample sampleList;

        public MasterSamplePage(MasterSample sampleList)
        {
            this.sampleList = sampleList;

            Title = Device.OS == TargetPlatform.Android ? "  " + sampleList.Title : sampleList.Title;

            if (sampleList.Samples.Count == 1)
            {
                var type = Type.GetType(sampleList.Samples[0].Type);
                if (type == null)
                {
                    Detail = new EmptyContent();
                }
                else
                {
                    var samplePage = Activator.CreateInstance(type) as SamplePage;
                    Detail = samplePage;
                }
            }
            else if (sampleList.Samples.Count > 1)
            {
                listView = new ListView
                {
                    ItemsSource = sampleList.Samples,
                    RowHeight = 40,
                    ItemTemplate = new DataTemplate(typeof(SampleListCell)),
                    BackgroundColor = Color.White,
                };

				if (Device.OS == TargetPlatform.iOS) 
				{
					StackLayout listStack = new StackLayout (){ Padding = new Thickness (0, 0, 0, 74) };
					listStack.Children.Add (listView);
					Master = listStack;
				} 
				else 
				{
					Master = listView;
				}
			
                listView.SeparatorColor = Color.FromHex("#B2B2B2");
                listView.SeparatorVisibility = DeviceExt.OnPlatform(SeparatorVisibility.Default, SeparatorVisibility.Default, SeparatorVisibility.None); 
                listView.ItemSelected += (sender, args) =>
                {
                    if (listView.SelectedItem == null)
                        return;

                    var sampleDetails = args.SelectedItem as SampleDetails;

                    var type = Type.GetType(sampleDetails.Type);
                    if (type == null)
                    {
                        Detail = new EmptyContent();
                    }
                    else
                    {
                        var samplePage = Activator.CreateInstance(type) as SamplePage;
                        Detail = samplePage;
                    }
                };

                SelectSample();
            }
        }

        async void SelectSample()
        {
            if (Device.OS == TargetPlatform.iOS && Device.Idiom == TargetIdiom.Phone)
                await Task.Delay(50);
            listView.SelectedItem = sampleList.Samples[0];
        }
    }

    internal class SampleListCell : ViewCell
    {
        private readonly Grid rootLayout;

		public string Type
		{
			get { return (string)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

        public static readonly BindableProperty TypeProperty = BindableProperty.Create("Type", typeof(string), typeof(SampleListCell), "", propertyChanged: OnTypePropertyChanged);

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
				default:
                    if (Device.OS == TargetPlatform.WinPhone)
                    {
                        color = ImageSource.FromResource("SampleBrowser.Icons.Empty.png");
                    }
                    break;
            }
            var icon = new Image
            {
                Source = color,
                HeightRequest = DeviceExt.OnPlatform(55, 20, 30),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = DeviceExt.OnPlatform(70, 100, 100),
                Aspect = Aspect.AspectFit,

            };
            ((SampleListCell)bindable).rootLayout.Children.Add(icon, 1, 0);
        }

        public SampleListCell()
        {
			this.SetBinding (TypeProperty, "SampleType");

            var sampleName = new LabelExt { VerticalOptions = LayoutOptions.Center };
            sampleName.SetBinding(Label.TextProperty, "Title");
            sampleName.VerticalOptions = LayoutOptions.Center;
            //if (Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone)
            //    sampleName.HorizontalOptions = LayoutOptions.FillAndExpand;

            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet)
                sampleName.WidthRequest = 210;
            sampleName.FontSize = Device.Idiom == TargetIdiom.Tablet
                ? DeviceExt.OnPlatform(15, 15, 25)
                : DeviceExt.OnPlatform(13, 13, 25);

			sampleName.VerticalOptions = LayoutOptions.Center;
            if ((Device.OS == TargetPlatform.Windows && Device.Idiom == TargetIdiom.Phone) || Device.OS == TargetPlatform.WinPhone)
            {
                sampleName.TextColor = Color.White;
                sampleName.FontSize = 23;
            }
            else
                sampleName.TextColor = Color.Black;

            if(App.Platform == Platforms.UWP && Device.Idiom == TargetIdiom.Phone)
                sampleName.FontSize = 15;

            rootLayout = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ColumnDefinitions = 
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(60, GridUnitType.Absolute) }
                }
            };

            if (Device.OS != TargetPlatform.Android)
                rootLayout.BackgroundColor = Color.White;
            sampleName.TextColor = Color.FromHex("#333D47");
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                sampleName.FontSize = 15;
                sampleName.SetBinding(Label.TextColorProperty, new Binding("ForegroundColor"));
                rootLayout.SetBinding(Layout.BackgroundColorProperty, new Binding("BackgroundColor"));
            }

            rootLayout.VerticalOptions = LayoutOptions.Center;

            if (Device.OS == TargetPlatform.Android)
                rootLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
            else
                rootLayout.HorizontalOptions = LayoutOptions.Center;

            rootLayout.Children.Add(sampleName, 0, 0);

            rootLayout.Padding = DeviceExt.OnPlatform(new Thickness(15, 10, 5, 5) , new Thickness(15, 10, 5, 5),
                new Thickness(4, 3, 5, 3));

            if (Ext.IsWinPhone())
            {
                StackLayout stackLayout = new StackLayout
                {
                    Children = { rootLayout },
                    BackgroundColor = Color.White,
                    Padding = 0,
                    Spacing = 0,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };

                View = stackLayout;
            }
            else
                View = rootLayout;
        }
    }

    public class LabelExt : Label
    {
       
    }
}