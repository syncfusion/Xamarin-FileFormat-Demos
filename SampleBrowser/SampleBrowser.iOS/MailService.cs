using System;
using System.IO;
using Foundation;
using MessageUI;
using SampleBrowser;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SampleBrowser_Forms.iOS.MailService))]
namespace SampleBrowser_Forms.iOS
{
	public class MailService : IMailService
	{
		public MailService()
		{
		}

		public void ComposeMail(string fileName, string[] recipients, string subject, string messagebody, MemoryStream stream)
		{
			if (MFMailComposeViewController.CanSendMail)
			{

				var mailer = new MFMailComposeViewController();

				mailer.SetMessageBody(messagebody ?? string.Empty, false);
				mailer.SetSubject(subject ?? subject);
				mailer.Finished += (s, e) => ((MFMailComposeViewController)s).DismissViewController(true, () => { });


				string exception = string.Empty;
				string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string filePath = Path.Combine(path, fileName);
				try
				{
					FileStream fileStream = File.Open(filePath, FileMode.Create);
					stream.Position = 0;
					stream.CopyTo(fileStream);
					fileStream.Flush();
					fileStream.Close();
				}
				catch (Exception e)
				{
					exception = e.ToString();
				}
				finally
				{
				}


				mailer.AddAttachmentData(NSData.FromFile(filePath), GetMimeType(fileName), Path.GetFileName(fileName));



				UIViewController vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
				while (vc.PresentedViewController != null)
				{
					vc = vc.PresentedViewController;
				}
				vc.PresentViewController(mailer, true, null);
			}
		}

	private string GetMimeType(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return null;
			}

			var extension = Path.GetExtension(filename.ToLowerInvariant());

			switch (extension)
			{
				case "png":
					return "image/png";
				case "doc":
					return "application/msword";
				case "pdf":
					return "application/pdf";
				case "jpeg":
				case "jpg":
					return "image/jpeg";
				case "zip":
				case "docx":
				case "xlsx":
				case "pptx":
					return "application/zip";
				case "htm":
				case "html":
					return "text/html";
			}

			return "application/octet-stream";
		}

	}
}
