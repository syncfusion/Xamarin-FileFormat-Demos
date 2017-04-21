using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleBrowser
{
    #region Sample Page
    public partial class LetterFormatting : SamplePage
    {
        #region Constructor
        public LetterFormatting()
        {
            InitializeComponent();

            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                this.SampleTitle.HorizontalOptions = LayoutOptions.Start;
                this.Content_1.HorizontalOptions = LayoutOptions.Start;
                this.btnGenerate.HorizontalOptions = LayoutOptions.Start;
                this.btnTemplate.HorizontalOptions = LayoutOptions.Start;
                this.ButtonGrid.HorizontalOptions = LayoutOptions.Start;

                this.SampleTitle.VerticalOptions = LayoutOptions.Center;
                this.Content_1.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.VerticalOptions = LayoutOptions.Center;
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
                this.btnGenerate.BackgroundColor = Color.Gray;
                this.btnTemplate.BackgroundColor = Color.Gray;
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
                this.btnTemplate.VerticalOptions = LayoutOptions.Center;
            }
        }
        #endregion
    }
    #endregion
    #region Command Implementation
    public class LetterFormattingCommand : CommandBase
    {
        #region Constructor
        public LetterFormattingCommand()
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
            CreateLetterFormat();
        }
        private void CreateLetterFormat()
        {
            // Creating a new document.
            using (WordDocument document = new WordDocument())
            {
                #region Execute Mail merge
                //Load Template document
                Assembly assembly = typeof(App).GetTypeInfo().Assembly;
                Stream inputStream = assembly.GetManifestResourceStream("SampleBrowser.Samples.DocIO.Templates.Letter Formatting.docx");
                //Open Template document
                document.Open(inputStream, FormatType.Word2013);
                inputStream.Dispose();
                //Create data source
                List<Customer> source = new List<Customer>();
                source.Add(new Customer("ALFKI", "Alfreds Futterkiste", "Maria Anders", "Sales Representative", "Obere Str. 57", "Berlin", "12209", "Germany", "030-0074321", "030-0076545"));
                //Execute Mail merge into a Word document
                document.MailMerge.Execute(source);
                #endregion
                #region Saving a Word document
                //Saves the Word document to stream.
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Save file in the disk based on specfic OS
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                    Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save("LetterFormatting.docx", "application/msword", stream);
                else
                    Xamarin.Forms.DependencyService.Get<ISave>().Save("LetterFormatting.docx", "application/msword", stream);
                #endregion
            }
        }
        #endregion
    }
    #endregion
    #region Helper classes
    /// <summary>
    /// Command class used to open the input Word template file
    /// </summary>
    public class OpenWordTemplateFileCommand : CommandBase
    {
        string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWordTemplateFileCommand"/> class.
        /// </summary>
        public OpenWordTemplateFileCommand()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenWordTemplateFileCommand"/> class with file name.
        /// </summary>
        public OpenWordTemplateFileCommand(string name)
        {
            fileName = name;
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
            string resourcePath = "SampleBrowser.Samples.DocIO.Templates." + FileName;
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            MemoryStream memStream = new MemoryStream(buffer);
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                Xamarin.Forms.DependencyService.Get<ISaveWindowsPhone>().Save(FileName, "application/msword", memStream);
            else
                Xamarin.Forms.DependencyService.Get<ISave>().Save(FileName, "application/msword", memStream);
        }
        #endregion
    }
    /// <summary>
    /// Specifies the customer details
    /// </summary>
    public class Customer
    {
        #region fields
        string m_customerID;
        string m_companyName;
        string m_contactName;
        string m_contactTitle;
        string m_address;
        string m_city;
        string m_postalCode;
        string m_country;
        string m_phone;
        string m_fax;
        #endregion

        #region properties
        public string CustomerID
        {
            get
            {
                return m_customerID;
            }
            set
            {
                m_customerID = value;
            }
        }
        public string CompanyName
        {
            get
            {
                return m_companyName;
            }
            set
            {
                m_companyName = value;
            }
        }
        public string ContactName
        {
            get
            {
                return m_contactName;
            }
            set
            {
                m_contactName = value;
            }
        }
        public string ContactTitle
        {
            get
            {
                return m_contactTitle;
            }
            set
            {
                m_contactTitle = value;
            }
        }
        public string Address
        {
            get
            {
                return m_address;
            }
            set
            {
                m_address = value;
            }
        }
        public string City
        {
            get
            {
                return m_city;
            }
            set
            {
                m_city = value;
            }
        }
        public string PostalCode
        {
            get
            {
                return m_postalCode;
            }
            set
            {
                m_postalCode = value;
            }
        }
        public string Country
        {
            get
            {
                return m_country;
            }
            set
            {
                m_country = value;
            }
        }
        public string Phone
        {
            get
            {
                return m_phone;
            }
            set
            {
                m_phone = value;
            }
        }
        public string Fax
        {
            get
            {
                return m_fax;
            }
            set
            {
                m_fax = value;
            }
        }
        #endregion

        #region constructor
        public Customer()
        { }

        public Customer(string customerID, string companyName, string contactName, string contactTitle, string address, string city, string postalCode, string country, string phone, string fax)
        {
            m_customerID = customerID;
            m_companyName = companyName;
            m_contactName = contactName;
            m_contactTitle = contactTitle;
            m_address = address;
            m_city = city;
            m_postalCode = postalCode;
            m_country = country;
            m_phone = phone;
            m_fax = fax;
        }
        #endregion
    }
    #endregion
}
