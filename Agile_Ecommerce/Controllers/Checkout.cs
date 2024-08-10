using Microsoft.AspNetCore.Mvc;

namespace Agile_Ecommerce.Controllers
{
	public class Checkout : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
