using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using Java.IO;

[assembly: Dependency(typeof(SampleBrowser.Droid.MailService))]
namespace SampleBrowser.Droid
{
	public class MailService : IMailService
	{
		public MailService()
		{
		}

		public void ComposeMail(string fileName, string[] recipients, string subject, string messagebody, MemoryStream filestream)
		{
			string exception = string.Empty;
			string root = null;
			if (Android.OS.Environment.IsExternalStorageEmulated)
			{
				root = Android.OS.Environment.ExternalStorageDirectory.ToString();
			}
			else
				root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

			Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion");
			myDir.Mkdir();

			Java.IO.File file = new Java.IO.File(myDir, fileName);

			if (file.Exists()) file.Delete();

			try
			{
				FileOutputStream outs = new FileOutputStream(file);
				outs.Write(filestream.ToArray());

				outs.Flush();
				outs.Close();
			}
			catch (Exception e)
			{
				exception = e.ToString();
			}

			Intent email = new Intent(Android.Content.Intent.ActionSend);
			var uri = Android.Net.Uri.Parse("file:///"+file.AbsolutePath);

			//file.SetReadable(true, true);
			email.PutExtra(Android.Content.Intent.ExtraSubject, subject);
			email.PutExtra(Intent.ExtraStream, uri);
			email.SetType("application/pdf");

			MainActivity.MainPageActivity.StartActivity(email);
		}
	}
}
