using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Controllers
{
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index( string Slug = "")
		{
			CategoryModel catgory = _dataContext.Categories.Where(c =>  c.Slug == Slug).FirstOrDefault();
			if (catgory == null) return RedirectToAction("Index");
			var productsByCategory = _dataContext.Products.Where(p => p.CategoryId == catgory.Id);
			return View( await productsByCategory.OrderByDescending(c => c.Id).ToListAsync());
		}
	}
}
