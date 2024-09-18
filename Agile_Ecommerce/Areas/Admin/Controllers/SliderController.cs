using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agile_Ecommerce.Models;
using Microsoft.AspNetCore.Hosting;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Slider")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Slider.OrderByDescending(p => p.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(SliderModel slider)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(slider.Description))
                {
                    ModelState.AddModelError("Description", "Mô tả không được để trống.");
                    return View(slider); // Trả về view với lỗi hiển thị nếu mô tả bị trống
                }

                if (slider.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await slider.ImageUpload.CopyToAsync(fs);
                    }
                    slider.Image = imageName;
                }

                _dataContext.Add(slider);
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

            return View(slider);
        }
        [Route("Edit/{Id}")]
        public async Task<IActionResult> Edit(int Id, SliderModel sliderModel)
        {

           
            var existed_product = await _dataContext.Slider.FindAsync(Id);



            return View(sliderModel);
        }
        [Route("Edit/{Id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderModel slider)
        {
           var slider_existed = _dataContext.Slider.Find(slider.Id);
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
           
                if (slider.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await slider.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    slider_existed.Image = imageName;
                }
                slider_existed.Name = slider.Name;
                slider_existed.Description = slider.Description;
                slider_existed.Status = slider.Status;
          

                _dataContext.Update(slider_existed);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Edit successfully";
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


            return View(slider);
        }
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            SliderModel slider = await _dataContext.Slider.FindAsync(Id);
            if (slider == null)
            {
                return NotFound();
            }
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
            string oldFileImage = Path.Combine(uploadDir, slider.Image);

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


            _dataContext.Slider.Remove(slider);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Deleted successfully";
            return RedirectToAction("Index");
        }
    }

}
