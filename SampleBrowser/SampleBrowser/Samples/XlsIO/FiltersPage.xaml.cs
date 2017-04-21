using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
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
    public partial class FiltersPage : SamplePage
    {
        #region Constructor
        public FiltersPage()
        {
            InitializeComponent();

            this.viewModel.InitializePicker(this.picker, this.Advanced);

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
                    this.Content_3.FontSize = 18.5;
                    this.Label1.FontSize = 18.5;
                    this.Label2.FontSize = 18.5;
                }
                else
                {
                    this.Description.FontSize = 13.5;
                    this.Content_3.FontSize = 13.5;
                    this.Label1.FontSize = 13.5;
                    this.Label2.FontSize = 13.5;
                }             
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
            }
			else if(Device.OS == TargetPlatform.iOS)
			{
                this.Content_3.FontSize = 16;
                this.Label1.FontSize = 16;
                this.Label2.FontSize = 16;               
            }
        }
        #endregion

        #region Event Methods
        void OnItemSelected(object sender, EventArgs e)
        {
            if (this.picker.SelectedIndex == 4)
            {
                this.DynamicGrid_1.IsVisible = true;
                this.DynamicGrid_2.IsVisible = true;
                this.DynamicGrid_3.IsVisible = true;
                this.DynamicGrid_4.IsVisible = true;
            }
            else
            {
                this.DynamicGrid_1.IsVisible = false;
                this.DynamicGrid_2.IsVisible = false;
                this.DynamicGrid_3.IsVisible = false;
                this.DynamicGrid_4.IsVisible = false;
            }
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for FiltersPage class.
    /// </summary>
    public class FilterCommand : CommandBase
    {
        /// <summary>
        /// View Model for the FilterCommand Class
        /// </summary>
        FiltersViewModel filtersViewmodel;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterCommand"/> class.
        /// </summary>
        public FilterCommand()
        {
        }
        public FilterCommand(FiltersViewModel viewmodel)
        {
            filtersViewmodel = viewmodel;
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
            ApplyFilter(filtersViewmodel.FilterTypeIndex, filtersViewmodel.AdvancedFilterIndex, filtersViewmodel.IsUniqueRecords);
        }

        /// <summary>
        /// Apply the different types of data filter
        /// </summary>
        /// <param name="FilterType">Filter Type index</param>
        /// <param name="AdvancedFilterIndex">Advanced Filter Type index</param>
        /// <param name="isUniqueRecords">Boolean value indicates whether it is unique or not.</param>
        private void ApplyFilter(int FilterType, int AdvancedFilterIndex, bool isUniqueRecords)
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = null;
            if (FilterType != 4)
                fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.FilterData.xlsx");
            else
                fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.AdvancedFilterData.xlsx");

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

                //Set the range to be filtered
                if (FilterType != 4)
                    sheet.AutoFilters.FilterRange = sheet.Range[1, 1, 49, 3];

                switch (FilterType)
                {
                    case 0:
                        //Applying custom filter
                        IAutoFilter filter1 = sheet.AutoFilters[0];
                        filter1.IsAnd = false;
                        filter1.FirstCondition.ConditionOperator = ExcelFilterCondition.Equal;
                        filter1.FirstCondition.DataType = ExcelFilterDataType.String;
                        filter1.FirstCondition.String = "Owner";

                        filter1.SecondCondition.ConditionOperator = ExcelFilterCondition.Equal;
                        filter1.SecondCondition.DataType = ExcelFilterDataType.String;
                        filter1.SecondCondition.String = "Sales Representative";
                        break;

                    case 1:
                        //Applying text filter
                        IAutoFilter filter2 = sheet.AutoFilters[0];
                        filter2.AddTextFilter(new string[] { "Owner", "Sales Representative", "Sales Associate" });
                        break;

                    case 2:
                        //Applying datetime filter
                        IAutoFilter filter3 = sheet.AutoFilters[1];
                        filter3.AddDateFilter(new DateTime(2004, 9, 1, 1, 0, 0, 0), DateTimeGroupingType.month);
                        filter3.AddDateFilter(new DateTime(2011, 1, 1, 1, 0, 0, 0), DateTimeGroupingType.year);
                        break;

                    case 3:
                        //Applying datetime filter
                        IAutoFilter filter4 = sheet.AutoFilters[1];
                        filter4.AddDynamicFilter(DynamicFilterType.Quarter1);
                        break;

                    case 4:
                        //Accessing the range to be filtered to perform advanced filter 
                        IRange filterRange = sheet.Range["A8:G51"];
                        //Accessing the range to be used as criteria to perform advanced filter 
                        IRange criteriaRange = sheet.Range["A2:B5"];
                        if (AdvancedFilterIndex == 0)
                        {
                            //Apply advanced filter and leave the filtered data in place
                            sheet.AdvancedFilter(ExcelFilterAction.FilterInPlace, filterRange, criteriaRange, null, isUniqueRecords);
                        }
                        else
                        {
                            IRange range = sheet.Range["I7:O7"];
                            range.Merge();
                            range.Text = "FilterCopy";
                            range.CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 112, 192);
                            range.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            range.CellStyle.Font.Bold = true;
                            IRange copyRange = sheet.Range["I8"];

                            //Apply advanced filter and copy the filtered data to new place
                            sheet.AdvancedFilter(ExcelFilterAction.FilterCopy, filterRange, criteriaRange, copyRange, isUniqueRecords);
                        }
                        break;
                }
                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;

                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("Filters.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("Filters.xlsx", "application/msexcel", stream);
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for FilersPage View Model
    /// </summary>
    public class FiltersViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Fields
        private int filterTypeIndex;
        private int advancedFilterIndex;
        private bool isUniqueRecords;
        private ICommand filter;
        private OpenTemplateFileCommand openTemplate;
        #endregion

        #region Properties
        public int FilterTypeIndex
        {
            get
            {
                return filterTypeIndex;
            }
            set
            {
                filterTypeIndex = value;
                OnPropertyChanged("FilterTypeIndex");
            }
        }
        public int AdvancedFilterIndex
        {
            get
            {
                return advancedFilterIndex;
            }
            set
            {
                advancedFilterIndex = value;
                OnPropertyChanged("AdvancedFilterIndex");
            }
        }
        public bool IsUniqueRecords
        {
            get
            {
                return isUniqueRecords;
            }
            set
            {
                isUniqueRecords = value;
                OnPropertyChanged("IsUniqueRecords");
            }
        }
        public ICommand FilterCommand
        {
            get
            {
                if (filter == null)
                    filter = new FilterCommand(this);
                return filter;
            }
            set
            {
                filter = value;
            }
        }
        public OpenTemplateFileCommand OpenTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenTemplateFileCommand("FilterData");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }
        #endregion

        #region Implementation Methods
        internal void InitializePicker(Picker picker, Picker Advanced)
        {
            picker.Items.Add("Custom Filter");
            picker.Items.Add("Text Filter");
            picker.Items.Add("DateTime Filter");
            picker.Items.Add("Dynamic Filter");
            picker.Items.Add("Advanced Filter");
            Advanced.Items.Add("Filter In Place");
            Advanced.Items.Add("Filter Copy");
            picker.SelectedIndex = 0;
            Advanced.SelectedIndex = 0;
        }
        #endregion

        #region Event Methods
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
    
    /// <summary>
    /// Class used for Picker events in FilterPage
    /// </summary>
    public class FilterPickerBehavior : Behavior<Picker>
    {
        /// <summary>
        /// View Model for the FilterPickerBehavior Class
        /// </summary>
        private FiltersViewModel viewModel;

        #region Implementation Methods
        protected override void OnAttachedTo(Picker bindable)
        {
            base.OnAttachedTo(bindable);
            viewModel = bindable.FindByName<StackLayout>("Layout").BindingContext as FiltersViewModel;
            bindable.SelectedIndexChanged += Bindable_SelectedIndexChanged;
        }
        protected override void OnDetachingFrom(Picker bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SelectedIndexChanged -= Bindable_SelectedIndexChanged;
        }
        private void Bindable_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.Items.Count == 5)
            {
                viewModel.FilterTypeIndex = picker.SelectedIndex;
                if (picker.SelectedIndex == 4)
                    viewModel.OpenTemplateFileCommand.FileName = "AdvancedFilterData";
                else
                    viewModel.OpenTemplateFileCommand.FileName = "FilterData";
            }
            else
                viewModel.AdvancedFilterIndex = picker.SelectedIndex;
        }
        #endregion
    }
}

