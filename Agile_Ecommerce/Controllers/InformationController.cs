using Microsoft.AspNetCore.Mvc;
using Agile_Ecommerce.Models;
using global::ShoppingOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingOnline.Models;
namespace Agile_Ecommerce.Controllers
{
        [Authorize] // Require users to be logged in
        public class InformationController : Controller
        {
            private UserManager<AppUserModel> _userManager;

            public InformationController(UserManager<AppUserModel> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IActionResult> Index()
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    UserModel userModel = new UserModel
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        // Add other user properties as needed
                    };

                    return View(userModel);
                }

                return RedirectToAction("Login", "Account"); // Redirect to login if user not found
            }
        }
    }

