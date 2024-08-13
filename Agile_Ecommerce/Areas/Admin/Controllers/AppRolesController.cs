using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Role")]
    [Authorize]
    public class AppRolesController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AppRolesController(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = context;
            _roleManager = roleManager;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderByDescending(p => p.Id).ToListAsync());
        }
        [HttpGet]
        [Route("Create")]
        
        public async Task<IActionResult> Create()
		{
			return View();
		}
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            //kiểm tra xem role đã tồn tại hay chưa
            if (_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                // Role đã tồn tại, hiển thị thông báo lỗi
                TempData["error"] = "Role already exist!";
            }
            else
            {
                // Tạo role mới
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
                // Hiển thị thông báo thành công
                TempData["success"] = "Added Role successfully";
            }

            return Redirect("Index");
        }
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit( string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                role.Name = model.Name;

                try
                {
                    await _roleManager.UpdateAsync(role);
                    TempData["success"] = "Role updated successfully!";
                    return RedirectToAction("Index");
                } catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occured while updating the role");
                }
            }
            //nếu như model không hợp lệ hoặc quyền không tìm thấy, trả về view với model hoặc 1 model trống cho quyền mới
            return View(model ?? new IdentityRole { Id = id});
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            try
            {
                await _roleManager.DeleteAsync(role);
                TempData["success"] = "Role deleted successfully!";
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the role.");
            }

            return Redirect("Index");
        }

    }
}
