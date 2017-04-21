using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class FormulasPage : SamplePage
    {
        #region Constructor
        public FormulasPage()
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
    /// Provides the implementation for FormulaCommand class.
    /// </summary>
    public class FormulaCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaCommand"/> class.
        /// </summary>
        public FormulaCommand()
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
            CreateFormulaExcelFile();
        }

        /// <summary>
        /// Creates a new Excel workbook with the formulas
        /// </summary>
        private void CreateFormulaExcelFile()
        {
            MemoryStream stream = new MemoryStream();
            //Creates a new instance for ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Instantiate the Excel application object
                IApplication application = excelEngine.Excel;

                //Assigns default application version as Excel 2013
                application.DefaultVersion = ExcelVersion.Excel2013;

                //Creates a new Excel workbook
                IWorkbook workbook = application.Workbooks.Create(1);

                //Access the first worksheet
                IWorksheet sheet = workbook.Worksheets[0];

                //Initializes Calculate Engine to perform calculation
                sheet.EnableSheetCalculations();

                #region Set values and formattings in the cells
                sheet.Range["A2"].Text = "Array formulas";
                sheet.Range["B2:E2"].Number = 3;
                sheet.Names.Add("ArrayRange", sheet.Range["B2:E2"]);
                sheet.Range["B3:E3"].Number = 5;
                sheet.Range["A2"].CellStyle.Font.Bold = true;
                sheet.Range["A2"].CellStyle.Font.Size = 14;

                sheet.Range["A5"].Text = "Formulas";
                sheet.Range["B5"].Text = "Results";
                sheet.Range["B5:C5"].Merge();
                sheet.Range["B5:C5"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                sheet.Range["A5:B5"].CellStyle.Font.Bold = true;
                sheet.Range["A5:B5"].CellStyle.Font.Size = 14;

                sheet.Range["A1"].Text = "Excel formula support";
                sheet.Range["A1"].CellStyle.Font.Bold = true;
                sheet.Range["A1"].CellStyle.Font.Size = 14;
                sheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                sheet.Range["A1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                sheet.Range["A1:C1"].Merge();
                #endregion

                sheet.Range["A7"].Text = "ABS(ABS(-B3))";
                //Set ABS formula to the cell B7
                sheet.Range["B7"].Formula = "ABS(ABS(-B3))";

                sheet.Range["A9"].Text = "SUM(B3,C3)";
                //Set SUM formula to the cell B9
                sheet.Range["B9"].Formula = "SUM(B3,C3)";

                sheet.Range["A11"].Text = "MIN(10,20,30,5,15,35,6,16,36)";
                //Set MIN function formula to the cell B11
                sheet.Range["B11"].Formula = "MIN(10,20,30,5,15,35,6,16,36)";

                sheet.Range["A13"].Text = "MAX(10,20,30,5,15,35,6,16,36)";
                //Set MAX function formula to the cell B13
                sheet.Range["B13"].Formula = "MAX(10,20,30,5,15,35,6,16,36)";
               
                sheet.Range["C7"].Number = 10;
                sheet.Range["C9"].Number = 10;
                sheet.Range["A15"].Text = "C7+C9";
                //Set addition formula to the cell C15
                sheet.Range["C15"].Formula = "C7+C9";
                
                //Modifying the column width
                sheet.Columns[0].ColumnWidth = 23;
                sheet.Columns[1].ColumnWidth = 10;
                sheet.Columns[2].ColumnWidth = 10;
                sheet.Columns[3].ColumnWidth = 10;
                sheet.Columns[4].ColumnWidth = 10;

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;

                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("Formulas.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("Formulas.xlsx", "application/msexcel", stream);
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for FormulasPage View Model
    /// </summary>
    public class FormulasViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Properties
        public FormulaCommand FormulaCommand
        {
            get
            {
                return (FormulaCommand)GetValue(FormulaCommandProperty);
            }
            set
            {
                SetValue(FormulaCommandProperty, value);
            }
        }

        public static readonly BindableProperty FormulaCommandProperty = BindableProperty.Create<FormulasViewModel, FormulaCommand>(s => s.FormulaCommand, new FormulaCommand(), BindingMode.OneWay, null, null);
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
