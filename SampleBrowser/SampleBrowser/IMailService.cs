using System;
using System.IO;
namespace SampleBrowser
{
	public interface IMailService
	{
		void ComposeMail(string fileName,string[] recipients, string subject, string messagebody, MemoryStream documentStream);
	}
}
