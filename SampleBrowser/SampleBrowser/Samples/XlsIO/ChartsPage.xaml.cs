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
    public partial class ChartsPage : SamplePage
    {
        #region Constructor
        public ChartsPage()
        {
            InitializeComponent();

            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Description.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;
                this.btnTemplate.HorizontalOptions = LayoutOptions.Start;
                this.ButtonGrid.HorizontalOptions = LayoutOptions.Start;

                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;            
                this.btnGenerate.BackgroundColor = Color.Gray;
                this.btnTemplate.BackgroundColor = Color.Gray;

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
    /// Provides the implementation for ChartsPageCommand class.
    /// </summary>
    public class ChartPageCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPageCommand"/> class.
        /// </summary>
        public ChartPageCommand()
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
            ChartCreation();
        }
        /// <summary>
        /// Creates the Chart in the Excel document
        /// </summary>
        private void ChartCreation()
        {
            string resourcePath = "SampleBrowser.Samples.XlsIO.Template.ChartData.xlsx";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(resourcePath);

            MemoryStream stream = new MemoryStream();
            //Creates a new instance for ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Instantiate the Excel application object
                IApplication application = excelEngine.Excel;

                //Assigns default application version as Excel 2013
                application.DefaultVersion = ExcelVersion.Excel2013;

                //Open an existing workbook 
                IWorkbook workbook = application.Workbooks.Open(fileStream);

                //Access the first worksheet
                IWorksheet sheet = workbook.Worksheets[0];

                //Create a Chart
                IChartShape chart = sheet.Charts.Add();

                //Set Chart Type as Column Clustered
                chart.ChartType = ExcelChartType.Column_Clustered;

                //Set data range of the chart
                chart.DataRange = sheet["A4:E14"];

                //Set the Chart Title
                chart.ChartTitle = sheet["A3"].Text;

                chart.HasLegend = false;

                //Positioning chart in a worksheet
                chart.TopRow = 15;
                chart.LeftColumn = 1;
                chart.RightColumn = 6;
                chart.BottomRow = 27;
                                
                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;

                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("Charts.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("Charts.xlsx", "application/msexcel", stream);
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for ChartsPage View Model
    /// </summary>
    public class ChartsViewModel : BindableObject, INotifyPropertyChanged
    {
        private OpenTemplateFileCommand openTemplate;     
           
        public event PropertyChangedEventHandler PropertyChanged;        

        public static readonly BindableProperty ChartCommandProperty = BindableProperty.Create<ChartsViewModel, ChartPageCommand>(s => s.ChartCommand, new ChartPageCommand(), BindingMode.OneWay, null, null);
        public ChartPageCommand ChartCommand
        {
            get
            {
                return (ChartPageCommand)GetValue(ChartCommandProperty);
            }
            set
            {
                SetValue(ChartCommandProperty, value);
            }
        }
        public OpenTemplateFileCommand OpenTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenTemplateFileCommand("ChartData");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
