using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Agile_Ecommerce.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Details(int Id)
		{
			if (Id == null) return RedirectToAction("Details");
			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			return View(productsById);
		}
	}
}
