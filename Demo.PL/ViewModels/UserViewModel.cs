namespace Demo.PL.ViewModels
{
	public class UserViewModel
	{
		public string Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string PhoneNUmber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
