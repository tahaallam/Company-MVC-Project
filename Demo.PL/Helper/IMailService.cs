using Demo.DAL.Models;

namespace Demo.PL.Helper
{
	public interface IMailService
	{
		public void SendEmail(Email email);
	}
}
