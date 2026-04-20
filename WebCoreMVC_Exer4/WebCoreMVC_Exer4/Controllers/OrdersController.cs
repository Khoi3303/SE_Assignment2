using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebCoreMVC_Exer4.Models;

namespace WebCoreMVC_Exer4.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext db;

        // Tiêm DbContext vào Controller (Dependency Injection)
        public OrdersController(AppDbContext context)
        {
            db = context;
        }

        // 1. Hàm Load giao diện Lập Đơn Hàng
        public IActionResult Create()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");

            // Đọc Giỏ hàng từ Session
            List<CartItem> cart = new List<CartItem>();
            string cartJson = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }

            return View(cart); // Gửi giỏ hàng sang View
        }

        // 2. Hàm Xử lý khi bấm nút "Thêm vào giỏ"
        [HttpPost]
        public IActionResult AddToCart(int ItemID, int Quantity)
        {
            var item = db.Items.Find(ItemID);
            if (item != null)
            {
                List<CartItem> cart = new List<CartItem>();
                string cartJson = HttpContext.Session.GetString("Cart");
                if (!string.IsNullOrEmpty(cartJson))
                {
                    cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                }

                var existItem = cart.FirstOrDefault(c => c.ItemId == ItemID);
                if (existItem != null)
                {
                    existItem.Quantity += Quantity;
                }
                else
                {
                    cart.Add(new CartItem { ItemId = item.ItemID, ItemName = item.ItemName, Quantity = Quantity, UnitAmount = item.Price });
                }

                // Mã hóa và lưu lại vào Session
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }
            return RedirectToAction("Create");
        }

        // 3. Hàm Xử lý khi bấm nút "Lưu Đơn Hàng"
        [HttpPost]
        public IActionResult SaveOrder(int AgentID)
        {
            string cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson)) return RedirectToAction("Create");

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            if (cart.Count == 0) return RedirectToAction("Create");

            // Lưu Đơn Hàng (Order)
            Order order = new Order { OrderDate = DateTime.Now, AgentID = AgentID };
            db.Orders.Add(order);
            db.SaveChanges();

            // Lưu Chi tiết Đơn Hàng (OrderDetail)
            foreach (var c in cart)
            {
                db.OrderDetails.Add(new OrderDetail { OrderID = order.OrderID, ItemID = c.ItemId, Quantity = c.Quantity, UnitAmount = c.UnitAmount });
            }
            db.SaveChanges();

            // Xóa Session
            HttpContext.Session.Remove("Cart");

            // Chuyển hướng sang trang Hóa Đơn và truyền theo mã Đơn hàng (OrderID)
            return RedirectToAction("Invoice", new { id = order.OrderID });
        }
        // 4. Hàm hiển thị Hóa Đơn
        public IActionResult Invoice(int? id)
        {
            if (id == null) return NotFound();

            // Nhớ thêm using Microsoft.EntityFrameworkCore; ở trên cùng file nếu nó báo lỗi gạch đỏ chữ Include nhé
            var orderInfo = db.Orders
                .Include(o => o.Agent)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Item)
                .FirstOrDefault(m => m.OrderID == id);

            if (orderInfo == null) return NotFound();

            return View(orderInfo);
        }
    }
}