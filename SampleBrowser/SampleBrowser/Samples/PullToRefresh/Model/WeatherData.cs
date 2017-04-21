using System;
using Xamarin.Forms;

namespace SampleBrowser
{
	public class WeatherData 
	{
		

		public WeatherData (String day,String month,string temperature,String type,String selectedType)
		{
			
			Day = day;
			Month = month;
			Temperature = temperature;
			SelectedType=selectedType;
			Type=type;
			id=type;
		}

		public string Day {
			get;
			set;
		}
		public string Month {
			get;
			set;
		}

		public string Temperature {
			get;
			set;
		}

		public String SelectedType {
			get;
			set;
		}
		public String id {
			get;
			set;
		}
        private String type;
        public String Type
        {
            get { return type; }
            set { type = value;
                ImageName = ImageSource.FromResource("SampleBrowser.Icons." + type);
            }
        }

		public ImageSource ImageName {
			get;
			set;
		}
	}
}


