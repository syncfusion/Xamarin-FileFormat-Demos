using System.Web;
using System.Web.Http;
using System.IO;
using Syncfusion.XlsIO;
using Syncfusion.Pdf;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.ExcelChartToImageConverter;
using System.Drawing;
using System.Net;
using System.Net.Http;
using Syncfusion.Pdf.Graphics;

namespace WebServices.Controllers
{
    public class ExcelController : ApiController
    {
        #region Excel To PDF
        [AcceptVerbs("Post")]
        /// <summary>
        /// Converts the Excel document to PDF document.
        /// </summary>
        public HttpResponseMessage ConvertToPDF()
        {
            using (Stream stream = Request.Content.ReadAsStreamAsync().Result)
            {
                // Creates new MemoryStream instance for output PDF.
                MemoryStream pdfStream = new MemoryStream();
                //Initializes the Excel Engine
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    // Instantiates the ChartToImageConverter and assigns the ChartToImageConverter instance of XlsIO application
                    application.ChartToImageConverter = new ChartToImageConverter();
                    // Tuning Chart Image Quality.
                    application.ChartToImageConverter.ScalingMode = ScalingMode.Best;
                    //Opens the Excel document from stream
                    IWorkbook workbook = application.Workbooks.Open(stream, ExcelOpenType.Automatic);
                    //Creates an instance of the ExcelToPdfConverter
                    ExcelToPdfConverter converter = new ExcelToPdfConverter(workbook);
                    //Converts Excel document into PDF document
                    PdfDocument pdfDocument = converter.Convert();
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
                        pdfPage.Graphics.DrawRectangle(pdfBrush, 0f, 0f, 200f, 20.65f);
                        //Create a new brush to draw the text.
                        pdfBrush = new PdfSolidBrush(Color.Red);
                        //Draw text in the current page.
                        pdfPage.Graphics.DrawString("Created by Syncfusion – XlsIO library", pdfFont, pdfBrush, 6f, 4f);
                    }
                    // Saves the PDF document to stream.
                    pdfDocument.Save(pdfStream);
                    pdfStream.Position = 0;
                    converter.Dispose();
                    workbook.Close();
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