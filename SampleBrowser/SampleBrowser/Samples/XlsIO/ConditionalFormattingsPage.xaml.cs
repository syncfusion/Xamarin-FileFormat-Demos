using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using COLOR = Syncfusion.Drawing.Color;

using Xamarin.Forms;
using Syncfusion.XlsIO;
using System.ComponentModel;

namespace SampleBrowser
{
    public partial class ConditionalFormattingsPage : SamplePage
    {
        #region Constructor
        public ConditionalFormattingsPage()
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
    /// Provides the implementation for CFPageCommand class.
    /// </summary>
    public class CFPageCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CFPageCommand"/> class.
        /// </summary>
        public CFPageCommand()
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
            ApplyCondtionalFormatting();
        }
        /// <summary>
        /// Apply the conditional formattings in the Excel document
        /// </summary>
        private void ApplyCondtionalFormatting()
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = null;
            fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.CFTemplate.xlsx");

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
                IWorksheet worksheet = workbook.Worksheets[0];

                #region Databar
                //Create data bars for the data in specified range
                IConditionalFormats conditionalFormats = worksheet.Range["C7:C46"].ConditionalFormats;
                IConditionalFormat conditionalFormat = conditionalFormats.AddCondition();
                conditionalFormat.FormatType = ExcelCFType.DataBar;
                IDataBar dataBar = conditionalFormat.DataBar;

                //Set the constraints
                dataBar.MinPoint.Type = ConditionValueType.LowestValue;
                dataBar.MinPoint.Value = "0";
                dataBar.MaxPoint.Type = ConditionValueType.HighestValue;
                dataBar.MaxPoint.Value = "0";

                //Set color for data bar
                dataBar.BarColor = COLOR.FromArgb(156, 208, 243);

                //Hide the value in data bar
                dataBar.ShowValue = false;
                #endregion

                #region Iconset
                //Create icon sets for the data in specified range
                conditionalFormat = conditionalFormats.AddCondition();                
                conditionalFormat.FormatType = ExcelCFType.IconSet;
                IIconSet iconSet = conditionalFormat.IconSet;

                //Apply four ratings icon and hide the data in the specified range
                iconSet.IconSet = ExcelIconSetType.FourRating;                
                iconSet.IconCriteria[0].Type = ConditionValueType.LowestValue;
                iconSet.IconCriteria[0].Value = "0";
                iconSet.IconCriteria[1].Type = ConditionValueType.HighestValue;
                iconSet.IconCriteria[1].Value = "0";
                iconSet.ShowIconOnly = true;

                //Set icon set conditional format in specified range
                conditionalFormats = worksheet.Range["E7:E46"].ConditionalFormats;
                conditionalFormat = conditionalFormats.AddCondition();
                conditionalFormat.FormatType = ExcelCFType.IconSet;
                iconSet = conditionalFormat.IconSet;

                //Apply three symbols icon and hide the data in the specified range
                iconSet.IconSet = ExcelIconSetType.ThreeSymbols;
                iconSet.IconCriteria[0].Type = ConditionValueType.LowestValue;
                iconSet.IconCriteria[0].Value = "0";
                iconSet.IconCriteria[1].Type = ConditionValueType.HighestValue;
                iconSet.IconCriteria[1].Value = "0";
                iconSet.ShowIconOnly = true;
                #endregion

                #region Databar Negative value settings
                //Create data bars for the data in specified range
                IConditionalFormats conditionalFormats1 = worksheet.Range["E7:E46"].ConditionalFormats;
                IConditionalFormat conditionalFormat1 = conditionalFormats1.AddCondition();              
                conditionalFormat1.FormatType = ExcelCFType.DataBar;
                IDataBar dataBar1 = conditionalFormat1.DataBar;

                //Set the constraints
                dataBar1.BarColor = COLOR.YellowGreen;
                dataBar1.NegativeFillColor = COLOR.Pink;
                dataBar1.NegativeBorderColor = COLOR.WhiteSmoke;
                dataBar1.BarAxisColor = COLOR.Yellow;
                dataBar1.BorderColor = COLOR.WhiteSmoke;
                dataBar1.DataBarDirection = DataBarDirection.context;
                dataBar1.DataBarAxisPosition = DataBarAxisPosition.middle;
                dataBar1.HasGradientFill = true;

                //Hide the value in data bar
                dataBar1.ShowValue = false;
                #endregion

                #region Color Scale
                //Create color scales for the data in specified range
                conditionalFormats = worksheet.Range["D7:D46"].ConditionalFormats;
                conditionalFormat = conditionalFormats.AddCondition();
                conditionalFormat.FormatType = ExcelCFType.ColorScale;
                IColorScale colorScale = conditionalFormat.ColorScale;

                //Sets 3 - color scale.
                colorScale.SetConditionCount(3);
                colorScale.Criteria[0].FormatColorRGB = COLOR.FromArgb(230, 197, 218);
                colorScale.Criteria[0].Type = ConditionValueType.LowestValue;
                colorScale.Criteria[0].Value = "0";
                colorScale.Criteria[1].FormatColorRGB = COLOR.FromArgb(244, 210, 178);
                colorScale.Criteria[1].Type = ConditionValueType.Percentile;
                colorScale.Criteria[1].Value = "50";
                colorScale.Criteria[2].FormatColorRGB = COLOR.FromArgb(245, 247, 171);
                colorScale.Criteria[2].Type = ConditionValueType.HighestValue;
                colorScale.Criteria[2].Value = "0";
                #endregion

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;
                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }           
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("AdvancedCF.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("AdvancedCF.xlsx", "application/msexcel", stream);
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for ConditionalFormattingsPage View Model
    /// </summary>
    public class CondtionalFormattingsViewModel : BindableObject, INotifyPropertyChanged
    {
        private OpenTemplateFileCommand openTemplate;

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly BindableProperty CFCommandProperty = BindableProperty.Create<CondtionalFormattingsViewModel, CFPageCommand>(s => s.CFCommand, new CFPageCommand(), BindingMode.OneWay, null, null);
        public OpenTemplateFileCommand OpenTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenTemplateFileCommand("CFTemplate");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }               
        public CFPageCommand CFCommand
        {
            get
            {
                return (CFPageCommand)GetValue(CFCommandProperty);
            }
            set
            {
                SetValue(CFCommandProperty, value);
            }
        }
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
