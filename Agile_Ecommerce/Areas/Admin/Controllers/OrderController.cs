using Agile_Ecommerce.Models;
using Agile_Ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agile_Ecommerce.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<OrderModel> category = _dataContext.Orders.ToList(); //33 datas


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
            return View();
        }
        public async Task<IActionResult> ViewOrder(string orderCode)
        {
            // Lấy OrderModel dựa trên orderCode
            var order = await _dataContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

            // Nếu không tìm thấy OrderModel, trả về NotFound
            if (order == null)
            {
                return NotFound();
            }

            // Truyền OrderModel vào View
            return View(order);
        }
        public async Task<IActionResult> Delete(string orderCode)
        {
            // Lấy đơn hàng dựa trên orderCode
            var order = await _dataContext.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

            // Nếu không tìm thấy đơn hàng, trả về NotFound
            if (order == null)
            {
                return NotFound();
            }

            // Xóa các chi tiết đơn hàng liên quan
            foreach (var orderDetail in order.OrderDetails)
            {
                _dataContext.OrderDetails.Remove(orderDetail);
            }

            // Xóa đơn hàng
            _dataContext.Orders.Remove(order);

            // Lưu thay đổi
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Order deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
