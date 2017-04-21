using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;

namespace SampleBrowser
{
    public partial class MergePDF
    {
        public MergePDF()
        {
            InitializeComponent();
            PDFViewModel.Navigation = ContentView.Navigation;
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Description.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;
                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.BackgroundColor = Xamarin.Forms.Color.Gray;
            }
            else if (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                if (!SampleBrowser.App.isUWP)
                {
                    this.Description.FontSize = this.ViewerLable.FontSize = 18.5;
                }
                else
                {
                    this.Description.FontSize = this.ViewerLable.FontSize = 13.5;
                }
                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;

            }
            MergePDFCommand.IsToggled = true;
            switch1.Toggled += Switch1_Toggled;
        }

        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            MergePDFCommand.IsToggled = e.Value;
        }
    }

    public class MergePDFCommand : CommandBase
    {
        public static bool IsToggled = true;

        #region Constructros
        public MergePDFCommand()
        {

        }
        #endregion

        #region Implementation
        protected override async void ExecuteCommand(object parameter)
        {
            MergeSample();
        }
        private async void MergeSample()
        {
            //Load PDF document to stream.
            Stream docStream1 = typeof(MergePDF).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets.Essential_Pdf.pdf");
            Stream docStream2 = typeof(MergePDF).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets.Essential_XlsIO.pdf");

            MemoryStream stream = new MemoryStream();

            //Load the existing documents
            using (PdfLoadedDocument ldoc = new PdfLoadedDocument(docStream1))
            {

                using (PdfLoadedDocument ldoc1 = new PdfLoadedDocument(docStream2))
                {
                    //Merge the PDF documents 
                    PdfDocument.Merge(ldoc, ldoc1);

                    //Save the document
                    ldoc.Save(stream);
                }                
            }

            stream.Position = 0;

            if (IsToggled)
            {
                //Open in Essential PDF viewer.
                PdfViewerUI pdfViewer = new SampleBrowser.PdfViewerUI();
                pdfViewer.PdfDocumentStream = stream;
                if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
                {
                    await PDFViewModel.Navigation.PushModalAsync(new NavigationPage(pdfViewer));
                }
                else
                {
                    await PDFViewModel.Navigation.PushAsync(pdfViewer);
                }
            }
            else
            {
                //Open in default system viewer.
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("MergePDF.pdf", "application/pdf", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("MergePDF.pdf", "application/pdf", stream);
            }
        }
        #endregion
    }
}
