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
using Microsoft.Reporting.WinForms; // Đừng quên dòng này ở tuốt trên cùng nhé

namespace GUI_PresentationLayer
{
    public partial class PrintForm : Form
    {
        OrderBLL orderBLL = new OrderBLL();

        public PrintForm()
        {
            InitializeComponent();
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            // Lấy chi tiết đơn hàng mới nhất
            DataTable dt = orderBLL.GetLastOrderDetails();

            // Làm sạch dữ liệu cũ
            this.reportViewer1.LocalReport.DataSources.Clear();

            // Đóng gói dữ liệu (Nhớ chữ "DataSet1" phải đúng tên lúc bạn tạo Dataset)
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            // Truyền vào Report và hiển thị
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
        }
    }
}