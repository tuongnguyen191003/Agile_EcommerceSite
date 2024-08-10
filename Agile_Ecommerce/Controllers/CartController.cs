using Microsoft.AspNetCore.Mvc;

namespace Agile_Ecommerce.Controllers
{
	public class CartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
