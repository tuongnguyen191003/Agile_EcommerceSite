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
			//if (ModelState.IsValid)
			//{
			//	Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
			//	if (result.Succeeded)
			//	{
			//		if (Url.IsLocalUrl(loginVM.ReturnUrl))
			//		{
			//			return Redirect(loginVM.ReturnUrl);
			//		}
			//		else
			//		{
			//			return RedirectToAction("Index", "Home");
			//		}
			//	}
			//	ModelState.AddModelError("", "Invalid Username or Password");
			//}

			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					// Lấy thông tin user sau khi đăng nhập
					var user = await _userManage.FindByNameAsync(loginVM.Username);

					// Kiểm tra RoleId
					if (user != null && user.RoleId == "Admin")
					{
						TempData["success"] = "Login Successfully, Wellcome Back!";
						// Nếu RoleId là "Admin", chuyển đến trang Admin/Product/Index
						return RedirectToAction("Index", "Product", new { area = "Admin" });
					}
					else
					{
						// Nếu RoleId không phải "Admin", chuyển đến trang Home/Index
						if (Url.IsLocalUrl(loginVM.ReturnUrl))
						{
							TempData["success"] = "Login Successfully, Wellcome Back!";
							return Redirect(loginVM.ReturnUrl);
						}
						else
						{
							TempData["success"] = "Login Successfully, Wellcome Back!";
							return RedirectToAction("Index", "Home");
						}
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
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email, PhoneNumber = user.PhoneNumber };
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

				// Kiểm tra xác nhận mật khẩu
				if (user.Password != user.ConfirmPassword)
				{
					ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
					return View(user);
				}

			}

			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl = "")
		{
			TempData["success"] = "See you again!";
			await _signInManager.SignOutAsync();
			
			return RedirectToAction("Index", "Home");
		}
	}
}
