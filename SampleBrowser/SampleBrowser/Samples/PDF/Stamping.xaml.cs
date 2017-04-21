#region Copyright Syncfusion Inc. 2001 - 2017
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Reflection;
using Xamarin.Forms;
using System.IO;

namespace SampleBrowser
{
    public partial class Stamping
    {
        public Stamping()
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
            StampingCommand.IsToggled = true;
            switch1.Toggled += Switch1_Toggled;
        }
        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            StampingCommand.IsToggled = e.Value;
        }
    }

    public class StampingCommand : CommandBase
    {
        public static bool IsToggled = true;

        #region Constructros
        public StampingCommand()
        {

        }
        #endregion

        #region Implementation
        protected override async void ExecuteCommand(object parameter)
        {
            StampingSample();
        }
        private async void StampingSample()
        {
            //Load PDF document to stream.
            Stream docStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets.Syncfusion_Windows8_whitepaper.pdf");

            MemoryStream stream = new MemoryStream();

            //Load the PDF document into the loaded document object.
            using (PdfLoadedDocument ldoc = new PdfLoadedDocument(docStream))
            {
                //Create font object.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 100f, PdfFontStyle.Regular);

                //Stamp or watermark on all the pages.
                foreach (PdfPageBase lPage in ldoc.Pages)
                {
                    PdfGraphics g = lPage.Graphics;
                    PdfGraphicsState state = g.Save();
                    g.SetTransparency(0.25f);
                    g.TranslateTransform(50, lPage.Size.Height / 2);
                    g.RotateTransform(-40);
                    g.DrawString("Syncfusion", font, PdfPens.Red, PdfBrushes.Red, new PointF(0, 0));
                    g.Restore(state);
                }

                //Save the PDF document
                ldoc.Save(stream);                
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
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("Stamping.pdf", "application/pdf", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("Stamping.pdf", "application/pdf", stream);
            }
        }
        #endregion
    }
}
