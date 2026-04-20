using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DTO;

namespace GUI_PresentationLayer
{
    public partial class OrderForm : Form
    {
        OrderBLL orderBLL = new OrderBLL();
        ItemBLL itemBLL = new ItemBLL();
        // AgentBLL agentBLL = new AgentBLL(); // (Nếu bạn đã tạo AgentBLL)
        // DAL.ItemDAL itemDAL = new DAL.ItemDAL(); // Tạm gọi thẳng DAL nếu chưa làm BLL
        DAL.DBConnection db = new DAL.DBConnection(); // Tạm dùng để load Agent nếu bạn chưa làm AgentDAL

        DataTable dtCart; // Giỏ hàng tạm

        public OrderForm()
        {
            InitializeComponent();
            InitCart();
        }

        private void InitCart()
        {
            // Tạo cấu trúc cho giỏ hàng
            dtCart = new DataTable();
            dtCart.Columns.Add("ItemID", typeof(int));
            dtCart.Columns.Add("ItemName", typeof(string));
            dtCart.Columns.Add("Quantity", typeof(int));
            dtCart.Columns.Add("Price", typeof(double));
            dtCart.Columns.Add("SubTotal", typeof(double));
            dgvCart.DataSource = dtCart;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            // Load danh sách Item vào ComboBox
            cmbItems.DataSource = itemBLL.GetAllItems();
            cmbItems.DisplayMember = "ItemName";
            cmbItems.ValueMember = "ItemID";

            // Đoạn này load Agent, bạn sửa lại dùng AgentBLL nếu đã tạo nhé
            string query = "SELECT * FROM Agent";
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(query, System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            DataTable dtAgents = new DataTable();
            da.Fill(dtAgents);

            cmbAgents.DataSource = dtAgents;
            cmbAgents.DisplayMember = "AgentName";
            cmbAgents.ValueMember = "AgentID";
        }

        // Khi đổi sản phẩm, hiển thị giá tương ứng
        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem đã có món hàng nào được chọn chưa
            if (cmbItems.SelectedItem != null)
            {
                // Try to get the selected item's ID and fetch price from BLL/DAL
                if (cmbItems.SelectedValue != null && int.TryParse(cmbItems.SelectedValue.ToString(), out int itemId))
                {
                    double? price = itemBLL.GetPriceById(itemId);
                    txtPrice.Text = price.HasValue ? price.Value.ToString() : string.Empty;
                }
            }
        }

        // Thêm hàng vào giỏ tạm
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Máy tính đang đọc được:\n- Biến txtQuantity: [{txtQuantity.Text}]\n- Biến txtPrice: [{txtPrice.Text}]", "Debug info");
            // 1. Kiểm tra xem ô Số lượng và ô Giá tiền có bị trống không
            if (string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng và đảm bảo giá tiền không bị trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Dùng TryParse để ép kiểu an toàn. Nếu nhập chữ "abc" vào ô số lượng, nó sẽ báo lỗi thay vì crash app.
            if (!int.TryParse(txtQuantity.Text, out int qty) || !double.TryParse(txtPrice.Text, out double price))
            {
                MessageBox.Show("Số lượng hoặc Giá tiền không hợp lệ (phải là số)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Lấy dữ liệu và thêm vào giỏ
            int itemId = Convert.ToInt32(cmbItems.SelectedValue);
            string itemName = cmbItems.Text;

            dtCart.Rows.Add(itemId, itemName, qty, price, qty * price);
        }

        // Lưu toàn bộ đơn hàng xuống CSDL
        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            if (dtCart.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống!");
                return;
            }

            // 1. Tạo đối tượng Order
            OrderDTO order = new OrderDTO();
            order.OrderDate = DateTime.Now;
            order.AgentID = Convert.ToInt32(cmbAgents.SelectedValue);

            // 2. Tạo danh sách OrderDetail từ giỏ hàng
            List<OrderDetailDTO> details = new List<OrderDetailDTO>();
            foreach (DataRow row in dtCart.Rows)
            {
                OrderDetailDTO detail = new OrderDetailDTO();
                detail.ItemID = Convert.ToInt32(row["ItemID"]);
                detail.Quantity = Convert.ToInt32(row["Quantity"]);
                detail.UnitAmount = Convert.ToDouble(row["Price"]);
                details.Add(detail);
            }

            // 3. Gọi BLL để lưu
            if (orderBLL.SaveFullOrder(order, details))
            {
                MessageBox.Show("Lưu đơn hàng thành công!", "Thông báo");
                dtCart.Clear(); // Xóa giỏ hàng sau khi lưu xong

                // MỞ HÓA ĐƠN LÊN NGAY TẠI ĐÂY:
                PrintForm frmPrint = new PrintForm();
                frmPrint.ShowDialog();
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            // Intentionally left empty to avoid re-entrancy / accessing DataRowView here.
        }
    }
}