using Demo.DAL.Models;
using Demo.PL.Settings;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System.Net;
//using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
namespace Demo.PL.Helper
{
	public class EmailSettings : IMailService
	{
		private MailSettings _options;

		public EmailSettings(IOptions<MailSettings> options)
        {
			_options = options.Value;
		}
        public void SendEmail(Email email)
		{
			var mail = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_options.Email) ,
				Subject = email.Subject 
			};
			mail.To.Add(MailboxAddress.Parse(email.To));
			mail.From.Add(new MailboxAddress(_options.DispalyName, _options.Email));
			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			smtp.Connect(_options.Host ,_options.Port ,MailKit.Security.SecureSocketOptions.StartTls);
			smtp.Authenticate(_options.Email , _options.Password);
			smtp.Send(mail);
			smtp.Disconnect(true);


		}







		#region smtp way to send Message
		//public static void SendEmail(Email email)
		//{
		//	var Client = new SmtpClient("smtp.gmail.com" , 587);
		//	Client.EnableSsl = true;
		//	Client.Credentials = new NetworkCredential("tahas224@gmail.com", "debtzamxxriztkxr");
		//	Client.Send("tahas224@gmail.com" , email.To ,email.Subject , email.Body );
		//} 
		#endregion
	}
}
