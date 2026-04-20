using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using BLL;

namespace GUI_PresentationLayer
{
    public partial class ItemForm : Form
    {
        ItemBLL itemBLL = new ItemBLL();

        public ItemForm()
        {
            InitializeComponent();
        }

        private void ItemForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgvItems.DataSource = itemBLL.GetAllItems();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemDTO item = new ItemDTO();
                item.ItemName = txtItemName.Text;
                item.Size = txtSize.Text;
                item.Price = Convert.ToDouble(txtPrice.Text);

                if (itemBLL.AddItem(item))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!");
                    LoadData(); // Nạp lại lưới dữ liệu
                }
                else
                {
                    MessageBox.Show("Vui lòng kiểm tra lại thông tin nhập.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message);
            }
        }
    }
}