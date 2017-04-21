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
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Tables;
using System.Reflection;
using Xamarin.Forms;
using System.IO;
using System.Xml.Linq;

namespace SampleBrowser
{
    public partial class TableFeatures
    {
        public TableFeatures()
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
            TableFeaturesCommand.IsToggled = true;
            switch1.Toggled += Switch1_Toggled;
        }

        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            TableFeaturesCommand.IsToggled= e.Value;           
        }
    }

    public class TableFeaturesCommand : CommandBase
    {
        #region Constructros
        public TableFeaturesCommand()
        {

        }
        #endregion

        #region Implementation

        public static bool IsToggled = true;


        protected override async void ExecuteCommand(object parameter)
        {
           TableFeaturesSample(); 
        }
        private async void TableFeaturesSample()
        {
            #region Field Definitions
            //Load product data.
            IEnumerable<Products> products = DataProvider.GetProducts();

            //Create a new PDF standard font
            PdfStandardFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 8f);

            PdfStandardFont smallFont = new PdfStandardFont(font, 5f);

            PdfFont bigFont = new PdfStandardFont(font, 16f);

            //Create a new PDF solid brush
            PdfBrush orangeBrush = new PdfSolidBrush(new PdfColor(247, 148, 29));

            PdfBrush grayBrush = new PdfSolidBrush(new PdfColor(170, 171, 171));

            //Create a new PDF pen
            PdfPen borderPen = new PdfPen(PdfBrushes.DarkGray, .3f);

            borderPen.LineCap = PdfLineCap.Square;

            PdfPen transparentPen = new PdfPen(PdfBrushes.Transparent, .3f);

            transparentPen.LineCap = PdfLineCap.Square;

            float margin = 40f;
            #endregion

            MemoryStream stream = new MemoryStream();

            //Create a new PDF document
            using (PdfDocument document = new PdfDocument())
            {
                //Set the margins
                document.PageSettings.Margins.All = 0;

                //Add a new PDF page
                PdfPage page = document.Pages.Add();

                //Add the document header
                AddHeader(document, "Syncfusion Essential PDF");

                //Add the document footer
                AddFooter(document, font, grayBrush);

                //Draw the text to the PDF page
                page.Graphics.DrawString("What You Get with Syncfusion", bigFont, orangeBrush, new PointF(10, 10));

                #region PdfGrid
                //Create a new PDF grid
                PdfGrid pdfGrid = new PdfGrid();

                IEnumerable<Report> report = DataProvider.GetReport();
                pdfGrid.DataSource = report;
                pdfGrid.Headers.Clear();
                pdfGrid.Columns[0].Width = 80;
                pdfGrid.Style.Font = font;
                pdfGrid.Style.CellSpacing = 15f;

                for (int i = 0; i < pdfGrid.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        PdfGridCell cell = pdfGrid.Rows[i].Cells[0];

                        cell.RowSpan = 2;

                        Stream imgStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets." + cell.Value.ToString().ToLower() + ".jpg");

                        //Set cell background image
                        cell.Style.BackgroundImage = new PdfBitmap(imgStream);

                        cell.Value = "";

                        cell = pdfGrid.Rows[i].Cells[1];

                        cell.Style.Font = bigFont;
                    }
                    for (int j = 0; j < pdfGrid.Columns.Count; j++)
                    {
                        pdfGrid.Rows[i].Cells[j].Style.Borders.All = transparentPen;
                    }
                }

                //Create a PDF grid layout format
                PdfGridLayoutFormat gridLayoutFormat = new PdfGridLayoutFormat();

                //Set pagination
                gridLayoutFormat.Layout = PdfLayoutType.Paginate;

                //Draw the grid to the PDF document
                pdfGrid.Draw(page, new RectangleF(new PointF(margin, 75), new SizeF(page.Graphics.ClientSize.Width - (2 * margin), page.Graphics.ClientSize.Height - margin)), gridLayoutFormat);
                #endregion

                //Save the PDF document 
                document.Save(stream);               
            }

            stream.Position = 0;

            if (IsToggled)
            {
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
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("TableFeatures.pdf", "application/pdf", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("TableFeatures.pdf", "application/pdf", stream);
            }
        }

        #endregion

        #region Helpher methods
        #region Header
        private void AddHeader(PdfDocument doc, string title)
        {
            RectangleF rect = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 50);

            //Create page template
            PdfPageTemplateElement header = new PdfPageTemplateElement(rect);

            //Create a new PDF standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 24);

            float doubleHeight = font.Height * 2;

            PdfColor activeColor = new PdfColor(24, 21, 104);

            SizeF imageSize = new SizeF(110f, 30f);

            //Locating the logo on the right corner of the Drawing Surface
            PointF imageLocation = new PointF(doc.Pages[0].GetClientSize().Width - imageSize.Width - 10, 5);

            Stream imgStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets.logo.jpg");

            PdfImage img = new PdfBitmap(imgStream);

            //Draw the image in the Header.
            header.Graphics.DrawImage(img, imageLocation, imageSize);

            //Create new PDF solid brush
            PdfSolidBrush brush = new PdfSolidBrush(activeColor);

            //Create a new PDF pen
            PdfPen pen = new PdfPen(new PdfColor(72, 61, 139), 3f);

            font = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);

            //Set formattings for the text
            PdfStringFormat format = new PdfStringFormat();

            format.Alignment = PdfTextAlignment.Center;

            format.LineAlignment = PdfVerticalAlignment.Middle;

            //Draw title
            header.Graphics.DrawString(title, font, brush, new RectangleF(0, 0, header.Width, header.Height), format);

            pen = new PdfPen(new PdfColor(24, 21, 104), 2f);

            header.Graphics.DrawLine(pen, 10, header.Height, header.Width - 10, header.Height);

            //Add header template at the top.
            doc.Template.Top = header;
        }
        #endregion

        #region Footer
        private void AddFooter(PdfDocument document, PdfFont font, PdfBrush grayBrush)
        {
            //Create a new footer template
            PdfPageTemplateElement footer = new PdfPageTemplateElement(new RectangleF(new PointF(0, document.PageSettings.Height - 40), new SizeF(document.PageSettings.Width, 40)));

            footer.Graphics.DrawRectangle(new PdfSolidBrush(new PdfColor(77, 77, 77)), footer.Bounds);

            footer.Graphics.DrawString("http://www.syncfusion.com", font, grayBrush, new PointF(footer.Width - (footer.Width / 4), 15));

            footer.Graphics.DrawString("Copyright © 2001 - 2012 Syncfusion Inc.", font, grayBrush, new PointF(10, 15));

            //Add the footer template at the bottom of the page
            document.Template.Bottom = footer;

        }
        # endregion
        #endregion
    }

    #region Helper classes
    internal sealed class DataProvider
    {
        public static IEnumerable<Products> GetProducts()
        {

            Stream xmlStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets.Products.xml");

            using (StreamReader reader = new StreamReader(xmlStream, true))
            {
                return XElement.Parse(reader.ReadToEnd())
                    .Elements("Products")
                    .Select(c => new Products
                    {
                        Image1 = c.Element("Image1").Value,
                        Description1 = c.Element("Description1").Value,
                        Image2 = c.Element("Image2").Value,
                        Description2 = c.Element("Description2").Value,
                        Image3 = (c.Element("Image3") != null) ? c.Element("Image3").Value : "dummy",
                        Description3 = (c.Element("Description3") != null) ? c.Element("Description3").Value : "dummy",
                    });
            }
        }

        public static IEnumerable<Report> GetReport()
        {
            Stream xmlStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDF.Assets.Report.xml");

            using (StreamReader reader = new StreamReader(xmlStream, true))
            {
                return XElement.Parse(reader.ReadToEnd())
                    .Elements("Report")
                    .Select(c => new Report
                    {
                        Image = c.Element("Image").Value,
                        Description = c.Element("Description").Value,
                    });
            }
        }
    }

    #region Products
    public class Products
    {
        public string Image1 { get; set; }
        public string Description1 { get; set; }
        public string Image2 { get; set; }
        public string Description2 { get; set; }
        public string Image3 { get; set; }
        public string Description3 { get; set; }
    }
    #endregion

    #region Report
    public class Report
    {
        public string Image { get; set; }
        public string Description { get; set; }
    }
    #endregion

    #endregion
}
