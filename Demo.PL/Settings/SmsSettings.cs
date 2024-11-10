using Org.BouncyCastle.Asn1.Crmf;

namespace Demo.PL.Settings
{
    public class SmsSettings
    {
        public string AccountSID { get; set; }
        public string AuthToken { get; set; }
        public string TwilioPhoneNumber { get; set; }
    }
}
