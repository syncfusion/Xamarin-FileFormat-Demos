using Syncfusion.SfDataGrid.XForms;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class ExportToDataGrid : SamplePage
    {
        #region Constructor
        public ExportToDataGrid()
        {
            InitializeComponent();            
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Description.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;
                this.btnTemplate.HorizontalOptions = LayoutOptions.Start;
                this.ButtonGrid.HorizontalOptions = LayoutOptions.Start;

                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.BackgroundColor = Color.Gray;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
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
                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
            }

            if (Device.Idiom == TargetIdiom.Phone && Device.OS != TargetPlatform.iOS)
            {
                this.dataGrid.DefaultColumnWidth = 120;
                //this.dataGrid.ColumnSizer = ColumnSizer.Auto;
                //this.dataGrid.AllowResizingColumn = true;
            }
            else
                this.dataGrid.DefaultColumnWidth = 160;
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for ExportToGridCommand class.
    /// </summary>
    public class ExportToGridCommand : CommandBase
    {
        /// <summary>
        /// View Model objects for ExportToDataGrid 
        /// </summary>
        ExportingViewModel exportViewModel;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportToGridCommand"/> class.
        /// </summary>
        public ExportToGridCommand()
        {
        }

        public ExportToGridCommand(ExportingViewModel viewModel)
        {
            exportViewModel = viewModel;
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
            if(!exportViewModel.IsDataGridExported)
                ImportDataFromExcel();            
        }

        /// <summary>
        /// Import the data from Excel
        /// </summary>
        private void ImportDataFromExcel()
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = null;
            fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.CFTemplate.xlsx");

            ObservableCollection<CustomerObject> customers = new ObservableCollection<CustomerObject>();
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
                IWorksheet worksheet = workbook.Worksheets[0];                

                //Looping through the cells and get the values
                for (int r = 7; r <= 46; r++)
                {
                    CustomerObject customer = new CustomerObject();
                    customer.SalesPerson = worksheet[r, 2].Text;
                    customer.SalesJanJune = (int)worksheet[r, 3].Number;
                    customer.SalesJulyDec = (int)worksheet[r, 4].Number;
                    customer.Change = worksheet[r, 5].DisplayText;
                    customers.Add(customer);
                }
            }
            this.exportViewModel.CustomersInfo = customers;
            exportViewModel.IsDataGridExported = true;
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for ExportToDataGrid View Model
    /// </summary>
    public class ExportingViewModel : INotifyPropertyChanged
    {
        private bool isDataGridExported;
        private OpenTemplateFileCommand openTemplate;
        private ICommand exportToDataGrid;

        public bool IsDataGridExported
        {
            get
            {
                return isDataGridExported;
            }
            set
            {
                isDataGridExported = value;
                RaisePropertyChanged("IsDataGridExported");
            }
        }
        public OpenTemplateFileCommand OpenTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenTemplateFileCommand("GridExportTemplate");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }       
        public ICommand ExportToGridCommand
        {
            get
            {
                if (exportToDataGrid == null)
                    exportToDataGrid = new ExportToGridCommand(this);
                return exportToDataGrid;
            }
            set
            {
                exportToDataGrid = value;
            }
        }
        public ExportingViewModel()
        {
            
        }

        #region ItemsSource
        private ObservableCollection<CustomerObject> customersInfo;
        public ObservableCollection<CustomerObject> CustomersInfo
        {
            get { return customersInfo; }
            set { this.customersInfo = value; RaisePropertyChanged("CustomersInfo"); }
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }    

    /// <summary>
    /// Class used to store the customer details
    /// </summary>
    public class CustomerObject : INotifyPropertyChanged
    {
        public CustomerObject()
        {
        }

        #region private variables

        private string _salesPerson;
        private int _salesJanJune;
        private int _salesJulyDec;
        private string _change;

        #endregion

        #region Public Properties

        public string SalesPerson
        {
            get
            {
                return _salesPerson;
            }
            set
            {
                this._salesPerson = value;
                RaisePropertyChanged("SalesPerson");
            }
        }

        public int SalesJanJune
        {
            get
            {
                return _salesJanJune;
            }
            set
            {
                this._salesJanJune = value;
                RaisePropertyChanged("SalesJanJune");
            }
        }

        public int SalesJulyDec
        {
            get
            {
                return _salesJulyDec;
            }
            set
            {
                this._salesJulyDec = value;
                RaisePropertyChanged("SalesJulyDec");
            }
        }

        public string Change
        {
            get
            {
                return _change;
            }
            set
            {
                this._change = value;
                RaisePropertyChanged("Change");
            }
        }
        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String Name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(Name));
        }

        #endregion
    }
}
