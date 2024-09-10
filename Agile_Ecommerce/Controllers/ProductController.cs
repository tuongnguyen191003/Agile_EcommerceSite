using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Runtime.Intrinsics.X86;
using System;
using System.Security.Claims;

namespace Agile_Ecommerce.Controllers
{
	public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
        private UserManager<AppUserModel> _userManage;
        public ProductController(DataContext context, UserManager<AppUserModel> userMange)
		{
			_dataContext = context;
            _userManage = userMange;

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

            var product = await _dataContext.Products
                .Include(p => p.Ratings) // Eagerly load ratings
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (product == null) return NotFound();

            // Check if user is authenticated and has already rated the product
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hasRated = product.Ratings.Any(r => r.UserId == userId);

            ViewBag.HasRated = hasRated;

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Filter(decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            var products = _dataContext.Products.AsQueryable();

            // Filter by price range
            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            // Sorting
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    break;
            }

            var filteredProducts = await products.ToListAsync();

            return View("Search", filteredProducts);
        }

        // Action to handle rating submission
        [Authorize]
          [HttpPost]
        [ValidateAntiForgeryToken] // For preventing CSRF attacks
        public async Task<IActionResult> RateProduct(int id, [Bind("Stars")] RatingModel ratingModel)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return View("Details", await _dataContext.Products.FindAsync(id));
            }

            // Get the current user's ID (assuming you have authentication/authorization setup)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the product from the database
            var product = await _dataContext.Products.FindAsync(id);

            // Check if the product exists
            if (product != null)
            {
                // If Ratings is null, initialize it as a new list
                if (product.Ratings == null)
                {
                    product.Ratings = new List<RatingModel>(); 
                }

                // Create a new Rating entity
                var rating = new RatingModel
                {
                    ProductId = id,
                    UserId = userId,
                    Rating = ratingModel.Rating
                };

                // Add the rating to the product
                product.Ratings.Add(rating);

                // Save changes
                await _dataContext.SaveChangesAsync();

                // Redirect to the product details page
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                // Handle case where product is not found
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

	