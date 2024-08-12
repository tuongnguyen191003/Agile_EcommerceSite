using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agile_Ecommerce.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataConext;
		public CheckoutController(DataContext context)
		{
			_dataConext = context;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if (userEmail == null)
			{
				return RedirectToAction("Login", "Account");
			}
			else
			{
				var orderCode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.OrderCode = orderCode;
				orderItem.UserName = userEmail;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataConext.Add(orderItem);
				_dataConext.SaveChanges();
				//TempData["success"] = "Order created successfully!";
				//return RedirectToAction("Index", "Cart");

				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orderDetails = new OrderDetails();
					orderDetails.UserName = userEmail;
					orderDetails.OrderCode = orderCode;
					orderDetails.ProductId = cart.ProductId;
					orderDetails.Price = cart.Price;
					orderDetails.Quantity = cart.Quantity;
					_dataConext.Add(orderDetails);
					_dataConext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Order created successfully!";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}
	}
}
