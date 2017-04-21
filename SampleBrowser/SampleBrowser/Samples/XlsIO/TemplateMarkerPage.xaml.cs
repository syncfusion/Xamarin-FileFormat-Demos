using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class TemplateMarkerPage : SamplePage
    {
        #region Constructor
        public TemplateMarkerPage()
        {
            InitializeComponent();
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Description.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;
                this.btnTemplate.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.BackgroundColor = Color.Gray;
                this.btnTemplate.BackgroundColor = Color.Gray;
                this.ButtonGrid.HorizontalOptions = LayoutOptions.Start;
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
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
            }
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for TemplateMarkerCommand class.
    /// </summary>
    public class TemplateMarkerCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMarkerCommand"/> class.
        /// </summary>
        public TemplateMarkerCommand()
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
            ApplyTemplateMarkers();
        }

        /// <summary>
        /// Fill the data in the Template Excel file
        /// </summary>
        private void ApplyTemplateMarkers()
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;

            //Access the template Excel document as stream
            Stream fileStream = null;
            fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.TemplateFile.xlsx");
            
            //Parsing XML file and converts into Expando Objects            
            Stream xmlFileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.BusinessObjects.xml");
            StreamReader reader = new StreamReader(xmlFileStream);
            IEnumerable<BusinessObject> _customers = GetData<BusinessObject>(reader.ReadToEnd()).ToList();

            MemoryStream stream = new MemoryStream();

            //Creates a new instance for ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Instantiate the Excel application object
                IApplication application = excelEngine.Excel;

                //Assigns default application version as Excel 2013
                application.DefaultVersion = ExcelVersion.Excel2013;

                //Open an existing workbook                
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(fileStream);

                //Access the first worksheet
                IWorksheet worksheet1 = workbook.Worksheets[0];
              
                //Create Template Marker Processor
                ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();

                //Adding the variables        
                marker.AddVariable("Customers", _customers);

                //Process the markers in the template.
                marker.ApplyMarkers();

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;

                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("TemplateMarker.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("TemplateMarker.xlsx", "application/msexcel", stream);
        }
        static IEnumerable<T> GetData<T>(string xml)
        where T : BusinessObject, new()
        {
            return XElement.Parse(xml)
               .Elements("Customers")
               .Select(c => new T
               {
                   SalesPerson = (string)c.Element("SalesPerson"),
                   SalesJanJune = (int)c.Element("SalesJanJune"),
                   SalesJulyDec = (int)c.Element("SalesJulyDec"),
                   Change = (int)c.Element("Change"),
               });

        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for TemplateMarkerPage View Model
    /// </summary>
    public class TemplateMarkerViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Fields and Properties 
        private OpenTemplateFileCommand openTemplate;
        public OpenTemplateFileCommand OpenTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenTemplateFileCommand("TemplateFile");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }
        public TemplateMarkerCommand TemplateMarkerCommand
        {
            get
            {
                return (TemplateMarkerCommand)GetValue(TemplateMarkerCommandProperty);
            }
            set
            {
                SetValue(TemplateMarkerCommandProperty, value);
            }
        }
        public static readonly BindableProperty TemplateMarkerCommandProperty = BindableProperty.Create<TemplateMarkerViewModel, TemplateMarkerCommand>(s => s.TemplateMarkerCommand, new TemplateMarkerCommand(), BindingMode.OneWay, null, null);
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
