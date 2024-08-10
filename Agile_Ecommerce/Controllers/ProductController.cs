using Microsoft.AspNetCore.Mvc;

namespace Agile_Ecommerce.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details()
		{
			return View();
		}
	}
}
