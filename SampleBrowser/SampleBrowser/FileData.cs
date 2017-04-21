using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser
{

    public class InputFileData : IDisposable
    {
        private string _fileName;
        private string _filePath;
        private bool _isDisposed;
        private readonly Action<bool> _dispose;
        private readonly Func<Stream> _streamGetter;
		private readonly byte[] _array;

        public InputFileData()
        { }

        public InputFileData(string filePath, string fileName, Func<Stream> streamGetter, Action<bool> dispose = null)
        {
            _filePath = filePath;
            _fileName = fileName;
            _dispose = dispose;
            _streamGetter = streamGetter;
        }

		public InputFileData(string filePath, string fileName, byte[] array)
		{
			_filePath = filePath;
			_fileName = fileName;
			_array = array;
		}

        public byte[] DataArray
        {
            get
            {
				if (_streamGetter == null && _array!=null)
				{
					return _array;
				}
				else
				{
					using (var stream = GetStream())
					{
						var resultBytes = new byte[stream.Length];
						stream.Read(resultBytes, 0, (int)stream.Length);

						return resultBytes;
					}
				}
            }
        }

        /// <summary>
        /// Filename of the picked file
        /// </summary>
        public string FileName
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(null);

                return _fileName;
            }

            set
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(null);

                _fileName = value;
            }
        }

        /// <summary>
        /// Full filepath of the picked file
        /// </summary>
        public string FilePath
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(null);

                return _filePath;
            }

            set
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(null);

                _filePath = value;
            }
        }

        /// <summary>
        /// Get stream if available
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);

            return _streamGetter();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _dispose?.Invoke(disposing);
        }

        ~InputFileData()
        {
            Dispose(false);
        }
    }

    public class FilePickerEventArgs : EventArgs
    {
        public byte[] FileByte { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public FilePickerEventArgs()
        {

        }

        public FilePickerEventArgs(byte[] fileByte)
        {
            FileByte = fileByte;
        }

        public FilePickerEventArgs(byte[] fileByte, string fileName)
            : this(fileByte)
        {
            FileName = fileName;
        }

        public FilePickerEventArgs(byte[] fileByte, string fileName, string filePath)
            : this(fileByte, fileName)
        {
            FilePath = filePath;
        }
    }

    public interface IFilePicker
    {
        Task<InputFileData> PickFile(string fileTypeInName);
    }
}
