using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;

namespace SampleBrowser
{
	
	public class ViewExt : StackLayout
	{
        public double TotalWidth { get; set; }
		private Xamarin.Forms.DataTemplate itemTemplate;

		public Xamarin.Forms.DataTemplate ItemTemplate
		{
			get { return itemTemplate; }
			set
			{
				itemTemplate = value;
				itemTemplate.Bindings.Add(BindingContextProperty, new Binding { Source = this, Path = "BindingContext"});

			}

		}

		public IList ItemsSource { get; set; }

		public WeatherData SelectedItem;


	}
}


