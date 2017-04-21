using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser
{
    class PdfViewerViewModel: INotifyPropertyChanged
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

        

        public PdfViewerViewModel(Stream stream)
        {
            LoadPdfWithStream(stream);
        }


        private void LoadPdfWithStream(Stream stream)
        {
            if (stream != null)
                m_pdf = stream;
        }

    }
}
