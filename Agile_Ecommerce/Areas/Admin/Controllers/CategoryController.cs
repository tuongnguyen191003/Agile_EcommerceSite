using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        
        [Route("Index")]
        public async Task<IActionResult> Index(int pg = 1)
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

        [HttpGet]
		[Route("Create")]
		public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(CategoryModel category)
        {
           
            if (ModelState.IsValid) //tình trạng model tốt
            {
                //code thêm dữ liệu
                category.Slug = category.Name.Replace(" ", "-");
                category.Description = category.Description.Replace("<p>", "").Replace("</p>", "").Replace("<br>", "\n");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Product exists already");
                    return View(category);
                }
                

                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add Category successfully";
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
        }

        [Route("Edit/{Id}")] // thêm Id vào route
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            if (category == null) // Kiểm tra xem category có tồn tại hay không
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{Id}")] // thêm Id vào route
        public async Task<IActionResult> Edit(int Id, CategoryModel category)
        {
            // Tìm category trong database dựa vào Id
            var existedCategory = await _dataContext.Categories.FindAsync(Id);

            if (existedCategory == null) // Kiểm tra xem category có tồn tại hay không
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy
            }

            // Kiểm tra model state
            if (ModelState.IsValid)
            {
                // Tạo slug mới từ name
                category.Slug = category.Name.Replace(" ", "-");

                // Kiểm tra slug có trùng với slug hiện tại hay không
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug && p.Id != Id);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Category exists already");
                    return View(existedCategory);
                }

                // Kiểm tra xem có thay đổi nào không
                if (existedCategory.Name == category.Name &&
                    existedCategory.Description == category.Description &&
                    existedCategory.Status == category.Status)
                {
                    TempData["success"] = "No changes were made.";
                    return RedirectToAction("Index");
                }

                // Cập nhật thông tin category
                existedCategory.Name = category.Name;
                existedCategory.Description = category.Description;
                existedCategory.Status = category.Status;

                _dataContext.Update(existedCategory);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update successfully";
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
            return View(existedCategory);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            if (category == null)
            {
                return NotFound();
            }
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
