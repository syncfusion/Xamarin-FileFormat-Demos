using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Xamarin.Forms;

namespace SampleBrowser
{
    public class ControlListViewModel : BindableObject
    {
        public List<MasterSample> MasterSampleLists { get; set; }

        public ControlListViewModel()
        {
            MasterSampleLists = new List<MasterSample>();
            PopulateSamplesList();
        }

        private void PopulateSamplesList()
        {
            var assembly = typeof (App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("SampleBrowser.SampleList.SampleList.xml");
            bool isSkipGroup = false;
            using (var reader = new StreamReader(stream))
            {
                var xmlReader = XmlReader.Create(reader);
                xmlReader.Read();

                while (!xmlReader.EOF)
                {
                    if (xmlReader.Name == "Group" && xmlReader.IsStartElement())
                    {
                        var masterSampleList = new MasterSample
                        {
                            Title = GetDataFromXmlReader(xmlReader, "Title"),
                            ImageID = GetDataFromXmlReader(xmlReader, "ImageId"),
                        };

						if (Device.OS == TargetPlatform.iOS && masterSampleList.Title == "PDFViewer")
						{
							double IOS_Version = DependencyService.Get<IIOSVersionDependencyService>().GetIOSVersion();

							if (IOS_Version < 9.0)
							{
								isSkipGroup = true;
								continue;
							}
						}

                        if (Device.OS == TargetPlatform.Android && masterSampleList.Title == "PDFViewer")
                        {
                            int androidVersion = DependencyService.Get<IAndroidVersionDependencyService>().GetAndroidVersion();

                            if (androidVersion < 21)
                            {
                                isSkipGroup = true;
                                continue;
                            }
                        }
                        
                        isSkipGroup = false;
                        bool isPreview;
                        bool.TryParse(GetDataFromXmlReader(xmlReader, "IsPreview"), out isPreview);
                        if (isPreview)
                            masterSampleList.Type = "Preview";

                        if (!isPreview)
                        {
                            bool isNew;
                            bool.TryParse(GetDataFromXmlReader(xmlReader, "IsNew"), out isNew);
                            if (isNew)
                                masterSampleList.Type = "New";
                        }

                        MasterSampleLists.Add(masterSampleList);
                    }
                    else if (xmlReader.Name == "Sample" && xmlReader.IsStartElement())
                    {
                        var sampleList = MasterSampleLists[MasterSampleLists.Count - 1];

                        var sampleDetails = new SampleDetails
                        {
                            Title = GetDataFromXmlReader(xmlReader, "Title"),
                            ImageId = GetDataFromXmlReader(xmlReader, "ImageID")
                        };

                        if (isSkipGroup)
                            continue;
						 // iOS Word viewer doesn't preserves the Charts in the document, So hide the Chart samples from the Xamarin.Forms.iOS samplebrowser
                        if (App.Platform == Platforms.iOS && sampleList.Title == "DocIO" && (sampleDetails.Title == "Bar Chart" || sampleDetails.Title == "Pie Chart"))
                            continue;
                        xmlReader.MoveToAttribute("Type");
                        sampleDetails.Type = xmlReader.Value;

                        bool isUpdated;
                        bool.TryParse(GetDataFromXmlReader(xmlReader, "IsUpdated"), out isUpdated);
                        if (isUpdated)
                            sampleDetails.SampleType = "Updated";

                        var isNew = false;
                        if (!isUpdated)
                        {
                            bool.TryParse(GetDataFromXmlReader(xmlReader, "IsNew"), out isNew);
                            if (isNew)
                                sampleDetails.SampleType = "New";
                        }

                        if (sampleList.Type == null && (isUpdated || isNew))
                        {
                            sampleList.Type = "Updated";
                        }

                        sampleList.Samples.Add(sampleDetails);
                    }
                    xmlReader.Read();
                }
            }
        }

        private static string GetDataFromXmlReader(XmlReader reader, string attribute)
        {
            reader.MoveToAttribute(attribute);
            return reader.Value;
        }
    }
}