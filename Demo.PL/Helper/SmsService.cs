using Demo.DAL.Models;
using Demo.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

namespace Demo.PL.Helper
{
	public class SmsService : ISmsService
	{
		private SmsSettings _options;
        public SmsService(IOptions<SmsSettings> options)
        {
            _options = options.Value;
        }
		public MessageResource SendSms(SmsMessage sms)
		{
			TwilioClient.Init(_options.AccountSID, _options.AuthToken);
			var Message = MessageResource.Create(
				body : sms.Body ,
				from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
				to : sms.phoneNumber
				);
			return Message;
		}
	}
}
