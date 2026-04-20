using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_PresentationLayer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void quảnLýSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo instance của ItemForm và hiển thị nó
            ItemForm itemForm = new ItemForm();
            itemForm.Show();
        }

        private void đăngSuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng MainForm
            LoginForm login = new LoginForm();
            login.Show(); // Mở lại LoginForm
        }

        private void quảnLýĐơnHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một phiên bản (instance) của OrderForm và hiển thị nó lên
            OrderForm orderForm = new OrderForm();
            orderForm.Show();
        }

        private void lọcDữLiệuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterForm filterForm = new FilterForm();
            filterForm.Show();
        }
    }
}
