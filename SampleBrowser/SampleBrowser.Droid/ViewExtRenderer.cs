using System;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using Android.Views;
using Xamarin.Forms;
using SampleBrowser;
using SampleBrowser.Droid;
using Android.Widget;
using Android.App;




[assembly: ExportRenderer(typeof(ViewExt), typeof(ViewExtRenderer))]
namespace SampleBrowser.Droid
{
	public class ViewExtRenderer : ViewRenderer<ViewExt, Android.Views.View>
	{
		
		protected override void OnElementChanged(ElementChangedEventArgs<ViewExt> e)
		{
			base.OnElementChanged(e);


			IList items = e.NewElement.ItemsSource;


			LinearLayout linearLayout = new LinearLayout(Forms.Context);
			linearLayout.VerticalScrollBarEnabled = true;
			//linearLayout.Orientation = Orientation;

			foreach (var item in items)
			{
				var templateLayout = e.NewElement.ItemTemplate.CreateContent() as Xamarin.Forms.View;

				templateLayout.BindingContext = item;

				var renderer = Convert(templateLayout, (VisualElement)e.NewElement.Parent);


				var native = renderer as Android.Views.View;

				var size = templateLayout.Measure(double.PositiveInfinity, double.PositiveInfinity);


				double heightt=App.ScreenHeight;
				double widthh = App.ScreenWidth;
				double density1 = App.Density - (App.Density * 0.22);

				double heightDensity = heightt / (density1);
				double widthDensity = widthh / (density1);



				var width = widthDensity/4;

				var height = size.Request.Height;

				templateLayout.Layout(new Rectangle(0, 0, width, height));

				float density = Forms.Context.Resources.DisplayMetrics.Density;

				native.LayoutParameters = new LinearLayout.LayoutParams((int)(width * density), (int)(height * density));

				//	linearLayout.SetBackgroundColor(Android.Graphics.Color.Red);

				linearLayout.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

				linearLayout.AddView(native);
			}

			LinearLayout root = new LinearLayout(Forms.Context);
			root.VerticalScrollBarEnabled = true;
			//root.Orientation = Orientation.Vertical;
			root.AddView(linearLayout);

			SetNativeControl(root);
		}

		public IVisualElementRenderer Convert(Xamarin.Forms.View source, Xamarin.Forms.VisualElement valid)
		{
			IVisualElementRenderer render = (IVisualElementRenderer)source.GetValue(RendererProperty);
			if (render == null)
			{
				render = Platform.CreateRenderer(source);
				source.SetValue(RendererProperty, render);
				var p = PlatformProperty.GetValue(valid);
				PlatformProperty.SetValue(source, p);
				IsPlatformEnabledProperty.SetValue(source, true);
			}

			return render;
		}


		private static Type _platformType = Type.GetType("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android", true);

		private static BindableProperty _rendererProperty;
		public static BindableProperty RendererProperty
		{
			get { return _rendererProperty ?? (_rendererProperty = (BindableProperty)_platformType.GetField("RendererProperty", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public).GetValue(null)); }
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


