using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class ImportXMLPage : SamplePage
    {
        #region Constructor
        public ImportXMLPage()
        {
            InitializeComponent();

            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Content_1.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;              
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
				this.btnGenerate.BackgroundColor = Color.Gray;
            }
            else if (Device.Idiom == TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                if (!SampleBrowser.App.isUWP)
                {
                    this.Content_1.FontSize = 18.5;
                }
                else
                {
                    this.Content_1.FontSize = 13.5;
                }             
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
            }
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for ImportXMLCommand class.
    /// </summary>
    public class ImportXMLCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportXMLCommand"/> class.
        /// </summary>
        public ImportXMLCommand()
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
            ImportXML();
        }

        /// <summary>
        /// Import XML data in a new Excel document
        /// </summary>
        private void ImportXML()
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.customers.xml");

            MemoryStream stream = new MemoryStream();
            //Creates a new instance for ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Instantiate the Excel application object
                IApplication application = excelEngine.Excel;

                //Assigns default application version as Excel 2013
                application.DefaultVersion = ExcelVersion.Excel2013;

                //Create a new workbook 
                IWorkbook workbook = application.Workbooks.Create(1);

                //Access the first worksheet
                IWorksheet sheet = workbook.Worksheets[0];

                //Import the XML contents to worksheet
                XlsIOExtensions exten = new XlsIOExtensions();
                exten.ImportXML(fileStream, sheet, 1, 1, true);

                //Apply style for header
                IStyle headerStyle = sheet[1, 1, 1, sheet.UsedRange.LastColumn].CellStyle;
                headerStyle.Font.Bold = true;
                headerStyle.Font.Color = ExcelKnownColors.Brown;
                headerStyle.Font.Size = 10;

                #region Resize columns
                sheet.Columns[0].ColumnWidth = 11;
                sheet.Columns[1].ColumnWidth = 30.5;
                sheet.Columns[2].ColumnWidth = 20;
                sheet.Columns[3].ColumnWidth = 25.6;
                sheet.Columns[6].ColumnWidth = 10.5;
                sheet.Columns[4].ColumnWidth = 40;
                sheet.Columns[5].ColumnWidth = 25.5;
                sheet.Columns[7].ColumnWidth = 9.6;
                sheet.Columns[8].ColumnWidth = 15;
                sheet.Columns[9].ColumnWidth = 15;
                #endregion

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;
                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("ImportXML.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("ImportXML.xlsx", "application/msexcel", stream);
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for ImportXMLPage View Model
    /// </summary>
    public class ImportXMLViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Properties
        public ImportXMLCommand ImportXMLCommand
        {
            get
            {
                return (ImportXMLCommand)GetValue(ImportXMLCommandProperty);
            }
            set
            {
                SetValue(ImportXMLCommandProperty, value);
            }
        }

        public static readonly BindableProperty ImportXMLCommandProperty = BindableProperty.Create<ImportXMLViewModel, ImportXMLCommand>(s => s.ImportXMLCommand, new ImportXMLCommand(), BindingMode.OneWay, null, null);
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
}
