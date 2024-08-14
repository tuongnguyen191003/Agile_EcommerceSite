using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Agile_Ecommerce.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index(int pg = 1)
		{
			List<CategoryModel> category = _dataContext.Categories.ToList(); //33 datas


			const int pageSize = 10; //10 items/trang

			if (pg < 1) //page < 1;
			{
				pg = 1; //page ==1
			}
			int recsCount = category.Count(); //33 items;

			var pager = new Paginate(recsCount, pg, pageSize);

			int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

			//category.Skip(20).Take(10).ToList()

			var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

			ViewBag.Pager = pager;

			return View(data);
		}

		public async Task<IActionResult> Details(int Id)
		{
			if (Id == null) return RedirectToAction("Details");
			var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
			return View(productsById);
		}
		public async Task<IActionResult> Search(string searchTerm)
		{
			var products = await _dataContext.Products
				.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
				.ToListAsync();

			ViewBag.Keyword=searchTerm;
			return View(products);
		}
	}
}
