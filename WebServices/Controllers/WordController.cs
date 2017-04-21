using System.IO;
using System.Web.Http;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Pdf;
using System.Net.Http;
using System.Net;

namespace WebServices.Controllers
{
    public class WordController : ApiController
    {
        #region Word To PDF
        /// <summary>
        /// Converts the Word document to PDF document.
        /// </summary>
        [HttpPost]
        [Route("api/word/converttopdf")]
        public HttpResponseMessage ConvertToPdf()
        {
            // Gets input Word document stream from result.
            using (Stream wordStream = Request.Content.ReadAsStreamAsync().Result)
            {
                // Creates new MemoryStream instance for output PDF.
                MemoryStream pdfStream = new MemoryStream();
                using (WordDocument wordDocument = new WordDocument(wordStream))
                {
                    // Initializes the ChartToImageConverter for converting charts during Word to PDF conversion.
                    wordDocument.ChartToImageConverter = new ChartToImageConverter();
                    wordDocument.ChartToImageConverter.ScalingMode = Syncfusion.OfficeChart.ScalingMode.Normal;
                    // Creates an instance of the DocToPDFConverter.
                    using (DocToPDFConverter docToPdfConverter = new DocToPDFConverter())
                    {
                        // Converts Word document into PDF document.
                        using (PdfDocument pdfDocument = docToPdfConverter.ConvertToPDF(wordDocument))
                        {
                            // Saves the PDF document to stream.
                            pdfDocument.Save(pdfStream);
                            pdfStream.Position = 0;
                            pdfDocument.Close(true);
                        }
                    }
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