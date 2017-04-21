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
using System.Xml.Linq;
using Syncfusion.OfficeChart;

namespace SampleBrowser
{
    public partial class ChartsPresentation : SamplePage
    {
        #region Constructor
        public ChartsPresentation()
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

    public class ChartsCommand : CommandBase
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
        /// Manipulates the chart sample.
        /// </summary>
        private void ManipulateSample()
        {
            MemoryStream stream = new MemoryStream();
            //Opens the existing presentation stream.
            using (IPresentation presentation = Presentation.Create())
            {
                ISlide slide = presentation.Slides.Add(SlideLayoutType.TitleOnly);
                IParagraph paragraph = ((IShape)slide.Shapes[0]).TextBody.Paragraphs.Add();
                //Apply center alignment to the paragraph
                paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;
                //Add slide title
                ITextPart textPart = paragraph.AddTextPart("Northwind Management Report");
                textPart.Font.Color = ColorObject.FromArgb(46, 116, 181);
                //Get chart data from xml file
                List<ProductDetails> Products = LoadXMLData();
                //Add a new chart to the presentation slide
                IPresentationChart chart = slide.Charts.AddChart(44.64, 133.2, 870.48, 380.16);
                //Set chart type
                chart.ChartType = OfficeChartType.Pie;
                //Set chart title
                chart.ChartTitle = "Best Selling Products";
                //Set chart properties font name and size
                chart.ChartTitleArea.FontName = "Calibri (Body)";
                chart.ChartTitleArea.Size = 14;
                for (int i = 0; i < Products.Count; i++)
                {
                    ProductDetails product = Products[i];
                    chart.ChartData.SetValue(i + 2, 1, product.ProductName);
                    chart.ChartData.SetValue(i + 2, 2, product.Sum);
                }
                //Create a new chart series with the name “Sales”
                AddSeriesForChart(chart);
                //Setting the font size of the legend.
                chart.Legend.TextArea.Size = 14;
                //Setting background color
                chart.ChartArea.Fill.ForeColor = Syncfusion.Drawing.Color.FromArgb(242, 242, 242);
                chart.PlotArea.Fill.ForeColor = Syncfusion.Drawing.Color.FromArgb(242, 242, 242);
                chart.ChartArea.Border.LinePattern = OfficeChartLinePattern.None;
                chart.PrimaryCategoryAxis.CategoryLabels = chart.ChartData[2, 1, 11, 1];
                //Saves the presentation instance to the stream.
                presentation.Save(stream);
            }
            stream.Position = 0;
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("ChartsSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save("ChartsSample.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", stream);
        }


        #region Helper Methods
        /// <summary>
        /// Gets list of product details from an XML file
        /// </summary>
        /// <returns></returns>
        private List<ProductDetails> LoadXMLData()
        {
            XDocument productXml;
            List<ProductDetails> Products = new List<ProductDetails>();
            ProductDetails productDetails;
            //Load XML file
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream productXMLStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.Presentation.Templates.Products.xml");
            productXml = XDocument.Load(productXMLStream);
            //Get list of product details
            IEnumerable<XElement> productElements = from product in productXml.Descendants("Product") select product;
            string serailNo = string.Empty;
            string productName = string.Empty;
            string sum = string.Empty;
            foreach (XElement element in productElements)
            {
                foreach (XElement child in element.Descendants())
                {
                    var childElement = element.Element(child.Name);
                    if (childElement != null)
                    {
                        string value = childElement.Value;
                        string elementName = child.Name.ToString();
                        switch (elementName)
                        {
                            case "SNO":
                                serailNo = value;
                                break;
                            case "ProductName":
                                productName = value;
                                break;
                            case "Sum":
                                sum = value;
                                break;
                        }
                    }
                }
                productDetails = new ProductDetails(int.Parse(serailNo), productName, decimal.Parse(sum));
                Products.Add(productDetails);
            }
            return Products;
        }

        /// <summary>
        /// Adds the series for the chart.
        /// </summary>
        /// <param name="chart">Represents the chart instance from the presentation.</param>
        private void AddSeriesForChart(IPresentationChart chart)
        {
            //Add a series for the chart.
            IOfficeChartSerie series = chart.Series.Add("Sales");
            series.Values = chart.ChartData[2, 2, 11, 2];
            //Setting data label
            series.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
            series.DataPoints.DefaultDataPoint.DataLabels.Position = OfficeDataLabelPosition.Outside;
            series.DataPoints.DefaultDataPoint.DataLabels.Size = 14;
        }

        #endregion HelperMethods

        #endregion Implementation
    }

    #region Helper class
    /// <summary>
    /// Specifies the Product details
    /// </summary>
    public class ProductDetails
    {
        #region fields

        private int m_serialNo;
        private string m_productName;
        private decimal m_sum;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the serial number of the product.
        /// </summary>
        public int SNO
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName
        {
            get { return m_productName; }
            set { m_productName = value; }
        }

        /// <summary>
        /// Gets or sets the sum value of the product.
        /// </summary>
        public decimal Sum
        {
            get { return m_sum; }
            set { m_sum = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the ProductDetails to create a new instance.
        /// </summary>
        /// <param name="serialNumber">Represents the serial number of the product.</param>
        /// <param name="productName">Represents the product name.</param>
        /// <param name="sum">Represents the sum value of the product.</param>
        public ProductDetails(int serialNumber, string productName, decimal sum)
        {
            SNO = serialNumber;
            ProductName = productName;
            Sum = Math.Round(sum, 3);
        }

        #endregion
    }
    #endregion
}
