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
    public partial class GettingStartedPresentation : SamplePage
    {
        #region Constructor
        public GettingStartedPresentation()
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

    public class GettingStartedCommand : CommandBase
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
        /// Manipulates the gettingstarted sample.
        /// </summary>
        private void ManipulateSample()
        {
            string resourcePath = "SampleBrowser.Samples.Presentation.Templates.HelloWorld.pptx";
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream fileStream = assembly.GetManifestResourceStream(resourcePath);
            MemoryStream stream = new MemoryStream();
            //Open the stream to the presentation instance.
            using (IPresentation presentation = Presentation.Open(fileStream))
            {
                #region Slide1
                ISlide slide = presentation.Slides[0];
                IShape titleShape = slide.Shapes[0] as IShape;
                //Sets the shape size and position.
                SetShapeBounds(titleShape, 23.76, 41.76, 900, 126);

                //Sets the properties for the textbody of the shape.
                ITextBody textFrame = (slide.Shapes[0] as IShape).TextBody;
                IParagraphs paragraphs = textFrame.Paragraphs;
                IParagraph paragraph = paragraphs.Add();
                paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;
                ITextPart textPart = paragraph.AddTextPart();
                textPart.Text = "Essential Presentation";
                textPart.Font.CapsType = TextCapsType.All;
                textPart.Font.FontName = "Adobe Garamond Pro";
                textPart.Font.Bold = true;
                textPart.Font.FontSize = 40;

                //Sets the shape size and position.
                IShape subtitle = slide.Shapes[1] as IShape;
                SetShapeBounds(subtitle, 36, 216, 849.6, 122.4);

                //Sets the properties for the textbody of the shape.
                textFrame = (slide.Shapes[1] as IShape).TextBody;
                textFrame.VerticalAlignment = VerticalAlignmentType.Top;
                paragraphs = textFrame.Paragraphs;
                AddParagraphProperties(paragraphs.Add(), "Lorem ipsum dolor sit amet, lacus amet amet ultricies. Quisque mi venenatis morbi libero, orci dis, mi ut et class porta, massa ligula magna enim, aliquam orci vestibulum tempus.Turpis facilisis vitae consequat, cum a a, turpis dui consequat massa in dolor per, felis non amet.");

                //Add text content to the paragraph.
                AddParagraphProperties(paragraphs.Add(), "Turpis facilisis vitae consequat, cum a a, turpis dui consequat massa in dolor per, felis non amet. Auctor eleifend in omnis elit vestibulum, donec non elementum tellus est mauris, id aliquam, at lacus, arcu pretium proin lacus dolor et. Eu tortor, vel ultrices amet dignissim mauris vehicula.");
                #endregion

                //Saves the presentation into the stream.
                presentation.Save(stream);
            }
            stream.Position = 0;
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("GettingStartedSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("GettingStartedSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
        }

        #region HelperMethods
        /// <summary>
        /// Adds formatting properties for the paragraph.
        /// </summary>
        /// <param name="paragraph">Represents the paragraph instance.</param>
        /// <param name="text">Represents the text content.</param>
        private void AddParagraphProperties(IParagraph paragraph, string text)
        {
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;
            ITextPart textPart = paragraph.AddTextPart();
            textPart.Text = text;
            textPart.Font.FontName = "Adobe Garamond Pro";
            textPart.Font.FontSize = 21;
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
