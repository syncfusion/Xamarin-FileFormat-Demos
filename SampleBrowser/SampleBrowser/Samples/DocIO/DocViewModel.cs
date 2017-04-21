using Xamarin.Forms;

namespace SampleBrowser
{
    public class DocViewModel : BindableObject
    {
        #region Fields
        private OpenWordTemplateFileCommand openTemplate;
        #endregion
        #region Constructor
        public DocViewModel()
        {
        }
        #endregion
        #region Properties
        /// <summary>
        /// Specifies the Command for Bar Chart Sample
        /// </summary>
        public BarChartCommand BarChartCommand
        {
            get
            {
                return (BarChartCommand)GetValue(BarChartCommandProperty);
            }
            set
            {
                SetValue(BarChartCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Bookmark Navigation Sample
        /// </summary>
        public BookmarkNavigationCommand BookmarkNavigationCommand
        {
            get
            {
                return (BookmarkNavigationCommand)GetValue(BookmarkNavigationCommandProperty);
            }
            set
            {
                SetValue(BookmarkNavigationCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Built-In Style Sample
        /// </summary>
        public BuiltInStyleCommand BuiltInStyleCommand
        {
            get
            {
                return (BuiltInStyleCommand)GetValue(BuiltInStyleCommandProperty);
            }
            set
            {
                SetValue(BuiltInStyleCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Custom style Sample
        /// </summary>
        public CustomStyleCommand CustomStyleCommand
        {
            get
            {
                return (CustomStyleCommand)GetValue(CustomStyleCommandProperty);
            }
            set
            {
                SetValue(CustomStyleCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Getting Started Sample
        /// </summary>
        public GettingStartedDocCommand GettingStartedDocCommand
        {
            get
            {
                return (GettingStartedDocCommand)GetValue(GettingStartedDocCommandProperty);
            }
            set
            {
                SetValue(GettingStartedDocCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Letter Formatting Sample
        /// </summary>
        public LetterFormattingCommand LetterFormattingCommand
        {
            get
            {
                return (LetterFormattingCommand)GetValue(LetterFormattingCommandProperty);
            }
            set
            {
                SetValue(LetterFormattingCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Pie Chart Sample
        /// </summary>
        public PieChartCommand PieChartCommand
        {
            get
            {
                return (PieChartCommand)GetValue(PieChartCommandProperty);
            }
            set
            {
                SetValue(PieChartCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Word to HTML Sample
        /// </summary>
        public WordToPDFCommand WordToPDFCommand
        {
            get
            {
                return (WordToPDFCommand)GetValue(WordToPDFCommandProperty);
            }
            set
            {
                SetValue(WordToPDFCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command for Word to HTML Sample
        /// </summary>
        public WordFilePickerCommand WordFilePickerCommand
        {
            get
            {
                return (WordFilePickerCommand)GetValue(WordFilePickerCommandProperty);
            }
            set
            {
                SetValue(WordFilePickerCommandProperty, value);
            }
        }
        /// <summary>
        /// Specifies the Command to open Word template for Letter Formatting Sample
        /// </summary>
        public OpenWordTemplateFileCommand OpenWordTemplateFileCommand
        {
            get
            {
                if (openTemplate == null)
                    openTemplate = new OpenWordTemplateFileCommand("Letter Formatting.docx");
                return openTemplate;
            }
            set
            {
                openTemplate = value;
            }
        }
        #endregion
        #region Static Properties
        public static INavigation Navigation;
        public static readonly BindableProperty BarChartCommandProperty = BindableProperty.Create<DocViewModel, BarChartCommand>(s => s.BarChartCommand, new BarChartCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty BookmarkNavigationCommandProperty = BindableProperty.Create<DocViewModel, BookmarkNavigationCommand>(s => s.BookmarkNavigationCommand, new BookmarkNavigationCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty BuiltInStyleCommandProperty = BindableProperty.Create<DocViewModel, BuiltInStyleCommand>(s => s.BuiltInStyleCommand, new BuiltInStyleCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty CustomStyleCommandProperty = BindableProperty.Create<DocViewModel, CustomStyleCommand>(s => s.CustomStyleCommand, new CustomStyleCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty GettingStartedDocCommandProperty = BindableProperty.Create<DocViewModel, GettingStartedDocCommand>(s => s.GettingStartedDocCommand, new GettingStartedDocCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty LetterFormattingCommandProperty = BindableProperty.Create<DocViewModel, LetterFormattingCommand>(s => s.LetterFormattingCommand, new LetterFormattingCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty PieChartCommandProperty = BindableProperty.Create<DocViewModel, PieChartCommand>(s => s.PieChartCommand, new PieChartCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty WordToPDFCommandProperty = BindableProperty.Create<DocViewModel, WordToPDFCommand>(s => s.WordToPDFCommand, new WordToPDFCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty OpenWordTemplateFileCommandProperty = BindableProperty.Create<DocViewModel, OpenWordTemplateFileCommand>(s => s.OpenWordTemplateFileCommand, new OpenWordTemplateFileCommand(), BindingMode.OneWay, null, null);
        public static readonly BindableProperty WordFilePickerCommandProperty = BindableProperty.Create<DocViewModel, WordFilePickerCommand>(s => s.WordFilePickerCommand, new WordFilePickerCommand(), BindingMode.OneWay, null, null);
        #endregion
    }
}
