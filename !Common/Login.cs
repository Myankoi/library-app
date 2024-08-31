using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace library_app
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if(string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username must be filled!");
                return;
            }

            if(string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password must be filled!");
                return;
            }

            UserServices user = new UserServices();
            user.LoginUser(username, password, this);
            }

        private void lkSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register frmRegister = new Register();
            frmRegister.Show();
            this.Hide();
        }

        private void cbxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
        }
    }
}
