using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]// Khai báo Area cho controller
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
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var rating = await _dataContext.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            return View(rating);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _dataContext.Ratings.FindAsync(id);
            if (rating != null)
            {
                _dataContext.Ratings.Remove(rating);
                await _dataContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
    