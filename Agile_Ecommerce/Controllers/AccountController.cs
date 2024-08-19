using Agile_Ecommerce.Models;
using Agile_Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingOnline.Models;
using System.Net.Mail;
using System.Net;

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
                var user = await _userManage.FindByNameAsync(loginVM.Username);

                if (user != null)
                {
                    if (user.VerificationCode == loginVM.VerificationCode)
                    {
                        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                        if (result.Succeeded)
                        {
                            // Lấy thông tin user sau khi đăng nhập
                            // ... (rest of your login logic)
                            if (user != null && user.RoleId == "Admin")
                            {
                                TempData["success"] = "Login Successfully, Wellcome Back!";
                                return RedirectToAction("Index", "Product", new { area = "Admin" });
                            }
                            else
                            {
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
                        else
                        {
                            ModelState.AddModelError("", "Invalid Username or Password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid verification code.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
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
                user.VerificationCode = GenerateRandomCode(6);
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email, PhoneNumber = user.PhoneNumber };
                IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    newUser.VerificationCode = user.VerificationCode;
                    await SendConfirmationEmail(newUser);

                    // Save changes to the database
                    await _userManage.UpdateAsync(newUser);

                    TempData["success"] = "Đăng ký tài khoản thành công!";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(user);
                }
            }
            return View(user);
        }
        private string GenerateRandomCode(int length)
        {
            // Use Random class to generate a random string of digits
            Random random = new Random();
            string code = "";
            for (int i = 0; i < length; i++)
            {
                code += random.Next(0, 10).ToString();
            }
            return code;
        }
        private async Task SendConfirmationEmail(AppUserModel user)
        {
            // Configure your email settings here
            string fromEmail = "tai1672003@gmail.com"; // Replace with your email address
            string password = "gxii jvmp yiqq hhaa"; // Replace with your email password
            string subject = "Welcome to Our Website!";
			string body = $"Thank you for registering with us, {user.Email}! Please verify your account by entering the following code: {user.VerificationCode}";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(user.Email);
                mail.Subject = subject;
                mail.Body = body;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com")) // Replace with your SMTP server
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, password);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(mail);
                }
            }
        }
		[HttpGet]
		public IActionResult Verify(string email, string code)
		{
			// Find the user by email
			var user = _userManage.FindByEmailAsync(email).Result;

			if (user != null && user.VerificationCode == code)
			{
				// Verify the account
				user.VerificationCode = null; // Clear the code after verification
				_userManage.UpdateAsync(user).Wait();

				TempData["success"] = "Your account has been verified successfully!";
				return Redirect("/account/login");
			}
			else
			{
				TempData["error"] = "Invalid verification code.";
				return Redirect("/account/login");
			}
		}
public async Task<IActionResult> Logout(string returnUrl = "")
		{
			TempData["success"] = "See you again!";
			await _signInManager.SignOutAsync();
			
			return RedirectToAction("Index", "Home");
		}
	}
}
