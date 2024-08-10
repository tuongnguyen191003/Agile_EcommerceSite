using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brands = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brands == null) return RedirectToAction("Index");
			var productsByBrand = _dataContext.Products.Where(p => p.BrandId == brands.Id);
			return View(await productsByBrand.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
