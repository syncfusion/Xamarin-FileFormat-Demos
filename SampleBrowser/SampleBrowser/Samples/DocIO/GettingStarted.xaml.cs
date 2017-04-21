using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace SampleBrowser
{
    #region Sample Page
    public partial class GettingStartedDocIO : SamplePage
    {
        #region Constructor
        public GettingStartedDocIO()
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
    public class GettingStartedDocCommand : CommandBase
    {
        #region Constructor
        public GettingStartedDocCommand()
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
            CreateWordDocument();
        }
        private void CreateWordDocument()
        {
            // Creating a new document.
            using (WordDocument document = new WordDocument())
            {
                #region Creating a Word document
                //Adding a new section to the document.
                WSection section = document.AddSection() as WSection;
                //Set Margin of the section
                section.PageSetup.Margins.All = 72;
                //Set page size of the section
                section.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);

                //Create Paragraph styles
                WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11f;
                style.ParagraphFormat.BeforeSpacing = 0;
                style.ParagraphFormat.AfterSpacing = 8;
                style.ParagraphFormat.LineSpacing = 13.8f;

                style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
                style.ApplyBaseStyle("Normal");
                style.CharacterFormat.FontName = "Calibri Light";
                style.CharacterFormat.FontSize = 16f;
                style.CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(46, 116, 181);
                style.ParagraphFormat.BeforeSpacing = 12;
                style.ParagraphFormat.AfterSpacing = 0;
                style.ParagraphFormat.Keep = true;
                style.ParagraphFormat.KeepFollow = true;
                style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
                IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();
                //Append picture to the paragraph
                Assembly assembly = typeof(App).GetTypeInfo().Assembly;
                Stream imageStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.AdventureCycle.jpg");
                WPicture picture = paragraph.AppendPicture(imageStream) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
                picture.VerticalOrigin = VerticalOrigin.Margin;
                picture.VerticalPosition = -24;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = 263.5f;
                picture.WidthScale = 20;
                picture.HeightScale = 15;

                paragraph.ApplyStyle("Normal");
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
                WTextRange textRange = paragraph.AppendText("Adventure Works Cycles") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Calibri";
                textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Red;

                //Appends paragraph.
                paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
                textRange = paragraph.AppendText("Adventure Works Cycles") as WTextRange;
                textRange.CharacterFormat.FontSize = 18f;
                textRange.CharacterFormat.FontName = "Calibri";


                //Appends paragraph.
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("Adventure Works Cycles, the fictitious company on which the AdventureWorks sample databases are based, is a large, multinational manufacturing company. The company manufactures and sells metal and composite bicycles to North American, European and Asian commercial markets. While its base operation is located in Bothell, Washington with 290 employees, several regional sales teams are located throughout their market base.") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.FirstLineIndent = 36;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText("In 2000, Adventure Works Cycles bought a small manufacturing plant, Importadores Neptuno, located in Mexico. Importadores Neptuno manufactures several critical subcomponents for the Adventure Works Cycles product line. These subcomponents are shipped to the Bothell location for final product assembly. In 2001, Importadores Neptuno, became the sole manufacturer and distributor of the touring bicycle product group.") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;

                paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
                textRange = paragraph.AppendText("Product Overview") as WTextRange;
                textRange.CharacterFormat.FontSize = 16f;
                textRange.CharacterFormat.FontName = "Calibri";


                //Appends table.
                IWTable table = section.AddTable();
                table.ResetCells(3, 2);
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;
                table.TableFormat.IsAutoResized = true;

                //Appends paragraph.
                paragraph = table[0, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                imageStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Mountain-200.jpg");
                //Appends picture to the paragraph.
                picture = paragraph.AppendPicture(imageStream) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
                picture.VerticalOrigin = VerticalOrigin.Paragraph;
                picture.VerticalPosition = 0;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = -5.15f;
                picture.WidthScale = 79;
                picture.HeightScale = 79;

                //Appends paragraph.
                paragraph = table[0, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.AppendText("Mountain-200");
                //Appends paragraph.
                paragraph = table[0, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                paragraph.BreakCharacterFormat.FontName = "Times New Roman";

                textRange = paragraph.AppendText("Product No: BK-M68B-38\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Size: 38\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Weight: 25\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Price: $2,294.99\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";

                //Appends paragraph.
                paragraph = table[0, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.AppendText("Mountain-300 ");
                //Appends paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                paragraph.BreakCharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Product No: BK-M47B-38\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Size: 35\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Weight: 22\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Price: $1,079.99\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";

                //Appends paragraph.
                paragraph = table[1, 0].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;

                //Appends paragraph.
                paragraph = table[1, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.LineSpacing = 12f;
                imageStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Mountain-300.jpg");
                //Appends picture to the paragraph.
                picture = paragraph.AppendPicture(imageStream) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
                picture.VerticalOrigin = VerticalOrigin.Paragraph;
                picture.VerticalPosition = 8.2f;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = -14.95f;
                picture.WidthScale = 75;
                picture.HeightScale = 75;

                //Appends paragraph.
                paragraph = table[2, 0].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.LineSpacing = 12f;
                imageStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Road-550-W.jpg");
                //Appends picture to the paragraph.
                picture = paragraph.AppendPicture(imageStream) as WPicture;
                picture.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
                picture.VerticalOrigin = VerticalOrigin.Paragraph;
                picture.VerticalPosition = 0;
                picture.HorizontalOrigin = HorizontalOrigin.Column;
                picture.HorizontalPosition = -4.9f;
                picture.WidthScale = 92;
                picture.HeightScale = 92;

                //Appends paragraph.
                paragraph = table[2, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.AppendText("Road-150 ");
                //Appends paragraph.
                paragraph = table[2, 1].AddParagraph();
                paragraph.ParagraphFormat.AfterSpacing = 0;
                paragraph.ParagraphFormat.LineSpacing = 12f;
                paragraph.BreakCharacterFormat.FontSize = 12f;
                paragraph.BreakCharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Product No: BK-R93R-44\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Size: 44\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Weight: 14\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                textRange = paragraph.AppendText("Price: $3,578.27\r") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
                textRange.CharacterFormat.FontName = "Times New Roman";
                //Appends paragraph.
                paragraph = table[2, 1].AddParagraph();
                paragraph.ApplyStyle("Heading 1");
                paragraph.ParagraphFormat.LineSpacing = 12f;

                //Appends paragraph.
                section.AddParagraph();
                #endregion
                #region Saving a document
                //Saves the word document to stream.
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Save file in the disk based on specfic OS
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("GettingStarted.docx", "application/msword", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("GettingStarted.docx", "application/msword", stream);
                #endregion
            }
        }
        #endregion
    }
    #endregion
}
