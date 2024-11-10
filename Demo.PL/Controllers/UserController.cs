using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager  , IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string seerchValue)
		{
			if (string.IsNullOrEmpty(seerchValue))
			{
				var User =await _userManager.Users.Select(U => new UserViewModel()
				{
					Id= U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNUmber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result 
				}).ToListAsync();
			return View(User);
			}
			else
			{
				var User =await _userManager.FindByEmailAsync(seerchValue);
				var MappedUser = new UserViewModel() 
				{
					Id = User.Id,
					FName = User.FName ,
					LName = User.LName,
					Email = User.Email ,
					PhoneNUmber = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result	
				};
				return View(new List<UserViewModel> { MappedUser});
			}
			
		}
		public async Task<IActionResult> Details(string id , string ViewName = "Details")
		{
			if (id is  null)
				return BadRequest();
			var User =await _userManager.FindByIdAsync(id);
			if (User is null )
				return NotFound();
			var MappedUser = _mapper.Map<UserViewModel>(User);
			return View(ViewName,MappedUser);
		}
		public async Task<IActionResult> Edit(string id )
		{
			return await Details(id, "Edit");
		}
		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> Edit(UserViewModel model ,[FromRoute] string? id)
		{
			if (id != model.Id) return BadRequest();
			if (model is null)
			{
				return BadRequest();
			}
			if (ModelState.IsValid)
			{
				try
				{
					var User =await _userManager.FindByIdAsync(id);
					User.PhoneNumber = model.PhoneNUmber;
					User.FName = model.FName;
					User.LName = model.LName;
				
		     	await  _userManager.UpdateAsync(User);
				return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{

					ModelState.AddModelError(string.Empty, ex.Message);
				}		
			}
			return View(model);	
		}
		public async Task<IActionResult> Delete(string id) 
		{
			return await Details(id, "Delete");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(UserViewModel model, string? id)
		{
			if (id != model.Id)
			{
				return BadRequest();
			}
			if (model is null) return NotFound();
			var User =await _userManager.FindByIdAsync(id);
			await _userManager.DeleteAsync(User);
			return RedirectToAction("Index");
		}
        #region fast Delete 
        //public async Task<IActionResult> Delete(string? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    var User = await _userManager.FindByIdAsync(id);
        //    await _userManager.DeleteAsync(User);
        //    return RedirectToAction("Index");
        //}
        #endregion

		
    }
}
