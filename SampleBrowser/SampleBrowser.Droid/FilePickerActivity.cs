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
using Android.Provider;
using Android.Database;
using Java.IO;
using Android.Webkit;

namespace SampleBrowser.Droid
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    [Preserve(AllMembers = true)]
    public class FilePickerActivity : Activity
    {
        private Context context;
        internal static string mimeType;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            context = Application.Context;


            var intent = new Intent(Intent.ActionGetContent);

            intent.SetType(mimeType);

            intent.AddCategory(Intent.CategoryOpenable);
            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "Select file"), 0);
            }
            catch (Exception exAct)
            {
                System.Diagnostics.Debug.Write(exAct);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Canceled)
            {
                // Notify user file picking was cancelled.
                OnFilePickCancelled();
                Finish();
            }
            else
            {
                System.Diagnostics.Debug.Write(data.Data);
                try
                {
                    var _uri = data.Data;

                    var filePath = IOUtil.getPath(context, _uri);

                    if (string.IsNullOrEmpty(filePath))
                        filePath = _uri.Path;

                    var file = IOUtil.readFile(filePath);

                    var fileName = GetFileName(context, _uri);

                    OnFilePicked(new FilePickerEventArgs(file, fileName, filePath));
                }
                catch (Exception readEx)
                {
                    // Notify user file picking failed.
                    OnFilePickCancelled();
                    System.Diagnostics.Debug.Write(readEx);
                }
                finally
                {
                    Finish();
                }
            }
        }

        string GetFileName(Context ctx, Android.Net.Uri uri)
        {

            string[] projection = { MediaStore.MediaColumns.DisplayName };

            var cr = ctx.ContentResolver;
            var name = "";
            var metaCursor = cr.Query(uri, projection, null, null, null);

            if (metaCursor != null)
            {
                try
                {
                    if (metaCursor.MoveToFirst())
                    {
                        name = metaCursor.GetString(0);
                    }
                }
                finally
                {
                    metaCursor.Close();
                }
            }
            return name;
        }

        internal static event EventHandler<FilePickerEventArgs> FilePicked;
        internal static event EventHandler<EventArgs> FilePickCancelled;

        private static void OnFilePickCancelled()
        {
            FilePickCancelled?.Invoke(null, null);
        }

        private static void OnFilePicked(FilePickerEventArgs e)
        {
            var picked = FilePicked;

            if (picked != null)
                picked(null, e);
        }
    }

    public class IOUtil
    {

        public static string getPath(Context context, Android.Net.Uri uri)
        {
            bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            // DocumentProvider
            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                // ExternalStorageProvider
                if (isExternalStorageDocument(uri))
                {
                    var docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    var type = split[0];

                    if ("primary".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        return Android.OS.Environment.ExternalStorageDirectory + "/" + split[1];
                    }

                    // TODO handle non-primary volumes
                }
                // DownloadsProvider
                else if (isDownloadsDocument(uri))
                {

                    string id = DocumentsContract.GetDocumentId(uri);
                    Android.Net.Uri contentUri = ContentUris.WithAppendedId(
                            Android.Net.Uri.Parse("content://downloads/public_downloads"), long.Parse(id));

                    return getDataColumn(context, contentUri, null, null);
                }
                // MediaProvider
                else if (isMediaDocument(uri))
                {
                    var docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    var type = split[0];

                    Android.Net.Uri contentUri = null;
                    //if ("image".Equals(type))
                    //{
                    //    contentUri = MediaStore.Images.Media.ExternalContentUri;
                    //}
                    //else if ("video".Equals(type))
                    //{
                    //    contentUri = MediaStore.Video.Media.ExternalContentUri;
                    //}
                    //else if ("audio".Equals(type))
                    //{
                    //    contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    //}

                    var selection = "_id=?";
                    var selectionArgs = new string[] {
                        split[1]
                    };

                    return getDataColumn(context, contentUri, selection, selectionArgs);
                }
            }
            // MediaStore (and general)
            else if ("content".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return getDataColumn(context, uri, null, null);
            }
            // File
            else if ("file".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }

        public static string getDataColumn(Context context, Android.Net.Uri uri, string selection,
        string[] selectionArgs)
        {

            ICursor cursor = null;
            var column = "_data";
            string[] projection = {
                column
            };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs,
                        null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int column_index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(column_index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }
            return null;
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is ExternalStorageProvider.
         */
        public static bool isExternalStorageDocument(Android.Net.Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is DownloadsProvider.
         */
        public static bool isDownloadsDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is MediaProvider.
         */
        public static bool isMediaDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }

        public static byte[] readFile(string file)
        {
            try
            {
                return readFile(new File(file));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                return new byte[0];
            }
        }

        public static byte[] readFile(File file)
        {
            // Open file
            var f = new RandomAccessFile(file, "r");

            try
            {
                // Get and check length
                long longlength = f.Length();
                var length = (int)longlength;

                if (length != longlength)
                    throw new IOException("Filesize exceeds allowed size");
                // Read file and return data
                byte[] data = new byte[length];
                f.ReadFully(data);
                return data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                return new byte[0];
            }
            finally
            {
                f.Close();
            }
        }       
    }
}