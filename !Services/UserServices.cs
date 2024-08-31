using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library_app
{
    public class UserServices
    {
        private LibraryDCDataContext dc = new LibraryDCDataContext();
        private UIAdjust ui = new UIAdjust();

        public void LoginUser(string username, string password, Login frmLogin)
        {
            HashingPassword Hash = new HashingPassword();
            string hashedPassword = Hash.HashPassword(password);
            var login = dc.users.Where(u => u.username.Equals(username)).FirstOrDefault();

            if (login == null || login.password != hashedPassword)
            {
                MessageBox.Show("Username or Password is incorrect.");
                return;
            }

            bool isAdmin = ui.UIAdjustBasedOnUserRole(username);

            if (isAdmin)
            {
                AdminMainMenu frmAdminMainMenu = new AdminMainMenu();
                frmAdminMainMenu.Show();
            } else
            {
                MainMenu frmUserMainMenu = new MainMenu();
                frmUserMainMenu.Show();
            }
            frmLogin.Hide();
        }

        public void RegisterUser(string username, string password, string email)
        {
            try
            {
                HashingPassword hash = new HashingPassword();
                string hashedPassword = hash.HashPassword(password);
                var existingUsername = dc.users.Where(u => u.username.Equals(username)).FirstOrDefault();
                var existingEmail = dc.users.Where(u => u.email.Equals(email)).FirstOrDefault();

                if (existingUsername != null || existingEmail != null)
                {
                    MessageBox.Show("Username or email has been used!");
                    return;
                } else
                {
                    user userData = new user
                    {
                        email = email,
                        username = username,
                        password = hashedPassword,
                    };

                    dc.users.InsertOnSubmit(userData);
                    dc.SubmitChanges();

                    MessageBox.Show("Registration success!");
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
    }
}
