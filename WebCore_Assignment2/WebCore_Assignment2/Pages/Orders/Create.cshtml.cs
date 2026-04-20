using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using WebCore_Assignment2.Models;

namespace WebCore_Assignment2.Pages.Orders
{
    // Lớp phụ trợ làm cái túi đựng hàng
    public class CartItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int UnitAmount { get; set; }
        public int SubTotal => Quantity * UnitAmount;
    }

    public class CreateModel : PageModel
    {
        private readonly SeAssignment2DbContext db = new SeAssignment2DbContext();

        public SelectList AgentList { get; set; }
        public SelectList ItemList { get; set; }
        public List<CartItem> Cart { get; set; } = new List<CartItem>();

        // Hàm chạy khi trang vừa được tải lên
        public void OnGet()
        {
            LoadData();
        }

        // Hàm phụ trợ tải danh sách Dropdown và đọc Giỏ hàng từ Session
        private void LoadData()
        {
            // Lưu ý: Nếu chữ AgentId bị gạch đỏ, hãy đổi thành AgentID tùy theo tên cột trong DB của bạn
            AgentList = new SelectList(db.Agents, "AgentId", "AgentName");
            ItemList = new SelectList(db.Items, "ItemId", "ItemName");

            var cartJson = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cartJson))
            {
                Cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }
        }

        // Nút 1: Thêm vào giỏ
        public IActionResult OnPostAddToCart(int ItemId, int Quantity)
        {
            LoadData();
            var item = db.Items.Find(ItemId);

            if (item != null)
            {
                var existItem = Cart.FirstOrDefault(c => c.ItemId == ItemId);
                if (existItem != null)
                {
                    existItem.Quantity += Quantity;
                }
                else
                {
                    Cart.Add(new CartItem { ItemId = item.ItemId, ItemName = item.ItemName, Quantity = Quantity, UnitAmount = Convert.ToInt32(item.Price) });
                }

                // Mã hóa giỏ hàng thành chuỗi JSON để lưu vào Session
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(Cart));
            }
            return RedirectToPage(); // Tải lại trang hiện tại
        }

        // Nút 2: Lưu thẳng xuống DB
        public IActionResult OnPostSaveOrder(int AgentId)
        {
            LoadData();
            if (Cart.Count == 0) return RedirectToPage();

            // 1. Lưu Order
            Order order = new Order { OrderDate = DateTime.Now, AgentId = AgentId };
            db.Orders.Add(order);
            db.SaveChanges(); // Lưu để lấy mã OrderId tự tăng

            // 2. Lưu OrderDetail
            foreach (var c in Cart)
            {
                db.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ItemId = c.ItemId,
                    Quantity = c.Quantity,
                    UnitAmount = c.UnitAmount
                });
            }
            db.SaveChanges();

            // 3. Xóa Session giỏ hàng
            HttpContext.Session.Remove("Cart");

            // Chuyển hướng sang trang Invoice và truyền theo mã Đơn hàng (OrderId)
            return RedirectToPage("Invoice", new { id = order.OrderId });
        }
    }
}