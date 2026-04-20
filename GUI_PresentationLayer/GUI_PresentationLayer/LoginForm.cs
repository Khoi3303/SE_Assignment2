using System;
using System.Windows.Forms;
using DTO;
using BLL;

namespace GUI_PresentationLayer
{
    public partial class LoginForm : Form
    {
        UserBLL userBLL = new UserBLL();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UserDTO user = new UserDTO();
            user.UserName = txtUsername.Text;
            user.Password = txtPassword.Text;

            if (userBLL.Login(user))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Mở Main Form
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username/password or account is locked.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}