using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COLOR = Syncfusion.Drawing;
using Xamarin.Forms;
using System.IO;
using System.Reflection;

namespace SampleBrowser
{
    public partial class SlidesPresentation : SamplePage
    {
        #region Constructor
        public SlidesPresentation()
        {
            InitializeComponent();

            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Description.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;

                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
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
                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Description.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
            }
        }
        #endregion
    }

    public class SlidesCommand : CommandBase
    {
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
            ManipulateSample();
        }
        /// <summary>
        /// Manipulates the slides sample.
        /// </summary>
        private void ManipulateSample()
        {
            string resourcePath = "SampleBrowser.Samples.Presentation.Templates.Slides.pptx";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(resourcePath);

            MemoryStream stream = new MemoryStream();
            //Opens the stream to the presentation instance.
            using (IPresentation presentation = Presentation.Open(fileStream))
            {
                CreateSlideWithTitle(presentation);
                CreateSlideWithParagraph(presentation);
                CreateSlideWithPicture(presentation);
                CreateSlideWithTable(presentation);
                //Saves the presentation instance to the stream.
                presentation.Save(stream);
            }
            stream.Position = 0;
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("SlidesSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("SlidesSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
        }

        #region HelperMethods
        /// <summary>
        /// Create slide with table in presentation.
        /// </summary>
        /// <param name="presentation">Represents the presentation instance.</param>
        private void CreateSlideWithTable(IPresentation presentation)
        {
            #region Slide4
            ISlide slide = presentation.Slides.Add(SlideLayoutType.TwoContent);
            IShape shape = slide.Shapes[0] as IShape;
            SetShapeBounds(shape, 36.72, 24.48, 815.04, 76.32);

            ITextBody textFrame = shape.TextBody;

            //Instance to hold paragraphs in textframe
            IParagraphs paragraphs = textFrame.Paragraphs;
            paragraphs.Add();
            IParagraph paragraph = paragraphs[0];
            ITextPart textpart = paragraph.AddTextPart();
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;

            //Assigns value to textpart
            textpart.Text = "Slide with Table";
            textpart.Font.FontName = "Helvetica CE 35 Thin";

            shape = slide.Shapes[1] as IShape;
            slide.Shapes.Remove(shape);

            ITable table = (ITable)slide.Shapes.AddTable(6, 6, 58.32, 154.08, 822.96, 273.6);
            foreach (IRow row in table.Rows)
                row.Height = 61.2;
            table.HasBandedRows = true;
            table.HasHeaderRow = true;
            table.HasBandedColumns = false;
            table.BuiltInStyle = BuiltInTableStyle.MediumStyle2Accent1;

            AddTableCellContent(table.Rows[0].Cells[0], "ID");
            AddTableCellContent(table.Rows[0].Cells[1], "Company Name");
            AddTableCellContent(table.Rows[0].Cells[2], "Contact Name");
            AddTableCellContent(table.Rows[0].Cells[3], "Address");
            AddTableCellContent(table.Rows[0].Cells[4], "City");
            AddTableCellContent(table.Rows[0].Cells[5], "Country");
            AddTableCellContent(table.Rows[1].Cells[0], "1");
            AddTableCellContent(table.Rows[1].Cells[1], "New Orleans Cajun Delights");
            AddTableCellContent(table.Rows[1].Cells[2], "Shelley Burke");
            AddTableCellContent(table.Rows[1].Cells[3], "P.O. Box 78934");
            AddTableCellContent(table.Rows[1].Cells[4], "New Orleans");
            AddTableCellContent(table.Rows[1].Cells[5], "USA");
            AddTableCellContent(table.Rows[2].Cells[0], "2");
            AddTableCellContent(table.Rows[2].Cells[1], "Cooperativa de Quesos 'Las Cabras");
            AddTableCellContent(table.Rows[2].Cells[2], "Antonio del Valle Saavedra");
            AddTableCellContent(table.Rows[2].Cells[3], "Calle del Rosal 4");
            AddTableCellContent(table.Rows[2].Cells[4], "Oviedo");
            AddTableCellContent(table.Rows[2].Cells[5], "Spain");
            AddTableCellContent(table.Rows[3].Cells[0], "3");
            AddTableCellContent(table.Rows[3].Cells[1], "Mayumi");
            AddTableCellContent(table.Rows[3].Cells[2], "Mayumi Ohno");
            AddTableCellContent(table.Rows[3].Cells[3], "92 Setsuko Chuo-ku");
            AddTableCellContent(table.Rows[3].Cells[4], "Osaka");
            AddTableCellContent(table.Rows[3].Cells[5], "Japan");
            AddTableCellContent(table.Rows[4].Cells[0], "4");
            AddTableCellContent(table.Rows[4].Cells[1], "Pavlova, Ltd.");
            AddTableCellContent(table.Rows[4].Cells[2], "Ian Devling");
            AddTableCellContent(table.Rows[4].Cells[3], "74 Rose St. Moonie Ponds");
            AddTableCellContent(table.Rows[4].Cells[4], "Melbourne");
            AddTableCellContent(table.Rows[4].Cells[5], "Australia");
            AddTableCellContent(table.Rows[5].Cells[0], "5");
            AddTableCellContent(table.Rows[5].Cells[1], "Specialty Biscuits, Ltd.");
            AddTableCellContent(table.Rows[5].Cells[2], "Peter Wilson");
            AddTableCellContent(table.Rows[5].Cells[3], "29 King's Way");
            AddTableCellContent(table.Rows[5].Cells[4], "Manchester");
            AddTableCellContent(table.Rows[5].Cells[5], "UK");

            slide.Shapes.RemoveAt(1);
            #endregion
        }

        /// <summary>
        /// Creates the slide with picture in the presentation.
        /// </summary>
        /// <param name="presentation">Represents the presentation instance.</param>
        private void CreateSlideWithPicture(IPresentation presentation)
        {
            #region Slide3
            ISlide slide = presentation.Slides.Add(SlideLayoutType.TwoContent);
            IShape shape = slide.Shapes[0] as IShape;
            SetShapeBounds(shape, 25.92, 36.72, 815.04, 76.32);

            //Adds textframe in shape
            ITextBody textFrame = shape.TextBody;

            //Instance to hold paragraphs in textframe
            IParagraphs paragraphs = textFrame.Paragraphs;
            paragraphs.Add();
            IParagraph paragraph = paragraphs[0];
            ITextPart textpart = paragraph.AddTextPart();
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;

            //Assigns value to textpart
            textpart.Text = "Slide with Image";
            textpart.Font.FontName = "Helvetica CE 35 Thin";

            //Adds shape in slide
            shape = slide.Shapes[1] as IShape;
            SetShapeBounds(shape, 578.16, 141.12, 316.08, 3326.16);
            textFrame = shape.TextBody;

            //Instance to hold paragraphs in textframe
            paragraphs = textFrame.Paragraphs;
            SetParagraphPropertiesForPictureSlide(paragraphs, "Lorem ipsum dolor sit amet, lacus amet amet ultricies. Quisque mi venenatis morbi libero, orci dis, mi ut et class porta, massa ligula magna enim, aliquam orci vestibulum tempus.");
            SetParagraphPropertiesForPictureSlide(paragraphs, "Turpis facilisis vitae consequat, cum a a, turpis dui consequat massa in dolor per, felis non amet.");
            SetParagraphPropertiesForPictureSlide(paragraphs, "Auctor eleifend in omnis elit vestibulum, donec non elementum tellus est mauris, id aliquam, at lacus, arcu pretium proin lacus dolor et. Eu tortor, vel ultrices amet dignissim mauris vehicula.");
            IShape shape3 = (IShape)slide.Shapes[2];
            slide.Shapes.RemoveAt(2);

            //Adds picture in the shape
            string resourcePath = "SampleBrowser.Samples.Presentation.Templates.tablet.jpg";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(resourcePath);
            slide.Shapes.AddPicture(fileStream, 58.32, 141.12, 477.36, 318.96);
            fileStream.Close();
            #endregion
        }

        /// <summary>
        /// Sets the paragraph properties for the picture slide.
        /// </summary>
        /// <param name="paragraphs">Represents the paragraph collection instance.</param>
        /// <param name="text">Represents the text content.</param>
        private void SetParagraphPropertiesForPictureSlide(IParagraphs paragraphs, string text)
        {
            IParagraph paragraph = paragraphs.Add();
            ITextPart textpart = paragraph.AddTextPart();
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;

            textpart.Text = text;
            textpart.Font.FontName = "Helvetica CE 35 Thin";
            textpart.Font.FontSize = 16;
        }

        /// <summary>
        /// Create the slides with paragraph instances.
        /// </summary>
        /// <param name="presentation">Represents the presentation instance.</param>
        private void CreateSlideWithParagraph(IPresentation presentation)
        {

            #region Slide2
            ISlide slide = presentation.Slides.Add(SlideLayoutType.SectionHeader);
            IShape shape = slide.Shapes[0] as IShape;
            SetShapeBounds(shape, 55.44, 23.04, 573.12, 71.28);

            ITextBody textFrame = shape.TextBody;

            //Instance to hold paragraphs in textframe
            IParagraphs paragraphs = textFrame.Paragraphs;
            IParagraph paragraph = paragraphs.Add();
            ITextPart textpart = paragraph.AddTextPart();
            paragraphs[0].HorizontalAlignment = HorizontalAlignmentType.Left;
            textpart.Text = "Slide with simple text";
            textpart.Font.FontName = "Helvetica CE 35 Thin";
            textpart.Font.FontSize = 40;
            //Get the shape from shape collection
            shape = slide.Shapes[1] as IShape;
            //Sets the properties of the shape.
            SetShapeBounds(shape, 87.12, 119.52, 725.76, 354.96);
            textFrame = shape.TextBody;

            //Instance to hold paragraphs in textframe
            paragraphs = textFrame.Paragraphs;
            SetParagraphProperties(paragraphs, "Lorem ipsum dolor sit amet, lacus amet amet ultricies. Quisque mi venenatis morbi libero, orci dis, mi ut et class porta, massa ligula magna enim, aliquam orci vestibulum tempus.");
            SetParagraphProperties(paragraphs, "Turpis facilisis vitae consequat, cum a a, turpis dui consequat massa in dolor per, felis non amet.");
            SetParagraphProperties(paragraphs, "Auctor eleifend in omnis elit vestibulum, donec non elementum tellus est mauris, id aliquam, at lacus, arcu pretium proin lacus dolor et. Eu tortor, vel ultrices amet dignissim mauris vehicula.");
            SetParagraphProperties(paragraphs, "Vestibulum duis integer diam mi libero felis, sollicitudin id dictum etiam blandit lacus, ac condimentum magna dictumst interdum et,");
            SetParagraphProperties(paragraphs, "nam commodo mi habitasse enim fringilla nunc, amet aliquam sapien per tortor luctus. Conubia voluptates at nunc, congue lectus, malesuada nulla.");
            SetParagraphProperties(paragraphs, "Rutrum quo morbi, feugiat sed mi turpis, ac cursus integer ornare dolor. Purus dui in et tincidunt, sed eros pede adipiscing tellus, est suscipit nulla,");
            SetParagraphProperties(paragraphs, "Auctor eleifend in omnis elit vestibulum, donec non elementum tellus est mauris, id aliquam, at lacus, arcu pretium proin lacus dolor et. Eu tortor, vel ultrices amet dignissim mauris vehicula.");
            SetParagraphProperties(paragraphs, "arcu nec fringilla vel aliquam, mollis lorem rerum hac vestibulum ante nullam. Volutpat a lectus, lorem pulvinar quis. Lobortis vehicula in imperdiet orci urna.");
            #endregion
        }

        /// <summary>
        /// Sets the paragraph properties.
        /// </summary>
        /// <param name="paragraphs">Represents the paragraph collection instance.</param>
        /// <param name="text">Represents the text content.</param>
        private void SetParagraphProperties(IParagraphs paragraphs, string text)
        {
            IParagraph paragraph = paragraphs.Add();
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;
            ITextPart textpart = paragraph.AddTextPart();
            textpart.Text = text;
            textpart.Font.FontName = "Calibri (Body)";
            textpart.Font.FontSize = 15;
            textpart.Font.Color = ColorObject.Black;
        }

        /// <summary>
        /// Creates slide with title content.
        /// </summary>
        /// <param name="presentation">Represents the presentation instance.</param>
        private void CreateSlideWithTitle(IPresentation presentation)
        {

            #region Slide1
            //Sets the size and position for the shape.
            ISlide slide = presentation.Slides[0];
            IShape shape = slide.Shapes[0] as IShape;
            //Sets the properties of the shape.
            SetShapeBounds(shape, 108, 139.68, 743.04, 144);

            //Sets the text property for the shape.
            ITextBody textFrame = shape.TextBody;
            IParagraphs paragraphs = textFrame.Paragraphs;
            IParagraph paragraph = paragraphs.Add();
            ITextPart textPart = paragraph.AddTextPart();
            paragraphs[0].IndentLevelNumber = 0;
            textPart.Text = "ESSENTIAL PRESENTATION";
            textPart.Font.FontName = "HelveticaNeue LT 65 Medium";
            textPart.Font.FontSize = 48;
            textPart.Font.Bold = true;
            slide.Shapes.RemoveAt(1);
            #endregion
        }

        /// <summary>
        /// Adds the content to the table cell.
        /// </summary>
        /// <param name="cell">Represents the cell instance in the table.</param>
        /// <param name="text">Represents the text content of the cell.</param>
        private void AddTableCellContent(ICell cell, string text)
        {
            IParagraph paragraph = cell.TextBody.Paragraphs.Add();
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;
            ITextPart textPart = paragraph.AddTextPart();
            textPart.Text = text;
        }
        /// <summary>
        /// Sets the bounds for the shape.
        /// </summary>
        /// <param name="shape">Represents the shape instance.</param>
        /// <param name="left">Represents the left position.</param>
        /// <param name="top">Represents the top position.</param>
        /// <param name="width">Represents the width of the shape.</param>
        /// <param name="height">Represents the height of the shape.</param>
        private void SetShapeBounds(IShape shape, double left, double top, double width, double height)
        {
            shape.Left = left;
            shape.Top = top;
            shape.Width = width;
            shape.Height = height;
        }
        #endregion HelperMethods
        #endregion
    }
}
