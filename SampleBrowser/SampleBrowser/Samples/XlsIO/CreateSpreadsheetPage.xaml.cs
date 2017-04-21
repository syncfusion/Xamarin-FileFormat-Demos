using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COLOR = Syncfusion.Drawing;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace SampleBrowser
{
    public partial class CreateSpreadsheetPage : SamplePage
    {
        #region Constructor
        public CreateSpreadsheetPage()
        {
            InitializeComponent();

            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Description.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;               
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.BackgroundColor = Color.Gray;
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
            }
        }
        #endregion

    }
    /// <summary>
    /// Provides the implementation for CreateSpreadSheetCommand class.
    /// </summary>
    public class CreateSpreadSheetCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSpreadSheetCommand"/> class.
        /// </summary>
        public CreateSpreadSheetCommand()
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
            CreateSpreadSheet();
        }
        /// <summary>
        /// Creates the new Excel document
        /// </summary>
        private void CreateSpreadSheet()
        {
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

                //Initializes Calculate Engine to perform calculation
                sheet.EnableSheetCalculations();

                //Modifying the column width
                sheet.Range["A2"].ColumnWidth = 20;
                sheet.Range["B2"].ColumnWidth = 13;
                sheet.Range["C2"].ColumnWidth = 13;
                sheet.Range["D2"].ColumnWidth = 13;

                //Modifying the row height
                sheet.Range["A2"].RowHeight = 34;

                //Merging the cells
                sheet.Range["A2:D2"].Merge(true);
                sheet.Range["B7:D7"].Merge(true);
                sheet.Range["B5:D5"].Merge(true);
                sheet.Range["B4:D4"].Merge(true);
                sheet.Range["B6:D6"].Merge(true);

                //Formatting the cells
                ApplyCellStyles(sheet);                

                //Applying date time number format
                sheet.Range["B6"].NumberFormat = "m/d/yyyy";
                
                //Applying currency number format to the cells
                sheet.Range["B7"].NumberFormat = "$#,##0.00";
                sheet.Range["B11:D20"].NumberFormat = "$#,##0.00";
                
                //Setting text to the cells     
                SetTextInSheet(sheet);

                //Setting number values to the cells
                SetNumbersInSheet(sheet);

                //Setting datetime in the cell
                sheet.Range["B6"].DateTime = DateTime.Parse("10/10/2012");

                //Setting formula in the cells
                sheet.Range["B11"].Formula = "=(B7*B10)";
                sheet.Range["B20"].Formula = "=SUM(B11:B19)";
                sheet.Range["C11"].Formula = "=(B7*C10)";
                sheet.Range["C20"].Formula = "=SUM(C11:C19)";
                sheet.Range["D11"].Formula = "=(B7*D10)";
                sheet.Range["D20"].Formula = "=SUM(D11:D19)";          

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;
                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("CreateSheet.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("CreateSheet.xlsx", "application/msexcel", stream);
        }

        /// <summary>
        /// Apply cell styles in the range
        /// </summary>
        /// <param name="sheet">IWorksheet object</param>
        private void ApplyCellStyles(IWorksheet sheet)
        {
            //Setting font styles
            sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A2"].CellStyle.Font.Bold = true;
            sheet.Range["A2"].CellStyle.Font.Size = 28;
            sheet.Range["A2"].CellStyle.Font.RGBColor = COLOR.Color.FromArgb(255, 0, 112, 192);

            sheet.Range["A4:B7"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A4:B7"].CellStyle.Font.Bold = true;
            sheet.Range["A4:B7"].CellStyle.Font.Size = 11;
            sheet.Range["A4:A7"].CellStyle.Font.RGBColor = COLOR.Color.FromArgb(255, 128, 128, 128);
            sheet.Range["B4:B7"].CellStyle.Font.RGBColor = COLOR.Color.FromArgb(255, 174, 170, 170);

            sheet.Range["A9:D20"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A9:D20"].CellStyle.Font.Size = 11;

            sheet.Range["A20:D20"].CellStyle.Font.Color = ExcelKnownColors.Black;
            sheet.Range["A20:D20"].CellStyle.Font.Bold = true;

            sheet.Range["A9"].CellStyle.Font.Color = ExcelKnownColors.White;
            sheet.Range["A9"].CellStyle.Font.Bold = true;

            sheet.Range["A10:D10"].CellStyle.Font.RGBColor = COLOR.Color.FromArgb(255, 174, 170, 170);

            ////Text Alignment Setting
            sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
            sheet.Range["A4:A7"].HorizontalAlignment = ExcelHAlign.HAlignLeft;
            sheet.Range["B4:B7"].HorizontalAlignment = ExcelHAlign.HAlignRight;

            //Applying cell back color
            sheet.Range["A20:D20"].CellStyle.Color = COLOR.Color.FromArgb(255, 0, 112, 192);
            sheet.Range["A9"].CellStyle.Color = COLOR.Color.FromArgb(255, 0, 112, 192);

            //Formatting the cells from B9 to D9
            IStyle style = sheet["B9:D9"].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignRight;
            style.Color = COLOR.Color.FromArgb(255, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.Black;
        }

        /// <summary>
        /// Set the number value in the range
        /// </summary>
        /// <param name="sheet">IWorksheet object</param>
        private void SetNumbersInSheet(IWorksheet sheet)
        {
            sheet.Range["B7"].Number = 0.70;
            sheet.Range["B10"].Number = 100;
            sheet.Range["B12"].Number = 0;
            sheet.Range["B13"].Number = 0;
            sheet.Range["B14"].Number = 0;
            sheet.Range["B15"].Number = 9;
            sheet.Range["B16"].Number = 12;
            sheet.Range["B17"].Number = 13;
            sheet.Range["B18"].Number = 9.5;
            sheet.Range["B19"].Number = 0;

            sheet.Range["C10"].Number = 145;
            sheet.Range["C12"].Number = 15;
            sheet.Range["C13"].Number = 0;
            sheet.Range["C14"].Number = 45;
            sheet.Range["C15"].Number = 9;
            sheet.Range["C16"].Number = 12;
            sheet.Range["C17"].Number = 15;
            sheet.Range["C18"].Number = 7;
            sheet.Range["C19"].Number = 0;

            sheet.Range["D10"].Number = 113;
            sheet.Range["D12"].Number = 17;
            sheet.Range["D13"].Number = 8;
            sheet.Range["D14"].Number = 45;
            sheet.Range["D15"].Number = 7;
            sheet.Range["D16"].Number = 11;
            sheet.Range["D17"].Number = 16;
            sheet.Range["D18"].Number = 7;
            sheet.Range["D19"].Number = 5;
        }

        /// <summary>
        /// Set the string value in the worksheet range
        /// </summary>
        /// <param name="sheet">IWorksheet object</param>
        private void SetTextInSheet(IWorksheet sheet)
        {
            sheet.Range["A2"].Text = "EXPENSE REPORT";
            sheet.Range["A4"].Text = "Employee";
            sheet.Range["B4"].Text = "Roger Federer";
            sheet.Range["A5"].Text = "Department";
            sheet.Range["B5"].Text = "Administration";
            sheet.Range["A6"].Text = "Week Ending";
            sheet.Range["A10"].Text = "Miles Driven";
            sheet.Range["A11"].Text = "Reimbursement";
            sheet.Range["A12"].Text = "Parking/Tolls";
            sheet.Range["A13"].Text = "Auto Rental";
            sheet.Range["A14"].Text = "Lodging";
            sheet.Range["A15"].Text = "Breakfast";
            sheet.Range["A16"].Text = "Lunch";
            sheet.Range["A17"].Text = "Dinner";
            sheet.Range["A18"].Text = "Snacks";
            sheet.Range["A19"].Text = "Others";
            sheet.Range["A20"].Text = "Total";
            sheet.Range["A7"].Text = "Mileage Rate";
            sheet.Range["A9"].Text = "Expenses";
            sheet.Range["B9"].Text = "Day 1";
            sheet.Range["C9"].Text = "Day 2";
            sheet.Range["D9"].Text = "Day 3";
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for CreateSpreadsheetPage View Model
    /// </summary>
    public class CreateSpreadSheetViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Properties
        public CreateSpreadSheetCommand CreateCommand
        {
            get
            {
                return (CreateSpreadSheetCommand)GetValue(CreateCommandProperty);
            }
            set
            {
                SetValue(CreateCommandProperty, value);
            }
        }
        public static readonly BindableProperty CreateCommandProperty = BindableProperty.Create<CreateSpreadSheetViewModel, CreateSpreadSheetCommand>(s => s.CreateCommand, new CreateSpreadSheetCommand(), BindingMode.OneWay, null, null);
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
