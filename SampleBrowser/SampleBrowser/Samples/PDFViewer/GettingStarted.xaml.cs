#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.SfPdfViewer.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class PDFViewerGettingStarted : SamplePage
    {
        public PDFViewerGettingStarted()
        {
            InitializeComponent();
            //pdfViewerControl.BindingContext = new PDFViewerViewModel();
            pageNumberEntry.Completed += pageNumberEntry_Completed;
            pdfViewerControl.PageChanged += PdfViewerControl_PageChanged;
            goToNextButton.Clicked += goToNextButton_Clicked;
            goToPreviousButton.Clicked += goToPreviousButton_Clicked;
			pdfViewerControl.DocumentLoaded += PdfViewerControl_DocumentLoaded;
			pageNumberEntry.TextChanged += PageNumberEntry_TextChanged;
            pageNumberEntry.Focused += PageNumberEntry_Focused;
            if (Device.OS == TargetPlatform.Windows)
            {
                var fileStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDFViewer.Assets.GIS Succinctly.pdf");
                pdfViewerControl.LoadDocument(fileStream);
                pageNumberEntry.Text = "1";
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var fileStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("SampleBrowser.Samples.PDFViewer.Assets.GIS Succinctly.pdf");
            pdfViewerControl.LoadDocument(fileStream);
            
        }
        private void PdfViewerControl_PageChanged(object sender, PageChangedEventArgs e)
        {
            pageNumberEntry.Text = e.PageNumber.ToString();
        }

        private void PageNumberEntry_Focused(object sender, FocusEventArgs e)
        {
            pageNumberEntry.Text = "";
        }

		 private void PdfViewerControl_DocumentLoaded(object sender, EventArgs args)
        {
            pageCountLabel.Text = pdfViewerControl.PageCount.ToString();
			pageNumberEntry.Text = pdfViewerControl.PageNumber.ToString();
        }
		
        private void PageNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] pageNum = pageNumberEntry.Text.ToCharArray();
            int targetPagenumber = 0;
            bool gotoResult = int.TryParse(pageNumberEntry.Text, out targetPagenumber);

            if (!gotoResult)
            {
                foreach (char pageNo in pageNum)
                {
                    if (!char.IsNumber(pageNo))
                    {
                        pageNumberEntry.Text = "";
                    }
                }
            }

        }

        private void goToPreviousButton_Clicked(object sender, EventArgs e)
        {
            if (pdfViewerControl.PageNumber > 1)
                pdfViewerControl.GoToPreviousPage();
        }

        private void goToNextButton_Clicked(object sender, EventArgs e)
        {
            if (pdfViewerControl.PageNumber < pdfViewerControl.PageCount)
                pdfViewerControl.GoToNextPage();
        }

        private void pageNumberEntry_Completed(object sender, EventArgs e)
        {
            int pageNumber = 1;
            if (int.TryParse(((sender as Entry).Text), NumberStyles.Integer, CultureInfo.InvariantCulture, out pageNumber))
            {
                if ((sender as Entry) != null && pageNumber > 0 && pageNumber <= pdfViewerControl.PageCount)
                    pdfViewerControl.GoToPage(int.Parse((sender as Entry).Text));
                else
                {
                    DisplayAlert("Error", "Please enter the valid page number.", "OK");
                    (sender as Entry).Text = pdfViewerControl.PageNumber.ToString();
                }
            }
            else
            {
                DisplayAlert("Error", "Please enter the valid page number.", "OK");
                (sender as Entry).Text = pdfViewerControl.PageNumber.ToString();
            }
            pageNumberEntry.Unfocus();
        }
    }
}
