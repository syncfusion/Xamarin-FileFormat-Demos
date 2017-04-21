using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace SampleBrowser
{
    #region Sample page
    public partial class BookmarkNavigation : SamplePage
    {
        #region Constructor
        public BookmarkNavigation()
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
    public class BookmarkNavigationCommand : CommandBase
    {
        #region Constructor
        public BookmarkNavigationCommand()
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
            ManipulateBookmarkContents();
        }
        private void ManipulateBookmarkContents()
        {
            // Creating a new document.
            using (WordDocument document = new WordDocument())
            {
                #region Document Manipulations
                Assembly assembly = typeof(App).GetTypeInfo().Assembly;
                //Adds section with one empty paragraph to the Word document
                document.EnsureMinimal();
                //sets the page margins
                document.LastSection.PageSetup.Margins.All = 72f;
                //Appends bookmark to the paragraph
                document.LastParagraph.AppendBookmarkStart("NorthwindDatabase");
                document.LastParagraph.AppendText("Northwind database with relational data");
                document.LastParagraph.AppendBookmarkEnd("NorthwindDatabase");
                // Open an existing template document with single section.
                WordDocument nwdInformation = new WordDocument();
                Stream inputStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Bookmark_Template.doc");
                // Open an existing template document.
                nwdInformation.Open(inputStream, FormatType.Doc);
                inputStream.Dispose();
                // Open an existing template document with multiple section.
                WordDocument templateDocument = new WordDocument();
                inputStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.BkmkDocumentPart_Template.doc");
                // Open an existing template document.
                templateDocument.Open(inputStream, FormatType.Doc);
                inputStream.Dispose();
                // Creating a bookmark navigator. Which help us to navigate through the 
                // bookmarks in the template document.
                BookmarksNavigator bk = new BookmarksNavigator(templateDocument);
                // Move to the NorthWind bookmark in template document
                bk.MoveToBookmark("NorthWind");
                //Gets the bookmark content as WordDocumentPart
                WordDocumentPart documentPart = bk.GetContent();
                // Creating a bookmark navigator. Which help us to navigate through the 
                // bookmarks in the Northwind information document.
                bk = new BookmarksNavigator(nwdInformation);
                // Move to the information bookmark 
                bk.MoveToBookmark("Information");
                // Get the content of information bookmark.
                TextBodyPart bodyPart = bk.GetBookmarkContent();
                // Creating a bookmark navigator. Which help us to navigate through the 
                // bookmarks in the destination document.
                bk = new BookmarksNavigator(document);
                // Move to the NorthWind database in the destination document
                bk.MoveToBookmark("NorthwindDatabase");
                //Replace the bookmark content using word document parts
                bk.ReplaceContent(documentPart);
                // Move to the Northwind_Information in the destination document
                bk.MoveToBookmark("Northwind_Information");
                // Replacing content of Northwind_Information bookmark.
                bk.ReplaceBookmarkContent(bodyPart);
                // Move to the text bookmark
                bk.MoveToBookmark("Text");
                //Deletes the bookmark content
                bk.DeleteBookmarkContent(true);
                // Inserting text inside the bookmark. This will preserve the source formatting
                bk.InsertText("Northwind Database contains the following table:");
                #region tableinsertion
                WTable tbl = new WTable(document);
                tbl.TableFormat.Borders.BorderType = BorderStyle.None;
                tbl.TableFormat.IsAutoResized = true;
                tbl.ResetCells(8, 2);
                IWParagraph paragraph;
                tbl.Rows[0].IsHeader = true;
                paragraph = tbl[0, 0].AddParagraph();
                paragraph.AppendText("Suppliers");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[0, 1].AddParagraph();
                paragraph.AppendText("1");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[1, 0].AddParagraph();
                paragraph.AppendText("Customers");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[1, 1].AddParagraph();
                paragraph.AppendText("1");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[2, 0].AddParagraph();
                paragraph.AppendText("Employees");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[2, 1].AddParagraph();
                paragraph.AppendText("3");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[3, 0].AddParagraph();
                paragraph.AppendText("Products");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[3, 1].AddParagraph();
                paragraph.AppendText("1");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[4, 0].AddParagraph();
                paragraph.AppendText("Inventory");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[4, 1].AddParagraph();
                paragraph.AppendText("2");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[5, 0].AddParagraph();
                paragraph.AppendText("Shippers");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[5, 1].AddParagraph();
                paragraph.AppendText("1");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[6, 0].AddParagraph();
                paragraph.AppendText("PO Transactions");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[6, 1].AddParagraph();
                paragraph.AppendText("3");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[7, 0].AddParagraph();
                paragraph.AppendText("Sales Transactions");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;

                paragraph = tbl[7, 1].AddParagraph();
                paragraph.AppendText("7");
                paragraph.BreakCharacterFormat.FontName = "Calibri";
                paragraph.BreakCharacterFormat.FontSize = 10;


                bk.InsertTable(tbl);
                #endregion
                bk.MoveToBookmark("Image");
                bk.DeleteBookmarkContent(true);
                // Inserting image to the bookmark.
                IWPicture pic = bk.InsertParagraphItem(ParagraphItemType.Picture) as WPicture;
                inputStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Northwind.png");
                pic.LoadImage(inputStream);
                inputStream.Dispose();
                pic.WidthScale = 50f;  // It reduce the image size because it don't fit 
                pic.HeightScale = 75f; // in document page.
                #endregion
                #region Saving Document
                //Save the word document to stream.
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Save file in the disk based on specfic OS
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("BookMarkNavigation.docx", "application/msword", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("BookMarkNavigation.docx", "application/msword", stream);
                #endregion
            }
        }
        #endregion
    }
    #endregion
}
