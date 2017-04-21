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
    public partial class ImagesPresentation : SamplePage
    {
        #region Constructor
        public ImagesPresentation()
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

    public class ImagesCommand : CommandBase
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
        /// Manipulates the images sample.
        /// </summary>
        private void ManipulateSample()
        {
            string resourcePath = "SampleBrowser.Samples.Presentation.Templates.Images.pptx";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(resourcePath);

            MemoryStream stream = new MemoryStream();
            //Opens the existing PowerPoint file from stream.
            using (IPresentation presentation = Presentation.Open(fileStream))
            {
                CreateTitleSlide(presentation);

                CreatePictureSlide(presentation);

                //Saves the presentation instance to the stream.
                presentation.Save(stream);
            }
            stream.Position = 0;
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("ImagesSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("ImagesSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
        }

        #region HelperMethods
        /// <summary>
        /// Creates slide with picture content.
        /// </summary>
        /// <param name="presentation">Represents the presentation instance.</param>
        private void CreatePictureSlide(IPresentation presentation)
        {
            #region Slide2
            //Adds a new slide into the PowerPoint presentation object
            ISlide slide = presentation.Slides.Add(SlideLayoutType.ContentWithCaption);
            //Sets the background color for the slide.
            slide.Background.Fill.FillType = FillType.Solid;
            slide.Background.Fill.SolidFill.Color = ColorObject.White;
            //Access the shape from the slide instance and set its size & position
            IShape shape = (IShape)slide.Shapes[0];
            SetShapeBounds(shape, 33.84, 82.8, 252, 353.52);
            //Adds the textual data into the shape body
            ITextBody textFrame = shape.TextBody;
            IParagraphs paragraphs = textFrame.Paragraphs;
            SetParagraphProperties(paragraphs.Add(), "Lorem ipsum dolor sit amet, lacus amet amet ultricies. Quisque mi venenatis morbi libero, orci dis, mi ut et class porta, massa ligula magna enim, aliquam orci vestibulum tempus.");
            paragraphs.Add();
            SetParagraphProperties(paragraphs.Add(), "Turpis facilisis vitae consequat, cum a a, turpis dui consequat massa in dolor per, felis non amet.");
            paragraphs.Add();
            SetParagraphProperties(paragraphs.Add(), "Auctor eleifend in omnis elit vestibulum, donec non elementum tellus est mauris, id aliquam, at lacus, arcu pretium proin lacus dolor et. Eu tortor, vel ultrices amet dignissim mauris vehicula.");
            paragraphs.Add();
            IParagraph paragraph4 = paragraphs.Add();
            SetParagraphProperties(paragraphs.Add(), "Lorem tortor neque, purus taciti quis id. Elementum integer orci accumsan minim phasellus vel.");
            paragraphs.Add();

            slide.Shapes.RemoveAt(1);
            slide.Shapes.RemoveAt(1);

            //Adds picture in the shape
            string resourcePath = "SampleBrowser.Samples.Presentation.Templates.tablet.jpg";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(resourcePath);
            slide.Shapes.AddPicture(fileStream, 373.96, 82.8, 525.6, 382.32);
            fileStream.Close();
            #endregion
        }

        private void CreateTitleSlide(IPresentation presentation)
        {

            #region Slide1
            //Access the first shape from the 1st slide and Sets the size & position for the shape.
            ISlide slide = presentation.Slides[0];
            IShape shape = (IShape)slide.Shapes[0];
            //Sets the properties of the shape.
            SetShapeBounds(shape, 91.44, 40.32, 687.6, 388.8);

            //Adds the textual data into the shape body
            //Shape body can contain collection of paragraph
            //each paragraph contain collection of TextPart where the textual data are preserved.
            ITextBody textFrame = shape.TextBody;
            IParagraphs paragraphs = textFrame.Paragraphs;
            paragraphs.Add();
            IParagraph paragraph = paragraphs[0];
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;
            ITextParts textParts = paragraph.TextParts;
            textParts.Add();
            ITextPart textPart = textParts[0];
            textPart.Text = "Essential Presentation ";
            textPart.Font.CapsType = TextCapsType.All;
            textPart.Font.FontName = "Calibri Light (Headings)";
            textPart.Font.FontSize = 80;
            textPart.Font.Color = ColorObject.Black;
            #endregion
        }

        /// <summary>
        /// Sets the paragraph properties.
        /// </summary>
        /// <param name="paragraph">Represents the paragraph instance.</param>
        /// <param name="text">Represents the text content.</param>
        private void SetParagraphProperties(IParagraph paragraph, string text)
        {
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;
            ITextPart textpart = paragraph.AddTextPart();
            textpart.Text = text;
            textpart.Font.Color = ColorObject.White;
            textpart.Font.FontName = "Calibri (Body)";
            textpart.Font.FontSize = 15;
        }

        /// <summary>
        /// Sets the bounds for the shape.
        /// </summary>
        /// <param name="shape">Represents the shape instance.</param>
        /// <param name="left">Represents the left position of the shape.</param>
        /// <param name="top">Represents the top position of the shape.</param>
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
