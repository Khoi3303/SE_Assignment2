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

namespace GUI_PresentationLayer
{
    public partial class FilterForm : Form
    {
        StatisticBLL statBLL = new StatisticBLL();
        AgentBLL agentBLL = new AgentBLL();
        ItemBLL itemBLL = new ItemBLL();

        public FilterForm()
        {
            InitializeComponent();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            // Nạp các lựa chọn cho loại bộ lọc
            cmbFilterType.Items.Add("Top 5 Sản phẩm bán chạy nhất");
            cmbFilterType.Items.Add("Tìm các mặt hàng theo Đại lý");
            cmbFilterType.Items.Add("Tìm các Đại lý theo Mặt hàng");
            cmbFilterType.SelectedIndex = 0; // Chọn mặc định dòng đầu tiên
        }

        // Khi thay đổi loại bộ lọc, thay đổi nội dung của ComboBox tham số
        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterType.SelectedIndex == 0)
            {
                cmbParameter.Enabled = false; // Không cần tham số
                cmbParameter.DataSource = null;
            }
            else if (cmbFilterType.SelectedIndex == 1)
            {
                cmbParameter.Enabled = true;
                cmbParameter.DataSource = agentBLL.GetAllAgents();
                cmbParameter.DisplayMember = "AgentName";
                cmbParameter.ValueMember = "AgentID";
            }
            else if (cmbFilterType.SelectedIndex == 2)
            {
                cmbParameter.Enabled = true;
                cmbParameter.DataSource = itemBLL.GetAllItems();
                cmbParameter.DisplayMember = "ItemName";
                cmbParameter.ValueMember = "ItemID";
            }
        }

        // Nút thực hiện lọc
        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (cmbFilterType.SelectedIndex == 0)
            {
                dgvResults.DataSource = statBLL.GetBestItems();
            }
            else if (cmbFilterType.SelectedIndex == 1 && cmbParameter.SelectedValue != null)
            {
                int agentId = Convert.ToInt32(cmbParameter.SelectedValue);
                dgvResults.DataSource = statBLL.GetItemsByAgent(agentId);
            }
            else if (cmbFilterType.SelectedIndex == 2 && cmbParameter.SelectedValue != null)
            {
                int itemId = Convert.ToInt32(cmbParameter.SelectedValue);
                dgvResults.DataSource = statBLL.GetAgentsByItem(itemId);
            }
        }
    }
}