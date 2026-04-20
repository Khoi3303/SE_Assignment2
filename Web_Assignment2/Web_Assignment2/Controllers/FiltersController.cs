using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Assignment2.Models;

namespace Web_Assignment2.Controllers
{
    // Lớp phụ trợ để chứa kết quả chung đưa lên bảng
    public class FilterResult
    {
        public string Name { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class FiltersController : Controller
    {
        private SE_Assignment2_DBEntities db = new SE_Assignment2_DBEntities();

        // 1. GET: Hiển thị giao diện lúc mới vào
        public ActionResult Index()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            return View(new List<FilterResult>()); // Trả về danh sách rỗng ban đầu
        }

        // 2. POST: Xử lý logic khi người dùng bấm "Lọc dữ liệu"
        [HttpPost]
        public ActionResult Index(int filterType, int? AgentID, int? ItemID)
        {
            // Giữ lại danh sách chọn để form không bị reset sau khi load lại
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", AgentID);
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", ItemID);
            ViewBag.CurrentFilter = filterType; // Để JS biết đang chọn loại nào mà hiển thị ô tương ứng

            List<FilterResult> results = new List<FilterResult>();

            if (filterType == 0) // Lọc 1: Top 5 Sản phẩm bán chạy nhất
            {
                results = db.OrderDetails
                    .GroupBy(od => od.Item.ItemName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity ?? 0) })
                    .OrderByDescending(r => r.TotalQuantity).Take(5).ToList();
            }
            else if (filterType == 1 && AgentID.HasValue) // Lọc 2: Tìm các Mặt hàng theo Đại lý
            {
                results = db.OrderDetails
                    .Where(od => od.Order.AgentID == AgentID.Value)
                    .GroupBy(od => od.Item.ItemName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity ?? 0) })
                    .OrderByDescending(r => r.TotalQuantity).ToList();
            }
            else if (filterType == 2 && ItemID.HasValue) // Lọc 3: Tìm Đại lý theo Mặt hàng
            {
                results = db.OrderDetails
                    .Where(od => od.ItemID == ItemID.Value)
                    .GroupBy(od => od.Order.Agent.AgentName)
                    .Select(g => new FilterResult { Name = g.Key, TotalQuantity = g.Sum(od => od.Quantity ?? 0) })
                    .OrderByDescending(r => r.TotalQuantity).ToList();
            }

            return View(results);
        }
    }
}