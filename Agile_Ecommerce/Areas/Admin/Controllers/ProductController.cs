using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}

		//[Route("Index")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products
				.OrderByDescending(p => p.Id)
				.Include(p => p.Category)
				.Include(p => p.Brand)
				.ToListAsync());
		}
		[Route("Create")]

		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
			return View();
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

			if (ModelState.IsValid)
			{
				//code thêm dữ liệu
				product.Slug = product.Name.Replace(" ", "-");
				product.Description = product.Description.Replace("<p>", "").Replace("</p>", "").Replace("<br>", "\n");
				product.Price = decimal.Parse(product.Price.ToString().Replace(",", "."));
				var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
				if (slug != null)
				{
					ModelState.AddModelError("", "Product exists already");
					return View(product);
				}
				if (product.ImageUpload != null)
				{
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
					string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imageName);

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					product.Image = imageName;
				}

				_dataContext.Add(product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Add successfully";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model đang có một vài vấn đề";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}


			return View(product);
		}
        [Route("Edit/{Id}")]
        public async Task<IActionResult> Edit(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
			return View(product);
		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{Id}")]
		public async Task<IActionResult> Edit(int Id, ProductModel product)
		{

			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            var existed_product = await _dataContext.Products.FindAsync(Id);

            if (ModelState.IsValid)
			{
				existed_product.Slug = existed_product.Name.Replace(" ", "-");
				existed_product.Description = existed_product.Description.Replace("<p>", "").Replace("</p>", "").Replace("<br>", "\n");
				if (existed_product.ImageUpload != null)
				{

					//upload new image
					string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
					string imageName = Guid.NewGuid().ToString() + "_" + existed_product.ImageUpload.FileName;
					string filePath = Path.Combine(uploadDir, imageName);

					//delete old picture
					string oldFileImage = Path.Combine(uploadDir, existed_product.Image);

					try
					{
						if (System.IO.File.Exists(oldFileImage))
						{
							System.IO.File.Delete(oldFileImage);
						}
					}
					catch (Exception ex)
					{
						ModelState.AddModelError("", "An error occured while deleting the product image");
					}

					FileStream fs = new FileStream(filePath, FileMode.Create);
					await product.ImageUpload.CopyToAsync(fs);
					fs.Close();
					existed_product.Image = imageName;
				}

				existed_product.Name = product.Name;
				existed_product.Description = product.Description;
				existed_product.Price = product.Price;
				existed_product.CategoryId = product.CategoryId;
				existed_product.BrandId = product.BrandId;

				_dataContext.Update(existed_product);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Updated successfully";
				return RedirectToAction("Index");
			}
			else
			{
				TempData["error"] = "Model đang có một vài vấn đề";
				List<string> errors = new List<string>();
				foreach (var value in ModelState.Values)
				{
					foreach (var error in value.Errors)
					{
						errors.Add(error.ErrorMessage);
					}
				}
				string errorMessage = string.Join("\n", errors);
				return BadRequest(errorMessage);
			}


			return View(product);
		}
		[Route("Delete")]
		public async Task<IActionResult> Delete(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			if (product == null)
			{
				return NotFound();
			}
			string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
			string oldFileImage = Path.Combine(uploadDir, product.Image);

			try
			{
				if (System.IO.File.Exists(oldFileImage))
				{
					System.IO.File.Delete(oldFileImage);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "An error occured while deleting the product image");
			}


			_dataContext.Products.Remove(product);
			await _dataContext.SaveChangesAsync();
			TempData["error"] = "Deleted successfully";
			return RedirectToAction("Index");
		}
	}
}
