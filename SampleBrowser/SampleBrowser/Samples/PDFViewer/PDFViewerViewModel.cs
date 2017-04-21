using System;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace SampleBrowser
{
    public class PDFViewerViewModel : INotifyPropertyChanged
	{
		private Stream m_pdf;
		public event PropertyChangedEventHandler PropertyChanged;

		public Stream PDF
		{
			get
			{
				return m_pdf;
			}
			set
			{
				m_pdf = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("PDF"));
				}
			}
		}

		public PDFViewerViewModel()
		{
			LoadPdf();
		}

        private void LoadPdf()
		{
			m_pdf = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDFViewer.Assets.GIS Succinctly.pdf");
		}
    }
}
