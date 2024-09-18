using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Agile_Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataConext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _dataConext = context;
        }

        public IActionResult Index()
        {
            var products = _dataConext.Products.Include("Category").Include("Brand").ToList();
            var slider = _dataConext.Slider.Where(s=>s.Status == 1 ).ToList();
            ViewBag.Slider = slider;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
			if (statuscode == 404)
			{
				return View("NotFound");
			}
			else
			{
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}
    }
}
