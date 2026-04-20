using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebCore_Assignment2.Models;

namespace WebCore_Assignment2.Pages.Orders
{
    public class InvoiceModel : PageModel
    {
        private readonly SeAssignment2DbContext db = new SeAssignment2DbContext();

        public Order OrderInfo { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null) return NotFound();

            // Lấy đơn hàng kèm theo thông tin Đại lý và Chi tiết sản phẩm
            OrderInfo = db.Orders
                .Include(o => o.Agent)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                .FirstOrDefault(m => m.OrderId == id);

            if (OrderInfo == null) return NotFound();

            return Page();
        }
    }
}