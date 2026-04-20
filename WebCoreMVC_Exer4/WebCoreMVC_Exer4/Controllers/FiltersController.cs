using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCoreMVC_Exer4.Models;

namespace WebCoreMVC_Exer4.Controllers
{
    // Lớp phụ trợ chứa dữ liệu kết quả in ra bảng
    public class FilterResult
    {
        public string Name { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class FiltersController : Controller
    {
        private readonly AppDbContext db;

        public FiltersController(AppDbContext context)
        {
            db = context;
        }

        // Hàm xử lý cả việc load trang lần đầu và khi bấm nút Lọc (nhận tham số từ URL)
        public IActionResult Index(int FilterType = 0, int? AgentID = null, int? ItemID = null)
        {
            // Tải dữ liệu cho 2 ô Dropdown và giữ lại lựa chọn cũ của người dùng
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", AgentID);
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", ItemID);
            ViewBag.CurrentFilter = FilterType;

            List<FilterResult> results = new List<FilterResult>();

            // Bắt đầu lọc bằng LINQ
            if (FilterType == 0) // Nghiệp vụ 1: Top 5 Sản phẩm
            {
                results = db.OrderDetails
                    .Include(od => od.Item)
                    .GroupBy(od => od.Item.ItemName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity) })
                    .OrderByDescending(r => r.TotalQuantity).Take(5).ToList();
            }
            else if (FilterType == 1 && AgentID.HasValue) // Nghiệp vụ 2: Hàng theo Đại lý
            {
                results = db.OrderDetails
                    .Include(od => od.Order)
                    .Include(od => od.Item)
                    .Where(od => od.Order.AgentID == AgentID.Value)
                    .GroupBy(od => od.Item.ItemName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity) })
                    .OrderByDescending(r => r.TotalQuantity).ToList();
            }
            else if (FilterType == 2 && ItemID.HasValue) // Nghiệp vụ 3: Đại lý theo Hàng
            {
                results = db.OrderDetails
                    .Include(od => od.Order).ThenInclude(o => o.Agent)
                    .Where(od => od.ItemID == ItemID.Value)
                    .GroupBy(od => od.Order.Agent.AgentName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity) })
                    .OrderByDescending(r => r.TotalQuantity).ToList();
            }

            return View(results);
        }
    }
}