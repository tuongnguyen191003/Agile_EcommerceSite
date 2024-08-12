using Agile_Ecommerce.Models;
using Agile_Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingOnline.Models;

namespace ShoppingOnline.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManage;
		private SignInManager<AppUserModel> _signInManager;
		public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userMange)
		{
			_signInManager = signInManager;
			_userManage = userMange;
		}
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl});
		}
		[HttpPost]
		//public async Task<IActionResult> Login(LoginViewModel loginVM)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
		//		if (result.Succeeded)
		//		{
		//			return RedirectToAction(loginVM.ReturnUrl ?? "");
		//		}
		//		ModelState.AddModelError("", "Invalid Username or Password");
		//	}
		//	return View(loginVM);	
		//}

		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					if (Url.IsLocalUrl(loginVM.ReturnUrl))
					{
						return Redirect(loginVM.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				ModelState.AddModelError("", "Invalid Username or Password");
			}
			return View(loginVM);
		}


		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserModel user)
		{
			if(ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email };
				IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);
				if (result.Succeeded)
				{
					TempData["success"] = "Đăng ký tài khoản thành công!";
					return Redirect("/account/login");
				}
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}

			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl = "")
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
