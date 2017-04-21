using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using Xamarin.Forms;

namespace SampleBrowser
{
    #region Sample page
    public partial class CustomStyle : SamplePage
    {
        #region Constructor
        public CustomStyle()
        {
            InitializeComponent();

            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Content_1.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;

                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Content_1.VerticalOptions = LayoutOptions.Center;
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
                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Content_1.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
            }
        }
        #endregion
    }
    #endregion
    #region Command Implementation
    public class CustomStyleCommand : CommandBase
    {
        #region Constructor
        public CustomStyleCommand()
        {
        }
        #endregion

        #region Implementation
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed,
        /// this object can be set to null.
        /// </param>
        protected override void ExecuteCommand(object parameter)
        {
            ApplyCustomStyles();
        }
        private void ApplyCustomStyles()
        {
            using (WordDocument document = new WordDocument())
            {
                #region Apply Custom Styles
                IWParagraphStyle style = null;
                // Adding a new section to the document.
                WSection section = document.AddSection() as WSection;
                //Set Margin of the section
                section.PageSetup.Margins.All = 72;
                IWParagraph par = document.LastSection.AddParagraph();
                WTextRange range = par.AppendText("Using CustomStyles") as WTextRange;
                range.CharacterFormat.TextBackgroundColor = Syncfusion.Drawing.Color.Red;
                range.CharacterFormat.TextColor = Syncfusion.Drawing.Color.White;
                range.CharacterFormat.FontSize = 18f;
                document.LastParagraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                #region Create custom styles
                // Create Paragraph styles
                style = document.AddParagraphStyle("MyStyle_Normal");
                style.CharacterFormat.FontName = "Bitstream Vera Serif";
                style.CharacterFormat.FontSize = 10f;
                style.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Justify;
                style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(0, 21, 84);

                style = document.AddParagraphStyle("MyStyle_Low");
                style.CharacterFormat.FontName = "Times New Roman";
                style.CharacterFormat.FontSize = 16f;
                style.CharacterFormat.Bold = true;

                style = document.AddParagraphStyle("MyStyle_Medium");
                style.CharacterFormat.FontName = "Monotype Corsiva";
                style.CharacterFormat.FontSize = 18f;
                style.CharacterFormat.Bold = true;
                style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(51, 66, 125);

                style = document.AddParagraphStyle("Mystyle_High");
                style.CharacterFormat.FontName = "Bitstream Vera Serif";
                style.CharacterFormat.FontSize = 20f;
                style.CharacterFormat.Bold = true;
                style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(242, 151, 50);
                #endregion
                IWParagraph paragraph = null;
                for (int i = 1; i < document.Styles.Count; i++)
                {
                    //Skip to apply the document default styles and also paragraph style.
                    if (document.Styles[i].Name == "Normal" || document.Styles[i].Name == "Default Paragraph Font" || document.Styles[i].StyleType != StyleType.ParagraphStyle)
                        continue;
                    // Getting styles from Document.
                    style = (IWParagraphStyle)document.Styles[i];
                    // Adding a new paragraph
                    section.AddParagraph();
                    paragraph = section.AddParagraph();
                    // Applying styles to the current paragraph.
                    paragraph.ApplyStyle(style.Name);
                    // Writing Text with the current style and formatting.
                    paragraph.AppendText("Northwind Database with [" + style.Name + "] Style");
                    // Adding a new paragraph
                    section.AddParagraph();
                    paragraph = section.AddParagraph();
                    // Applying another style to the current paragraph.
                    paragraph.ApplyStyle("MyStyle_Normal");
                    // Writing text with current style.
                    paragraph.AppendText("The Northwind sample database (Northwind.mdb) is included with all versions of Access. It provides data you can experiment with and database objects that demonstrate features you might want to implement in your own databases. Using Northwind, you can become familiar with how a relational database is structured and how the database objects work together to help you enter, store, manipulate, and print your data.");
                }
                #endregion
                #region Saving Document
                //Save the word document to stream.
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Save file in the disk based on specfic OS
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("WordDocument_CustomStyles.docx", "application/msword", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("WordDocument_CustomStyles.docx", "application/msword", stream);
                #endregion
            }
        }
        #endregion
    }
    #endregion
}
