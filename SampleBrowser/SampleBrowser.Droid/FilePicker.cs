using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using SampleBrowser;

[assembly: Dependency(typeof(SampleBrowser.Droid.FilePicker))]
namespace SampleBrowser.Droid
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    ///
    [Preserve(AllMembers = true)]
    public class FilePicker : IFilePicker
    {
        private readonly Context _context;
        private int _requestId;
        private TaskCompletionSource<InputFileData> _completionSource;

        public FilePicker()
        {
            _context = Android.App.Application.Context;
        }

        public async Task<InputFileData> PickFile(string fileTypeInName)
        {
            string type = "file/*";
            switch (fileTypeInName)
            {
                case "excel":
                    type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "word":
                    type = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(".doc");
                    type = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(".docx");
                    break;
                case "presentation":
                    type = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(".pptx");                
                    break;
            }
            var media = await TakeMediaAsync(type, Intent.ActionGetContent);

            return media;
        }

        private Task<InputFileData> TakeMediaAsync(string type, string action)
        {
            var id = GetRequestId();

            var ntcs = new TaskCompletionSource<InputFileData>(id);

            if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at a time");

            try
            {
                var pickerIntent = new Intent(this._context, typeof(FilePickerActivity));
                FilePickerActivity.mimeType = type;
                pickerIntent.SetFlags(ActivityFlags.NewTask);

                this._context.StartActivity(pickerIntent);

                EventHandler<FilePickerEventArgs> handler = null;
                EventHandler<EventArgs> cancelledHandler = null;

                handler = (s, e) =>
                {
                    var tcs = Interlocked.Exchange(ref _completionSource, null);

                    FilePickerActivity.FilePicked -= handler;

                    tcs?.SetResult(new InputFileData(e.FilePath, e.FileName, () => System.IO.File.OpenRead(e.FilePath)));
                };

                cancelledHandler = (s, e) =>
                {
                    var tcs = Interlocked.Exchange(ref _completionSource, null);

                    FilePickerActivity.FilePickCancelled -= cancelledHandler;

                    tcs?.SetResult(null);
                };

                FilePickerActivity.FilePickCancelled += cancelledHandler;
                FilePickerActivity.FilePicked += handler;
            }
            catch (Exception exAct)
            {
                System.Diagnostics.Debug.Write(exAct);
            }

            return _completionSource.Task;
        }
        private int GetRequestId()
        {
            int id = _requestId;

            if (_requestId == int.MaxValue)
                _requestId = 0;
            else
                _requestId++;

            return id;
        }

    }
}