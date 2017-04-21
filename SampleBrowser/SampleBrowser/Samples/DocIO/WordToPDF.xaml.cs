using System;
using System.Net.Http;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace SampleBrowser
{
    #region Sample Page
    /// <summary>
    /// Provides the implementation for WordToPDF class.
    /// </summary>
    public partial class WordToPDF : SamplePage
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WordToPDF"/> class.
        /// </summary>
        public WordToPDF()
        {
            // Assign page instance to Command.
            WordToPDFCommand.wordToPDF = this;
            WordFilePickerCommand.wordToPDF = this;
            InitializeComponent();
            // Sets PDFView model navigation to view the PDF file using PDFViewer.
            PDFViewModel.Navigation = ContentView.Navigation;
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                SampleTitle.HorizontalOptions = LayoutOptions.Start;
                Content_1.HorizontalOptions = LayoutOptions.Start;
                Error.HorizontalOptions = LayoutOptions.Start;
                btnGenerate.HorizontalOptions = LayoutOptions.Start;
                SampleTitle.VerticalOptions = LayoutOptions.Center;
                Content_1.VerticalOptions = LayoutOptions.Center;
                Error.VerticalOptions = LayoutOptions.Center;
                btnGenerate.VerticalOptions = LayoutOptions.Center;
                btnGenerate.BackgroundColor = Color.Gray;
                chooseButton.BackgroundColor = Color.Gray;
            }
            else if (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                if (!App.isUWP)
                {
                    Content_1.FontSize = 18.5;
                    ViewerLabel.FontSize = 18.5;
                    Error.FontSize = 18.5;
                }
                else
                {
                    Content_1.FontSize = 13.5;
                    ViewerLabel.FontSize = 13.5;
                    Error.FontSize = 13.5;
                }
                SampleTitle.VerticalOptions = LayoutOptions.Center;
                Content_1.VerticalOptions = LayoutOptions.Center;
                Error.VerticalOptions = LayoutOptions.Center;
                btnGenerate.VerticalOptions = LayoutOptions.Center;
            }
            WordToPDFCommand.IsToggled = true;
            switch1.Toggled += Switch1_Toggled;
            Error.Text = string.Empty;
            Error.TextColor = Color.Red;
            this.fileName.Text = "DoctoPDF.docx";
        }
        #endregion

        #region Implementation Methods
        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            WordToPDFCommand.IsToggled = e.Value;
        }
        public void SetErrorText(string error)
        {
            Error.Text = error;
        }
        public void SetFileName(string fileName)
        {
            this.fileName.Text = fileName;
        }
        #endregion
    }
    #endregion

    #region Command Implementation
    /// <summary>
    /// Provides the implementation for WordToPDFCommand class.
    /// </summary>
    public class WordToPDFCommand : CommandBase
    {
        #region Fields
        public static bool IsToggled = true;
        public static WordToPDF wordToPDF;
        public static string fileName;
        public static byte[] bytes;
        #endregion

        #region Constructor
        public WordToPDFCommand()
        {
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        protected override void ExecuteCommand(object parameter)
        {
            WordtoPDF();
        }
        /// <summary>
        /// Converts Word document to PDF using Web API service.
        /// </summary>
        private async void WordtoPDF()
        {
            #region Word to PDF
            wordToPDF.SetErrorText(string.Empty);
            // Gets assembly.
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            // Gets input Word document as stream.
            Stream inputStream = null;
            if (fileName == null || bytes == null)
            {
                inputStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.DoctoPDF.docx");
            }
            else
                inputStream = new MemoryStream(bytes);
            // Creates new instance of HttpClient to access service.
            HttpClient client = new HttpClient();
            // Gets Uri 
            string requestUri = "http://js.syncfusion.com/demos/ioservices/api/word/converttopdf";
            // Posts input Word document to service and gets resultant PDF as content of HttpResponseMessage
            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsync(requestUri, new StreamContent(inputStream));
                //Dispose the input stream and client instances.
                inputStream.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {
                wordToPDF.SetErrorText(ex.Message.ToString());
                return;
            }
            MemoryStream outputStream = null;
            // Gets PDF from content stream if service got success.
            if (response.IsSuccessStatusCode)
            {
                var responseHeaders = response.Headers;
                outputStream = new MemoryStream(await response.Content.ReadAsByteArrayAsync());
                // Dispose the response instance.
                response.Dispose();
            }
            else
            {
                // Pop ups if service fails.
                wordToPDF.SetErrorText("The input document could not be processed, Could you please email the document to support@syncfusion.com for troubleshooting?");
                return;
            }
            #endregion

            #region Launch PDF file
            if (IsToggled)
            {
                //Open in Essential PDF viewer.
                PdfViewerUI pdfViewer = new SampleBrowser.PdfViewerUI();
                pdfViewer.PdfDocumentStream = outputStream;
                if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
                    await PDFViewModel.Navigation.PushModalAsync(new NavigationPage(pdfViewer));
                else
                    await PDFViewModel.Navigation.PushAsync(pdfViewer);
            }
            else
            {
                //Open in default system viewer.
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    await DependencyService.Get<ISaveWindowsPhone>().Save("WordToPDF.pdf", "application/pdf", outputStream);
                else
                    DependencyService.Get<ISave>().Save("WordToPDF.pdf", "application/pdf", outputStream);
                // Dispose the output stream instance.
                outputStream.Dispose();
            }
            wordToPDF.SetErrorText(string.Empty);
            #endregion
        }
        #endregion
    }
    public class WordFilePickerCommand : CommandBase
    {
        #region Fields
        public static WordToPDF wordToPDF;
        #endregion
        #region Constructor
        public WordFilePickerCommand()
        {
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        protected override void ExecuteCommand(object parameter)
        {
            PickWordFile();
        }
        private async void PickWordFile()
        {
            try
            {
                InputFileData filedata = await Xamarin.Forms.DependencyService.Get<IFilePicker>().PickFile("word");
                WordToPDFCommand.fileName = filedata.FileName;
                WordToPDFCommand.bytes = filedata.DataArray;
                wordToPDF.SetFileName(filedata.FileName);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        #endregion
    }
    #endregion

}
