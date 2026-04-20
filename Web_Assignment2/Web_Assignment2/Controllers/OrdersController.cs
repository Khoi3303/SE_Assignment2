using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_Assignment2.Models;

namespace Web_Assignment2.Controllers
{
    public class OrdersController : Controller
    {
        private SE_Assignment2_DBEntities db = new SE_Assignment2_DBEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Agent);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // ===================================================================
        // 1. GET: Orders/Create (Hàm hiển thị giao diện Lập Đơn Hàng)
        // ===================================================================
        public ActionResult Create()
        {
            // Truyền danh sách Đại lý
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");

            // Truyền danh sách Sản phẩm 
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");

            // Khởi tạo Giỏ hàng rỗng nếu chưa có
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartItem>();
            }

            return View();
        }

        // ===================================================================
        // 2. POST: Thêm sản phẩm vào giỏ hàng
        // ===================================================================
        [HttpPost]
        public ActionResult AddToCart(int ItemID, int Quantity)
        {
            var cart = Session["Cart"] as List<CartItem>;
            var item = db.Items.Find(ItemID);

            if (item != null)
            {
                var existItem = cart.FirstOrDefault(c => c.ItemID == ItemID);
                if (existItem != null)
                {
                    existItem.Quantity += Quantity; // Cộng dồn nếu đã có
                }
                else
                {
                    cart.Add(new CartItem { ItemID = item.ItemID, ItemName = item.ItemName, Quantity = Quantity, UnitAmount = Convert.ToInt32(item.Price) });
                }
            }
            Session["Cart"] = cart;
            return RedirectToAction("Create"); // Tải lại trang Create
        }

        // ===================================================================
        // 3. POST: Lưu toàn bộ Đơn hàng xuống Database
        // ===================================================================
        [HttpPost]
        public ActionResult SaveOrder(int AgentID)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0) return RedirectToAction("Create");

            // Lưu bảng Order
            Order order = new Order { OrderDate = DateTime.Now, AgentID = AgentID };
            db.Orders.Add(order);
            db.SaveChanges();

            // Lưu bảng OrderDetail
            foreach (var c in cart)
            {
                OrderDetail od = new OrderDetail();
                od.OrderID = order.OrderID;
                od.ItemID = c.ItemID;
                od.Quantity = c.Quantity;
                od.UnitAmount = c.UnitAmount;
                db.OrderDetails.Add(od);
            }
            db.SaveChanges();

            // Xóa giỏ hàng sau khi lưu xong
            Session["Cart"] = null;
            // Chuyển hướng thẳng tới trang Hóa Đơn và truyền theo Mã đơn hàng vừa tạo
            return RedirectToAction("Invoice", new { id = order.OrderID });
        }

        // ===================================================================
        // 4. GET: Trang In Hóa Đơn
        // ===================================================================
        public ActionResult Invoice(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Order order = db.Orders.Find(id);
            if (order == null) return HttpNotFound();

            return View(order);
        }


        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,AgentID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}