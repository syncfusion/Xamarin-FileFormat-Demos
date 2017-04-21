using Xamarin.Forms;

namespace SampleBrowser
{
    public class SampleDetailsPage : MultiPage<ContentPage>
    {
        public ContentPage Detail
        {
            get { return (ContentPage) GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        public static readonly BindableProperty DetailProperty = BindableProperty.Create("Detail", typeof(ContentPage), typeof(SampleDetailsPage), null, propertyChanged: OnDetailPropertyChanged);

        private static void OnDetailPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SampleDetailsPage) bindable).SwapChildren();
        }

        private void SwapChildren()
        {
            Children.Clear();
            if (Detail == null) return;

            Children.Add(Detail);

            var samplePage = Detail as SamplePage;

            if (Master == null || Device.OS == TargetPlatform.WinPhone || (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)) return;
            samplePage.Master = Master;
            samplePage.UpdateSampleList();
        }

        public View Master
        {
            get { return (View) GetValue(MasterProperty); }
            set { SetValue(MasterProperty, value); }
        }

        public static readonly BindableProperty MasterProperty = BindableProperty.Create("Master", typeof(View), typeof(SampleDetailsPage), null, propertyChanged: OnMasterPropertyChanged);

        private static void OnMasterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        protected override ContentPage CreateDefault(object item)
        {
            return null;
        }

		public SampleDetailsPage ()
		{
			if(Device.OS == TargetPlatform.iOS)
				Padding = new Thickness (0, 0, 0, -64);
		}
    }
}