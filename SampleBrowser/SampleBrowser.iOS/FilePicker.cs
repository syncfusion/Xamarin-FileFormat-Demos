using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SampleBrowser;
using Foundation;
using UIKit;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using MobileCoreServices;

[assembly: Dependency(typeof(SampleBrowser_Forms.iOS.FilePicker))]
namespace SampleBrowser_Forms.iOS
{
	public class FilePicker : IFilePicker
	{
		private static Lazy<CustomFilePicker> impl = new Lazy<CustomFilePicker>(() => new CustomFilePicker(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
		public async Task<InputFileData> PickFile(string fileTypeInName)
		{
			var media = await impl.Value.PickFile(fileTypeInName);

			return media;
		}
	}
	
    /// <summary>
    /// Implementation for FilePicker
    /// </summary>
    public class CustomFilePicker : NSObject, IUIDocumentMenuDelegate
    {
        private int _requestId;
        private TaskCompletionSource<InputFileData> _completionSource;

        /// <summary>
        /// Event which is invoked when a file was picked
        /// </summary>
        public EventHandler<FilePickerEventArgs> Handler
        {
            get;
            set;
        }

        private void OnFilePicked(FilePickerEventArgs e)
        {
            Handler?.Invoke(null, e);
        }

        public void DidPickDocumentPicker(UIDocumentMenuViewController documentMenu, UIDocumentPickerViewController documentPicker)
        {
            documentPicker.DidPickDocument += DocumentPicker_DidPickDocument;
            documentPicker.WasCancelled += DocumentPicker_WasCancelled;

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(documentPicker, true, null);
        }

        private void DocumentPicker_DidPickDocument(object sender, UIDocumentPickedEventArgs e)
        {
            var securityEnabled = e.Url.StartAccessingSecurityScopedResource();
            var doc = new UIDocument(e.Url);
            var data = NSData.FromUrl(e.Url);
            var dataBytes = new byte[data.Length];

            System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));

            string filename = doc.LocalizedName;

            // iCloud drive can return null for LocalizedName.
            if (filename == null)
            {
                // Retrieve actual filename by taking the last entry after / in FileURL.
                // e.g. /path/to/file.ext -> file.ext

                // pathname is either a string or null.
                var pathname = doc.FileUrl?.ToString();

                // filesplit is either:
                // 0 (pathname is null, or last / is at position 0)
                // -1 (no / in pathname)
                // positive int (last occurence of / in string)
                var filesplit = pathname?.LastIndexOf('/') ?? 0;

                filename = pathname?.Substring(filesplit + 1);
            }

            OnFilePicked(new FilePickerEventArgs(dataBytes, filename));
        }

        /// <summary>
        /// Handles when the file picker was cancelled. Either in the
        /// popup menu or later on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DocumentPicker_WasCancelled(object sender, EventArgs e)
        {
            {
                var tcs = Interlocked.Exchange(ref _completionSource, null);
                tcs.SetResult(null);
            }
        }

        /// <summary>
        /// Lets the user pick a file with the systems default file picker
        /// For iOS iCloud drive needs to be configured
        /// </summary>
        /// <returns></returns>
        public async Task<InputFileData> PickFile(string fileTypeInName)
        {
            var media = await TakeMediaAsync(fileTypeInName);

            return media;
        }

        private Task<InputFileData> TakeMediaAsync(string fileTypeInName)
        {
            var id = GetRequestId();

            var ntcs = new TaskCompletionSource<InputFileData>(id);

            if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
                throw new InvalidOperationException("Only one operation can be active at a time");            
            string utType = "";
            switch (fileTypeInName)
            {
                case "excel":utType = "org.openxmlformats.spreadsheetml.sheet";
                    break;
                case "word":
                    utType = "org.openxmlformats.wordprocessingml.document";
                    break;
                case "presentation":
                    utType = "org.openxmlformats.presentationml.presentation";
                    break;
            }

            var allowedUtis = new string[] {
				utType
            };

            var importMenu =
                new UIDocumentMenuViewController(allowedUtis, UIDocumentPickerMode.Import)
                {
                    Delegate = this,
                    ModalPresentationStyle = UIModalPresentationStyle.Popover
                };

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(importMenu, true, null);

            var presPopover = importMenu.PopoverPresentationController;

            if (presPopover != null)
            {
                presPopover.SourceView = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
                presPopover.PermittedArrowDirections = UIPopoverArrowDirection.Down;
            }

            Handler = null;

            Handler = (s, e) => {
                var tcs = Interlocked.Exchange(ref _completionSource, null);

				tcs?.SetResult(new InputFileData(e.FilePath, e.FileName, e.FileByte));
            };

            return _completionSource.Task;
        }

        public void WasCancelled(UIDocumentMenuViewController documentMenu)
        {
            var tcs = Interlocked.Exchange(ref _completionSource, null);

            tcs?.SetResult(null);
        }

        private int GetRequestId()
        {
            var id = _requestId;

            if (_requestId == int.MaxValue)
                _requestId = 0;
            else
                _requestId++;

            return id;
        }
    }
}