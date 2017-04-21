using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.OfficeChart;
using Xamarin.Forms;
using System.Xml.Linq;

namespace SampleBrowser
{
    #region Sample page
    public partial class PieChart : SamplePage
    {
        #region Constructor
        public PieChart()
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
                this.btnGenerate.BackgroundColor = Xamarin.Forms.Color.Gray;
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
    public class PieChartCommand : CommandBase
    {
        #region Constructor
        public PieChartCommand()
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
            CreatePieChart();
        }
        private void CreatePieChart()
        {
            //Creating a new Word document
            using (WordDocument document = new WordDocument())
            {
                #region Create Pie chart
                //Add new section to the Word document
                IWSection section = document.AddSection();
                //Set page margins of the section
                section.PageSetup.Margins.All = 72;
                //Add new paragraph to the section
                IWParagraph paragraph = section.AddParagraph();
                //Apply heading style to the title paragraph
                paragraph.ApplyStyle(BuiltinStyle.Heading1);
                //Apply center alignment to the paragraph
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                //Append text to the paragraph
                paragraph.AppendText("Northwind Management Report").CharacterFormat.TextColor = Syncfusion.Drawing.Color.FromArgb(46, 116, 181);
                //Add new paragraph
                paragraph = section.AddParagraph();
                //Get chart data from xml file
                List<ProductDetail> Products = LoadXMLData();
                //Create and Append chart to the paragraph
                WChart pieChart = document.LastParagraph.AppendChart(446, 270);
                //Set chart data
                pieChart.ChartType = OfficeChartType.Pie;
                pieChart.ChartTitle = "Best Selling Products";
                pieChart.ChartTitleArea.FontName = "Calibri (Body)";
                pieChart.ChartTitleArea.Size = 14;
                for (int i = 0; i < Products.Count; i++)
                {
                    ProductDetail product = Products[i];
                    pieChart.ChartData.SetValue(i + 2, 1, product.ProductName);
                    pieChart.ChartData.SetValue(i + 2, 2, product.Sum);
                }
                //Create a new chart series with the name “Sales”
                IOfficeChartSerie pieSeries = pieChart.Series.Add("Sales");
                pieSeries.Values = pieChart.ChartData[2, 2, 11, 2];
                //Setting data label
                pieSeries.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                pieSeries.DataPoints.DefaultDataPoint.DataLabels.Position = OfficeDataLabelPosition.Outside;
                //Setting background color
                pieChart.ChartArea.Fill.ForeColor = Syncfusion.Drawing.Color.FromArgb(242, 242, 242);
                pieChart.PlotArea.Fill.ForeColor = Syncfusion.Drawing.Color.FromArgb(242, 242, 242);
                pieChart.ChartArea.Border.LinePattern = OfficeChartLinePattern.None;
                pieChart.PrimaryCategoryAxis.CategoryLabels = pieChart.ChartData[2, 1, 11, 1];
                #endregion
                #region Saving a document
                //Saves the word document to stream.
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Save file in the disk based on specfic OS
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>()
                        .Save("PieChart.docx", "application/msword", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("PieChart.docx", "application/msword", stream);
                #endregion
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Gets list of product details from an XML file
        /// </summary>
        /// <returns></returns>
        private List<ProductDetail> LoadXMLData()
        {
            XDocument productXml;
            List<ProductDetail> Products = new List<ProductDetail>();
            ProductDetail productDetails;
            //Load XML file
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream productXMLStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Products.xml");
            productXml = XDocument.Load(productXMLStream);
            //Get list of product details
            IEnumerable<XElement> pc = from p in productXml.Descendants("Product") select p;
            string serailNo = string.Empty;
            string productName = string.Empty;
            string sum = string.Empty;
            foreach (XElement dt in pc)
            {
                foreach (XElement el in dt.Descendants())
                {
                    var xElement = dt.Element(el.Name);
                    if (xElement != null)
                    {
                        string value = xElement.Value;
                        string elementName = el.Name.ToString();
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
                productDetails = new ProductDetail(int.Parse(serailNo), productName, decimal.Parse(sum));
                Products.Add(productDetails);
            }
            return Products;
        }
        #endregion
    }
    #endregion
    #region Helper class
    /// <summary>
    /// Specifies the Product details
    /// </summary>
    public class ProductDetail
    {
        #region fields

        private int m_serialNo;
        private string m_productName;
        private decimal m_sum;

        #endregion

        #region properties

        public int SNO
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }

        public string ProductName
        {
            get { return m_productName; }
            set { m_productName = value; }
        }

        public decimal Sum
        {
            get { return m_sum; }
            set { m_sum = value; }
        }

        #endregion

        #region Constructor

        public ProductDetail(int serialNumber, string productName, decimal sum)
        {
            SNO = serialNumber;
            ProductName = productName;
            Sum = Math.Round(sum, 3);
        }

        #endregion
    }
    #endregion
}
