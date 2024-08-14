using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Brand")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;
        }
        [Route("Index")]
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<BrandModel> category = _dataContext.Brands.ToList(); //33 datas


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

        [Route("Create")]
		public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		[Route("Create")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                brand.Slug = brand.Name.Replace(" ", "-");
                brand.Description = brand.Description.Replace("<p>", "").Replace("</p>", "").Replace("<br>", "\n");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Brand exists already");
                    return View(brand);
                }

                _dataContext.Add(brand);
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
            return View(brand);
        }

        [Route("Edit/{Id}")] // thêm Id vào route
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand  = await _dataContext.Brands.FindAsync(Id);
            if (brand == null) // Kiểm tra xem brand có tồn tại hay không
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{Id}")] // thêm Id vào route
        public async Task<IActionResult> Edit(int Id, BrandModel brand)
        {
            // Tìm brand trong database dựa vào Id
            var existedBrand = await _dataContext.Brands.FindAsync(Id);

            if (existedBrand == null) // Kiểm tra xem brand có tồn tại hay không
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy
            }

            // Kiểm tra model state
            if (ModelState.IsValid)
            {
                // Tạo slug mới từ name
                brand.Slug = brand.Name.Replace(" ", "-");

                // Kiểm tra slug có trùng với slug hiện tại hay không
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug && p.Id != Id);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Category exists already");
                    return View(existedBrand);
                }

                // Kiểm tra xem có thay đổi nào không
                if (existedBrand.Name == brand.Name &&
                    existedBrand.Description == brand.Description &&
                    existedBrand.Status == brand.Status)
                {
                    TempData["success"] = "No changes were made.";
                    return RedirectToAction("Index");
                }

                // Cập nhật thông tin category
                existedBrand.Name = brand.Name;
                existedBrand.Description = brand.Description;
                existedBrand.Status = brand.Status;

                _dataContext.Update(existedBrand);
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
            return View(existedBrand);
        }
        [Route("Delete")]
		public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);
            if (brand == null)
            {
                return NotFound();
            }
            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
