using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCore_Assignment2.Models;

namespace WebCore_Assignment2.Pages.Filters
{
    // Lớp phụ trợ chứa dữ liệu kết quả in ra bảng
    public class FilterResult
    {
        public string Name { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly SeAssignment2DbContext db = new SeAssignment2DbContext();

        public SelectList AgentList { get; set; }
        public SelectList ItemList { get; set; }
        public List<FilterResult> Results { get; set; } = new List<FilterResult>();

        // [BindProperty(SupportsGet = true)] giúp nhận dữ liệu trực tiếp từ URL khi bấm nút Lọc
        [BindProperty(SupportsGet = true)]
        public int FilterType { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? AgentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ItemId { get; set; }

        // Hàm duy nhất xử lý cả việc Load trang lẫn khi bấm Lọc
        public void OnGet()
        {
            // Tải dữ liệu cho 2 ô Dropdown
            AgentList = new SelectList(db.Agents, "AgentId", "AgentName", AgentId);
            ItemList = new SelectList(db.Items, "ItemId", "ItemName", ItemId);

            // Bắt đầu lọc bằng LINQ tùy theo loại nghiệp vụ
            if (FilterType == 0) // Nghiệp vụ 1: Top 5 Sản phẩm
            {
                Results = db.OrderDetails
                    .Include(od => od.Item)
                    .GroupBy(od => od.Item.ItemName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity ?? 0) })
                    .OrderByDescending(r => r.TotalQuantity).Take(5).ToList();
            }
            else if (FilterType == 1 && AgentId.HasValue) // Nghiệp vụ 2: Hàng theo Đại lý
            {
                Results = db.OrderDetails
                    .Include(od => od.Order)
                    .Include(od => od.Item)
                    .Where(od => od.Order.AgentId == AgentId.Value)
                    .GroupBy(od => od.Item.ItemName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity ?? 0) })
                    .OrderByDescending(r => r.TotalQuantity).ToList();
            }
            else if (FilterType == 2 && ItemId.HasValue) // Nghiệp vụ 3: Đại lý theo Hàng
            {
                Results = db.OrderDetails
                    .Include(od => od.Order).ThenInclude(o => o.Agent)
                    .Where(od => od.ItemId == ItemId.Value)
                    .GroupBy(od => od.Order.Agent.AgentName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity ?? 0) })
                    .OrderByDescending(r => r.TotalQuantity).ToList();
            }
        }
    }
}