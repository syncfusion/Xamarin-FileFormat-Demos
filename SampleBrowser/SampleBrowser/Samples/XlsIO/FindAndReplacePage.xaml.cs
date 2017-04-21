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
	public partial class FindAndReplacePage : SamplePage
	{
        #region Constructor
        public FindAndReplacePage()
		{            
			InitializeComponent ();
            this.viewModel.InitializePicker(picker);
			if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
			{
				this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
				this.Description.HorizontalOptions = LayoutOptions.Start;
				this.btnGenerate.HorizontalOptions = LayoutOptions.Start;
                this.btnTemplate.HorizontalOptions = LayoutOptions.Start;
                this.ButtonGrid.HorizontalOptions = LayoutOptions.Start;

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
				this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
            }           
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for FindAndReplaceCommand class.
    /// </summary>
    public class FindAndReplaceCommand : CommandBase
    {
        /// <summary>
        /// View Model for the class FindAndReplaceCommand 
        /// </summary>
        FindAndReplaceViewModel findAndReplaceViewModel;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FindAndReplaceCommand"/> class.
        /// </summary>
        public FindAndReplaceCommand()
        {
        }

        public FindAndReplaceCommand(FindAndReplaceViewModel viewModel)
        {
            findAndReplaceViewModel = viewModel;
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
            FindAndReplace(findAndReplaceViewModel.FindStringIndex, findAndReplaceViewModel.ReplaceText, findAndReplaceViewModel.IsMatchCase, findAndReplaceViewModel.IsMatchEntireCell);
        }
        private void FindAndReplace(int replaceIndex, string text, bool isMatchCase, bool isMatchEntireCell)
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.XlsIO.Template.ReplaceOptions.xlsx");

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

                string replaceText = "";

                switch (replaceIndex)
                {
                    default:
                    case 0:
                        replaceText = "Berlin";
                        break;

                    case 1:
                        replaceText = "8000";
                        break;

                    case 2:
                        replaceText = "Representative";
                        break;

                }
                ExcelFindOptions option = ExcelFindOptions.None;

                if (isMatchCase)
                {
                    //Set the option to match the case
                    option |= ExcelFindOptions.MatchCase;
                }

                if (isMatchEntireCell)
                {
                    //Set the option to match the entire cell content
                    option |= ExcelFindOptions.MatchEntireCellContent;
                }

                if (text != null && text != "")
                {
                    //Replace the text with specified value based on given find option 
                    sheet.Replace(replaceText, text, option);
                }

                //Set the version of the workbook.
                workbook.Version = ExcelVersion.Excel2013;

                // Saving the workbook in xlsx format
                workbook.SaveAs(stream);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("FindAndReplace.xlsx", "application/msexcel", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("FindAndReplace.xlsx", "application/msexcel", stream);
        }
        #endregion
    }

    /// <summary>
    /// Provides the implementation for FindAndReplacePage View Model
    /// </summary>
    public class FindAndReplaceViewModel : BindableObject, INotifyPropertyChanged
    {
        #region Fields
        private int findStringIndex;
        private string replaceText;
        private bool isMatchCase;
        private bool isMatchEntireCell;
        private ICommand findAndReplace;
        private OpenTemplateFileCommand openTemplate;
        #endregion

        #region Properties
        public int FindStringIndex
        {
            get
            {
                return findStringIndex;
            }
            set
            {
                findStringIndex = value;
                OnPropertyChanged("FindStringIndex");
            }
        }
        public string ReplaceText
        {
            get
            {
                return replaceText;
            }
            set
            {
                replaceText = value;
                OnPropertyChanged("ReplaceText");
            }
        }
        public bool IsMatchCase
        {
            get
            {
                return isMatchCase;
            }
            set
            {
                isMatchCase = value;
                OnPropertyChanged("IsMatchCase");
            }
        }
        public bool IsMatchEntireCell
        {
            get
            {
                return isMatchEntireCell;
            }
            set
            {
                isMatchEntireCell = value;
                OnPropertyChanged("IsMatchCase");
            }
        }
        public OpenTemplateFileCommand OpenTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenTemplateFileCommand("ReplaceOptions");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }
        public ICommand FindAndReplaceCommand
        {
            get
            {
                if (findAndReplace == null)
                    findAndReplace = new FindAndReplaceCommand(this);
                return findAndReplace;
            }
            set
            {
                findAndReplace = value;
            }
        }
        #endregion

        #region Implementation Methods
        internal void InitializePicker(Picker picker)
        {
            picker.Items.Add("Berlin");
            picker.Items.Add("8000");
            picker.Items.Add("Representative");
            picker.SelectedIndex = 0;            
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
    /// Class used for Picker events in FindAndReplacePage
    /// </summary>
    public class FindAndReplacePicker : Behavior<Picker>
    {
        /// <summary>
        /// View Model for the FilterPickerBehavior Class
        /// </summary>
        private FindAndReplaceViewModel viewModel;

        #region Implementation Methods
        protected override void OnAttachedTo(Picker bindable)
        {
            base.OnAttachedTo(bindable);
            viewModel = bindable.FindByName<StackLayout>("Layout").BindingContext as FindAndReplaceViewModel;
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
            viewModel.FindStringIndex = picker.SelectedIndex;
        }
        #endregion
    }
}

