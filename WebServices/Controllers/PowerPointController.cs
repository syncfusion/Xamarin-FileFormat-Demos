using System.Web.Http;
using System.Web;
using System.IO;
using Syncfusion.Presentation;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.PresentationToPdfConverter;
using Syncfusion.Pdf;
using System.Drawing;
using System.Net;
using System.Net.Http;
using Syncfusion.Pdf.Graphics;

namespace WebServices.Controllers
{
    public class PowerPointController : ApiController
    {
        #region PPTX To PDF
        [AcceptVerbs("Post")]
        /// <summary>
        /// Converts the PowerPoint presentation (PPTX) to PDF document.
        /// </summary>
        public HttpResponseMessage ConvertToPDF()
        {
            using (Stream stream = Request.Content.ReadAsStreamAsync().Result)
            {
                // Creates new MemoryStream instance for output PDF.
                MemoryStream pdfStream = new MemoryStream();
                //Opens the PowerPoint presentation (PPTX) from stream
                using (IPresentation presentation = Presentation.Open(stream))
                {
                    //Initializes the ChartToImageConverter for converting charts during PPTX to PDF conversion
                    presentation.ChartToImageConverter = new ChartToImageConverter();
                    presentation.ChartToImageConverter.ScalingMode = Syncfusion.OfficeChart.ScalingMode.Best;

                    //Creates an instance of the PresentationToPdfConverterSettings
                    PresentationToPdfConverterSettings settings = new PresentationToPdfConverterSettings();
                    settings.ShowHiddenSlides = true;
                    //Converts PowerPoint presentation (PPTX) into PDF document
                    PdfDocument pdfDocument = PresentationToPdfConverter.Convert(presentation, settings);
                    //Adds watermark at top left corner of first page in the generated PDF document, to denote it is generated using demo web service
                    //If you want to remove this watermark, please comment or delete the codes within below "if" statement
                    if (pdfDocument.Pages.Count > 0)
                    {
                        PdfPage pdfPage = pdfDocument.Pages[0];
                        //Create PDF font and PDF font style using Font.
                        Font font = new Font("Times New Roman", 12f, FontStyle.Regular);
                        PdfFont pdfFont = new PdfTrueTypeFont(font, false);
                        //Create a new pdf brush to draw the rectangle.
                        PdfBrush pdfBrush = new PdfSolidBrush(Color.White);
                        //Draw rectangle in the current page.
                        pdfPage.Graphics.DrawRectangle(pdfBrush, 0f, 0f, 228f, 20.65f);
                        //Create a new brush to draw the text.
                        pdfBrush = new PdfSolidBrush(Color.Red);
                        //Draw text in the current page.
                        pdfPage.Graphics.DrawString("Created by Syncfusion – Presentation library", pdfFont, pdfBrush, 6f, 4f);
                    }
                    // Saves the PDF document to stream.
                    pdfDocument.Save(pdfStream);
                    pdfStream.Position = 0;
                    pdfDocument.Close(true);
                    // Creates HttpResponseMessage to return result with output PDF stream.
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new StreamContent(pdfStream);
                    result.Content.Headers.ContentLength = pdfStream.Length;
                    return result;
                }
            }
        }
        #endregion
    }
}
