using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")] // Khai báo Area cho controller
    public class RateController : Controller
    {
        private readonly DataContext _dataContext;

        public RateController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ratings = await _dataContext.Ratings.Include(r => r.Product).ToListAsync();
            return View(ratings);
        }
    }
}
    