using Syncfusion.Drawing;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;
using Syncfusion.Pdf.Graphics;

namespace SampleBrowser
{
    public partial class MailAttachment
    {
        public MailAttachment()
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
                    this.Description.FontSize = 18.5;
                }
                else
                {
                    this.Description.FontSize = 13.5;
                }
                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
            }
        }
    }

    public class MailAttachmentCommand : CommandBase
    {
        #region Constructros
        public MailAttachmentCommand()
        {

        }
        #endregion

        #region Implementation
        protected override async void ExecuteCommand(object parameter)
        {
            MailAttachmentSample();
        }
        private async void MailAttachmentSample()
        {
            MemoryStream stream = new MemoryStream();

            //Create a new PDF document
            using (PdfDocument document = new PdfDocument())
            {
                //Add page to the PDF document.
                PdfPage page = document.Pages.Add();

                //Create graphics instance.
                PdfGraphics g = page.Graphics;

                //Create font object              
                PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12.5f);

                //Create a new PDF Brush
                PdfBrush brush = PdfBrushes.Black;

                string text = "Lorem Ipsum dolor sit amet, consectetuer adipiscingelit. Duis tellus. Donec ante dolor, iaculis nec, gravidaac, cursus in, eros. Mauris vestibulum, felis et egestasullamcorper, purus nibh vehicula sem, eu egestas antenisl non justo. Fusce tincidunt, lorem nev dapibusconsectetuer, leo orci mollis ipsum, eget suscipit erospurus in ante.";

                //Draw the text to the PDF page
                g.DrawString(text, font, brush, new RectangleF(0, 50,page.GetClientSize().Width, 200));

                //Save the PDF document
                document.Save(stream);
            }

            stream.Position = 0;

            //Open in default system viewer.
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<IMailService>().ComposeMail("MailAttachment.pdf",null,"Email","Syncfusion",stream);
            else
                Xamarin.Forms.DependencyService.Get<IMailService>().ComposeMail("MailAttachment.pdf", null, "Email", "Syncfusion", stream);
        }
        #endregion
    }
}
