using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCoreMVC_Exer4.Models
{
    // 1. Bảng Sản phẩm
    public class Item
    {
        [Key]
        public int ItemID { get; set; }

        [Required, StringLength(100)]
        public string ItemName { get; set; }

        public string? Size { get; set; } // Thêm dấu ? cho phép rỗng
        public double Price { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; } // Thêm dấu ?
    }

    // 2. Bảng Đại lý
    public class Agent
    {
        [Key]
        public int AgentID { get; set; }

        [Required, StringLength(100)]
        public string AgentName { get; set; }

        public string? Address { get; set; } // Thêm dấu ? cho phép rỗng

        public ICollection<Order>? Orders { get; set; } // Thêm dấu ?
    }

    // 3. Bảng Đơn hàng
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }

        // Khóa ngoại nối với bảng Agent
        public int AgentID { get; set; }
        [ForeignKey("AgentID")]
        public Agent? Agent { get; set; } // Thêm dấu ?

        public ICollection<OrderDetail>? OrderDetails { get; set; } // Thêm dấu ?
    }

    // 4. Bảng Chi tiết Đơn hàng
    public class OrderDetail
    {
        // Khóa ngoại nối với bảng Order
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order? Order { get; set; } // Thêm dấu ?

        // Khóa ngoại nối với bảng Item
        public int ItemID { get; set; }
        [ForeignKey("ItemID")]
        public Item? Item { get; set; } // Thêm dấu ?

        public int Quantity { get; set; }
        public double UnitAmount { get; set; }
    }

    // 5. Cấu hình Database Context
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập khóa chính kép cho bảng OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderID, od.ItemID });
        }
    }
}