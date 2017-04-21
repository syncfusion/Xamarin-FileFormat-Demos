using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using Xamarin.Forms;

namespace SampleBrowser
{
    #region Sample page
    public partial class BuiltInStyle : SamplePage
    {
        #region Constructor
        public BuiltInStyle()
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
    #region Command Impelmentation
    public class BuiltInStyleCommand : CommandBase
    {
        #region Constructor
        public BuiltInStyleCommand()
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
            ApplyBuiltInStyle();
        }
        private void ApplyBuiltInStyle()
        {
            // Creating a new document.
            using (WordDocument document = new WordDocument())
            {
                #region Create a Word document
                //Add new section to the Word document
                WSection section = document.AddSection() as WSection;
                //Set Margin of the section
                section.PageSetup.Margins.All = 72;
                //Add new paragraph
                WParagraph para = section.AddParagraph() as WParagraph;
                //Add multi-column to the section
                section.AddColumn(100, 100);
                section.AddColumn(100, 100);
                //Make the section columns are qual
                section.MakeColumnsEqual();
                #region Built-in styles
                #region List Style
                //Apply List Style
                para.AppendText("This para is written with style List").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.List);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                //Apply List5 style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style List5").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.List5);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                #endregion
                #region ListNumber Style
                //List Number style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style ListNumber").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ListNumber);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                //List Number5 style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style ListNumber5").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ListNumber5);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                #endregion
                #region TOA Heading Style
                //Apply TOA Heading style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style TOA Heading").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ToaHeading);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                #endregion
                //Add new paragraph
                para = section.AddParagraph() as WParagraph;
                //Set Section break
                section.BreakCode = SectionBreakCode.NewColumn;
                #region ListBullet Style
                //ListBullet
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style ListBullet").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ListBullet);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");

                //ListBullet5
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style ListBullet5").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ListBullet5);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");

                #endregion
                #region List Continue Style
                //Apply ListContinue Style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style ListContinue").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ListContinue);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                //Apply ListContinue5 style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style ListContinue5").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.ListContinue5);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                #endregion
                #region HTMLSample Style
                //Apply HtmlSample style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("\nThis para is written with style HtmlSample").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.HtmlSample);
                para.AppendText("Google Chrome\n");
                para.AppendText("Mozilla Firefox\n");
                para.AppendText("Internet Explorer");
                #endregion
                //Add new section to the document
                section = document.AddSection() as WSection;
                //Set Section break
                section.BreakCode = SectionBreakCode.NoBreak;
                #region Document Map Style
                //Apply Docuemnt Map Style
                para = section.AddParagraph() as WParagraph;
                para.AppendText("This para is written with style DocumentMap\n").CharacterFormat.UnderlineStyle = Syncfusion.Drawing.UnderlineStyle.Double;
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.DocumentMap);
                IWTextRange textrange = para.AppendText("Google Chrome\n");
                textrange.CharacterFormat.TextBackgroundColor = Syncfusion.Drawing.Color.Red;
                textrange = para.AppendText("Mozilla Firefox\n");
                textrange.CharacterFormat.TextBackgroundColor = Syncfusion.Drawing.Color.Red;
                textrange = para.AppendText("Internet Explorer");
                textrange.CharacterFormat.TextBackgroundColor = Syncfusion.Drawing.Color.Red;
                #endregion
                #region Heading Styles
                //Apply Heading Styles
                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading1);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading2);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading3);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading4);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading5);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading6);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading7);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading8);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                para = section.AddParagraph() as WParagraph;
                para.ApplyStyle(BuiltinStyle.Heading9);
                para.AppendText("Hello World. This para is written with style " + para.StyleName.ToString());

                #endregion
                #endregion Built-in styles
                #endregion
                #region Saving Document
                //Save the word document to the stream.
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Save file in the disk based on specfic OS
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("WordDocument_BuiltInStyles.docx", "application/msword", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("WordDocument_BuiltInStyles.docx", "application/msword", stream);
                #endregion
            }
        }
        #endregion
    }
    #endregion
}
