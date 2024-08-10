using Microsoft.AspNetCore.Mvc;

namespace Agile_Ecommerce.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
