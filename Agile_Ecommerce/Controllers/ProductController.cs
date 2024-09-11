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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RateProduct(int id, [Bind("Rating, Content")] RatingModel ratingModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var product = await _dataContext.Products.FindAsync(id);

                // Check if the product was found
                if (product == null)
                {
                    return NotFound();
                }

                var rating = new RatingModel
                {
                    ProductId = id,
                    UserId = userId,
                    Rating = ratingModel.Rating,
                    Content = ratingModel.Content ,// The comment is now stored in Content
                    CreatedDate = DateTime.Now
                };

                product.Ratings.Add(rating);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return View("Details", await _dataContext.Products.FindAsync(id));
            }
        }
    }
}
    