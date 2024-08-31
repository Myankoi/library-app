using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_app
{
    public class UIAdjust
    {
        private LibraryDCDataContext dc = new LibraryDCDataContext();
        public bool UIAdjustBasedOnUserRole(string username)
        {
            var userRole = dc.users.Where(u => u.username.Equals(username)).FirstOrDefault();
            if (userRole.role_id == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
