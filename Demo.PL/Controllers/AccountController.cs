using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Twilio.Types;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMailService _mailService;
		private readonly ISmsService _smsService;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager ,IMailService mailService ,ISmsService smsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_mailService = mailService;
			_smsService = smsService;
		}
        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };
                var result = await _userManager.CreateAsync(User, model.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password is Error");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User Not exist");
                }
            }
            return View();
        }
        #endregion
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }


		//[HttpPost]
		//public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        var User = await _userManager.FindByEmailAsync(model.Email);
		//        if (model is not null)
		//        {
		//            var token =await _userManager.GeneratePasswordResetTokenAsync(User);
		//            var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);
		//            var email = new Email()
		//            {
		//                Subject = "Reset Password",
		//                To = model.Email,
		//                Body = ResetPasswordLink
		//            };

		//          //  EmailSettings.SendEmail(email);
		//          _mailService.SendEmail(email);
		//            return RedirectToAction(nameof(CheckYourInbox));
		//        }
		//        else
		//        {
		//            ModelState.AddModelError(string.Empty, "Email Not Exisit");
		//        }
		//    }
		//      return View("ForgetPassword", model);
		//} 
		//public IActionResult CheckYourInbox()
		//{
		//    return View();
		//}
		//public IActionResult ResetPassword(string email , string token)
		//{
		//    TempData["email"] = email;
		//    TempData["token"] = token;
		//    return View();
		//}
		//[HttpPost]
		//public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        string email = TempData["email"] as string; 
		//        string token = TempData["token"]  as string ;
		//        var User =await _userManager.FindByEmailAsync(email);
		//       var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
		//        if (Result.Succeeded)
		//            return RedirectToAction(nameof(Login)); 
		//        else
		//            foreach(var error in Result.Errors)
		//                ModelState.AddModelError(string.Empty, error.Description);
		//    }
		//    return View(model);
		//}



		//[HttpPost]
		//public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        var User = await _userManager.FindByEmailAsync(model.Email);
		//        if (model is not null)
		//        {
		//            var token =await _userManager.GeneratePasswordResetTokenAsync(User);
		//            var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);
		//            var email = new Email()
		//            {
		//                Subject = "Reset Password",
		//                To = model.Email,
		//                Body = ResetPasswordLink
		//            };

		//          //  EmailSettings.SendEmail(email);
		//          _mailService.SendEmail(email);
		//            return RedirectToAction(nameof(CheckYourInbox));
		//        }
		//        else
		//        {
		//            ModelState.AddModelError(string.Empty, "Email Not Exisit");
		//        }
		//    }
		//      return View("ForgetPassword", model);
		//} 
		//public IActionResult CheckYourInbox()
		//{
		//    return View();
		//}
		//public IActionResult ResetPassword(string email , string token)
		//{
		//    TempData["email"] = email;
		//    TempData["token"] = token;
		//    return View();
		//}
		//[HttpPost]
		//public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        string email = TempData["email"] as string; 
		//        string token = TempData["token"]  as string ;
		//        var User =await _userManager.FindByEmailAsync(email);
		//       var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
		//        if (Result.Succeeded)
		//            return RedirectToAction(nameof(Login)); 
		//        else
		//            foreach(var error in Result.Errors)
		//                ModelState.AddModelError(string.Empty, error.Description);
		//    }
		//    return View(model);
		//}


		
		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (model is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(User);
					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);
					var email = new Email()
					{
						Subject = "Reset Password",
						To = model.Email,
						Body = ResetPasswordLink
					};

					//  EmailSettings.SendEmail(email);
					_mailService.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Not Exist");
				}
			}
			return View("ForgetPassword", model);
		}
        
        #region SMS MESSAGE
        [HttpPost]
		public async Task<IActionResult> SendSms(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (model is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(User);
					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);
					var SmsMsg = new SmsMessage
					{
						Body = ResetPasswordLink,
						phoneNumber = User.PhoneNumber
					};
					_smsService.SendSms(SmsMsg);
					//  EmailSettings.SendEmail(email);
					//_mailService.SendEmail(email);
					//return RedirectToAction(nameof(CheckYourInbox));
					return Ok("Check Your Phone ");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Not Exisit");
				}
			}
			return View("ForgetPassword", model);
		}
		#endregion
		public IActionResult CheckYourInbox()
		{
			return View();
		}
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var User = await _userManager.FindByEmailAsync(email);
				var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
				if (Result.Succeeded)
					return RedirectToAction(nameof(Login));
				else
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}

	

	}
}
