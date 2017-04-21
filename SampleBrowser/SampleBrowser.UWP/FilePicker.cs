using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleBrowser;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SampleBrowser.UWP.FilePicker))]
namespace SampleBrowser.UWP
{
    class FilePicker : IFilePicker
    {
        public async Task<InputFileData> PickFile(string fileTypeInName)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            switch (fileTypeInName)
            {
                case "excel":
                    picker.FileTypeFilter.Add(".xls");
                    picker.FileTypeFilter.Add(".xlsx");
                    break;
                case "word":
                    picker.FileTypeFilter.Add(".doc");
                    picker.FileTypeFilter.Add(".docx");
                    break;
                case "presentation":
                    picker.FileTypeFilter.Add(".pptx");
                    break;
            }

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                var x = await file.OpenReadAsync();
                return new InputFileData(file.Path, file.Name, () => x.AsStreamForRead());
            }

            return null;
        }

    }
}
