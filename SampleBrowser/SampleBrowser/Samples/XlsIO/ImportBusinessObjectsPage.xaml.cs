#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using COLOR = Syncfusion.Drawing;
using Xamarin.Forms;
using System.Xml.Linq;
using System.ComponentModel;

namespace SampleBrowser
{
    public partial class ImportBusinessObjectsPage : SamplePage
    {
        #region Constructor
        public ImportBusinessObjectsPage()
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
    /// Provides the implementation for ImportBusinessObjectCommand class.
    /// </summary>
    public class ImportBusinessObjectCommand : CommandBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportBusinessObjectCommand"/> class.
        /// </summary>
        public ImportBusinessObjectCommand()
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
            ImportBusinessObjects();
        }

        /// <summary>
        /// Import Objects in a new Excel Document
        /// </summary>
        private void ImportBusinessObjects()
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.BusinessObjects.xml");

            StreamReader reader = new StreamReader(fileStream);
            IEnumerable<BusinessObject> customers = GetData<BusinessObject>(reader.ReadToEnd());

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
                               
                // Import the Custom Object to worksheet               
                sheet.ImportData(customers, 1, 1, true);

                #region Define Styles
                IStyle pageHeader = workbook.Styles.Add("PageHeaderStyle");
                IStyle tableHeader = workbook.Styles.Add("TableHeaderStyle");

                pageHeader.Font.RGBColor = COLOR.Color.FromArgb(255, 83, 141, 213);
                pageHeader.Font.FontName = "Calibri";
                pageHeader.Font.Size = 18;
                pageHeader.Font.Bold = true;
                pageHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                pageHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;

                tableHeader.Font.Color = ExcelKnownColors.Black;
                tableHeader.Font.Bold = true;
                tableHeader.Font.Size = 11;
                tableHeader.Font.FontName = "Calibri";
                tableHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                tableHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;
                tableHeader.Color = COLOR.Color.FromArgb(255, 118, 147, 60);
                tableHeader.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                tableHeader.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                tableHeader.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                tableHeader.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                #endregion

                #region Apply Styles
                // Apply style for header
                sheet["A1:D1"].Merge();
                sheet["A1"].Text = "Yearly Sales Report";
                sheet["A1"].CellStyle = pageHeader;
                sheet["A1"].RowHeight = 20;

                sheet["A2:D2"].Merge();
                sheet["A2"].Text = "Namewise Sales Comparison Report";
                sheet["A2"].CellStyle = pageHeader;
                sheet["A2"].CellStyle.Font.Bold = false;
                sheet["A2"].CellStyle.Font.Size = 16;
                sheet["A2"].RowHeight = 20;

                sheet["A3:A4"].Merge();
                sheet["D3:D4"].Merge();
                sheet["B3:C3"].Merge();
                sheet["B3"].Text = "Sales";
                sheet["A3:D4"].CellStyle = tableHeader;

                sheet["A3"].Text = "Sales Person";
                sheet["B4"].Text = "Jan - Jun";
                sheet["C4"].Text = "Jul - Dec";
                sheet["D3"].Text = "Change (%)";

                sheet.Columns[0].ColumnWidth = 19;
                sheet.Columns[1].ColumnWidth = 10;
                sheet.Columns[2].ColumnWidth = 10;
                sheet.Columns[3].ColumnWidth = 11;
                #endregion

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;
                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("BusinessObjects.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("BusinessObjects.xlsx", "application/msexcel", stream);           
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
    /// Provides the implementation for ImportBusinessObjectsPage View Model
    /// </summary>
    public class ImportBusinessObjectsViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Properties
        public ImportBusinessObjectCommand ImportBusinessObjectCommand
        {
            get
            {
                return (ImportBusinessObjectCommand)GetValue(ImportBusinessObjectCommandProperty);
            }
            set
            {
                SetValue(ImportBusinessObjectCommandProperty, value);
            }
        }
        public static readonly BindableProperty ImportBusinessObjectCommandProperty = BindableProperty.Create<ImportBusinessObjectsViewModel, ImportBusinessObjectCommand>(s => s.ImportBusinessObjectCommand, new ImportBusinessObjectCommand(), BindingMode.OneWay, null, null);
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

    /// <summary>
    /// Class used in ImportData Method
    /// </summary>
    public class BusinessObject
    {
        public string SalesPerson { get; set; }
        public int SalesJanJune { get; set; }
        public int SalesJulyDec { get; set; }
        public int Change { get; set; }
    }
}
