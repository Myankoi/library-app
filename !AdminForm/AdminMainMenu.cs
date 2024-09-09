using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library_app
{
    public partial class AdminMainMenu : Form
    {
        public AdminMainMenu()
        {
            InitializeComponent();
            Dashboard frmDashboard = new Dashboard();
            frmDashboard.Dock = DockStyle.Fill;
            MainContainer.Controls.Add(frmDashboard);
        }
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            MainContainer.Controls.Clear();
            Dashboard frmDashboard = new Dashboard();
            frmDashboard.Dock = DockStyle.Fill;
            MainContainer.Controls.Add(frmDashboard);
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            MainContainer.Controls.Clear();
            AdminMasterBook frmAddBook = new AdminMasterBook();
            frmAddBook.Dock = DockStyle.Fill;
            MainContainer.Controls.Add(frmAddBook);
        }

        private void btnSearchBook_Click(object sender, EventArgs e)
        {
            MainContainer.Controls.Clear();
            AdminSearchBook frmSearchBook = new AdminSearchBook(MainContainer);
            frmSearchBook.Dock = DockStyle.Fill;
            MainContainer.Controls.Add(frmSearchBook);
        }

        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            MainContainer.Controls.Clear();
            AdminIssueBook frmIssueBook = new AdminIssueBook(MainContainer);
            frmIssueBook.Dock = DockStyle.Fill;
            MainContainer.Controls.Add(frmIssueBook);
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            MainContainer.Controls.Clear();
            AdminReturnBook frmReturnBook = new AdminReturnBook();
            frmReturnBook.Dock = DockStyle.Fill;
            MainContainer.Controls.Add(frmReturnBook);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to logout?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check == DialogResult.Yes)
            {
                Login frmLogin = new Login();
                frmLogin.Show();
                this.Hide();
            }
        }
    }
}
