using System;
using System.Net.Http;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using System.ComponentModel;
namespace SampleBrowser
{
    #region Sample Page
    /// <summary>
    /// Provides the implementation for ExceltoPDFPage class.
    /// </summary>
    public partial class ExceltoPDFPage : SamplePage
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceltoPDFPage"/> class.
        /// </summary>
        public ExceltoPDFPage()
        {
            // Assign page instance to Command.
            ExceltoPDFCommand.excelToPDF = this;
            ExcelFilePickerCommand.exceltoPDFPage = this;
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
                this.chooseButton.BackgroundColor = Color.Gray;
            }
            else if (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                if (!App.isUWP)
                {
                    Content_1.FontSize = 18.5;
                    Error.FontSize = 18.5;
                    ViewerLabel.FontSize = 18.5;
                }
                else
                {
                    Content_1.FontSize = 13.5;
                    Error.FontSize = 13.5;
                    ViewerLabel.FontSize = 13.5;
                }
                SampleTitle.VerticalOptions = LayoutOptions.Center;
                Content_1.VerticalOptions = LayoutOptions.Center;
                Error.VerticalOptions = LayoutOptions.Center;
                btnGenerate.VerticalOptions = LayoutOptions.Center;
            }
            ExceltoPDFCommand.IsToggled = true;
            switch1.Toggled += Switch1_Toggled;
            Error.Text = string.Empty;
            Error.TextColor = Color.Red;
            this.fileName.Text = "ExceltoPDF.xlsx";
        }
        #endregion
        #region Implementation Methods
        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            ExceltoPDFCommand.IsToggled = e.Value;
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
    /// Provides the implementation for ExceltoPDFCommand class.
    /// </summary>
    public class ExceltoPDFCommand : CommandBase
    {
        #region Fields
        public static bool IsToggled = true;
        public static ExceltoPDFPage excelToPDF;
        public static string fileName;
        public static byte[] bytes;
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceltoPDFCommand"/> class.
        /// </summary>
        public ExceltoPDFCommand()
        {
        }
        #endregion

        #region Implementation Methods
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        protected override void ExecuteCommand(object parameter)
        {
            ExceltoPDF();
        }
        /// <summary>
        /// Creates the new Excel document
        /// </summary>
        private async void ExceltoPDF()
        {
            #region Excel to PDF
            excelToPDF.SetErrorText(string.Empty);
            // Gets assembly.
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            // Gets input Word document from embedded resource collection.
            Stream inputStream = null;
            if (fileName == null || bytes == null)
            {
                inputStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.ExceltoPDF.xlsx");
            }
            else
                inputStream = new MemoryStream(bytes);
            // Creates new instance of HttpClient to access service.
            HttpClient client = new HttpClient();
            // Gets Uri 
            string requestUri = "http://js.syncfusion.com/demos/ioservices/api/excel/converttopdf";
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
                excelToPDF.SetErrorText(ex.Message.ToString());
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
                excelToPDF.SetErrorText("The input document could not be processed, Could you please email the document to support@syncfusion.com for troubleshooting?");
                return;
            }
            #endregion

            #region Launch PDF file
            if (IsToggled)
            {
                if (PDFViewModel.Navigation.ModalStack.Count == 0)
                {
                    //Open in Essential PDF viewer.
                    PdfViewerUI pdfViewer = new SampleBrowser.PdfViewerUI();
                    pdfViewer.PdfDocumentStream = outputStream;
                    if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
                        await PDFViewModel.Navigation.PushModalAsync(new NavigationPage(pdfViewer));
                    else
                        await PDFViewModel.Navigation.PushAsync(pdfViewer);
                }
            }
            else
            {
                //Open in default system viewer.
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    await DependencyService.Get<ISaveWindowsPhone>().Save("ExcelToPDF.pdf", "application/pdf", outputStream);
                else
                    DependencyService.Get<ISave>().Save("ExcelToPDF.pdf", "application/pdf", outputStream);
                // Dispose the output stream instance.
                outputStream.Dispose();
            }
            excelToPDF.SetErrorText(string.Empty);
            #endregion
        }
        #endregion
    }
    public class ExcelFilePickerCommand : CommandBase
    {
        #region Fields
        public static ExceltoPDFPage exceltoPDFPage;
        #endregion
        #region Constructor
        public ExcelFilePickerCommand()
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
            PickExcelFile();
        }
        private async void PickExcelFile()
        {
            try
            {
                InputFileData filedata = await Xamarin.Forms.DependencyService.Get<IFilePicker>().PickFile("excel");
                ExceltoPDFCommand.fileName = filedata.FileName;
                ExceltoPDFCommand.bytes = filedata.DataArray;
                exceltoPDFPage.SetFileName(filedata.FileName);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
        #endregion
    }
    #endregion

    #region View Model
    /// <summary>
    /// Provides the implementation for ExceltoPDFPage View Model
    /// </summary>
    public class ExceltoPDFViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Properties
        public ExceltoPDFCommand ExceltoPDFCommand
        {
            get
            {
                return (ExceltoPDFCommand)GetValue(ExceltoPDFCommandProperty);
            }
            set
            {
                SetValue(ExceltoPDFCommandProperty, value);
            }
        }
        public ExcelFilePickerCommand ExcelFilePickerCommand
        {
            get
            {
                return (ExcelFilePickerCommand)GetValue(ExcelFilePickerCommandProperty);
            }
            set
            {
                SetValue(ExcelFilePickerCommandProperty, value);
            }
        }
        public static readonly BindableProperty ExceltoPDFCommandProperty = BindableProperty.Create<ExceltoPDFViewModel, ExceltoPDFCommand>(s => s.ExceltoPDFCommand, new ExceltoPDFCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty ExcelFilePickerCommandProperty = BindableProperty.Create<ExceltoPDFViewModel, ExcelFilePickerCommand>(s => s.ExcelFilePickerCommand, new ExcelFilePickerCommand(), BindingMode.OneWay, null, null);
        #endregion

        #region Event Methods
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
    #endregion
}
