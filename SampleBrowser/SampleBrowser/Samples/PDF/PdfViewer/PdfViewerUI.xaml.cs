using Syncfusion.SfPdfViewer.XForms;
using System;
using System.Globalization;
using System.IO;

using Xamarin.Forms;

namespace SampleBrowser
{
    public partial class PdfViewerUI : ContentPage
    {
        internal Stream PdfDocumentStream
        {
            get;
            set;
        }
        public PdfViewerUI()
        {
            InitializeComponent();
            pdfViewerControl.BindingContext = new PdfViewerViewModel(PdfDocumentStream);
            backIcon.Source = ImageSource.FromFile("Icons/back.png");
            headerStack.BackgroundColor = Color.FromHex("#FF1196CD");
            var backtIconTapped = new TapGestureRecognizer();
            backtIconTapped.Tapped += BacktIconTapped_Tapped;
            backIcon.GestureRecognizers.Add(backtIconTapped);
            pageNumberEntry.Completed += pageNumberEntry_Completed;
            pdfViewerControl.PageChanged += PdfViewerControl_PageChanged;
            goToNextButton.Clicked += goToNextButton_Clicked;
            goToPreviousButton.Clicked += goToPreviousButton_Clicked;
            pdfViewerControl.DocumentLoaded += PdfViewerControl_DocumentLoaded;
            pageNumberEntry.TextChanged += PageNumberEntry_TextChanged;
            pageNumberEntry.Focused += PageNumberEntry_Focused;
            pageNumberEntry.Text = "1";
            if (Device.Idiom != TargetIdiom.Phone && Device.OS == TargetPlatform.Windows)
            {
                headerStack.IsVisible = true;
                headerStack.HeightRequest = 100;
            }
            else
            {
                headerStack.IsVisible = false;
                headerStack.HeightRequest = 0;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pdfViewerControl.LoadDocument(PdfDocumentStream);
            //pdfViewerControl.BindingContext = new PdfViewerViewModel(PdfDocumentStream);
        }
        private void BacktIconTapped_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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
