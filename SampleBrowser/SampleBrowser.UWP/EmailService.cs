
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Windows.ApplicationModel.DataTransfer;
using SampleBrowser;
using Windows.ApplicationModel.Email;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SampleBrowser.UWP.MailService))]
namespace SampleBrowser.UWP
{
    public class MailService : IMailService
    {
       
       public async void ComposeMail(string fileName, string[] recipients, string subject, string messagebody, MemoryStream documentStream)
        { 
        var emailMessage = new EmailMessage
            {
                Subject = subject,
                Body = messagebody
        };
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile outFile = await local.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (Stream outStream = await outFile.OpenStreamForWriteAsync())
            {
                outStream.Write(documentStream.ToArray(), 0, (int)documentStream.Length);
            }
            emailMessage.Attachments.Add(new EmailAttachment(fileName, outFile));            

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
    }
}