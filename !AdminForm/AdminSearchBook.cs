using library_app._AdminForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library_app
{
    public partial class AdminSearchBook : UserControl
    {
        private LibraryDCDataContext dc = new LibraryDCDataContext();
        private Panel mainPanel;
        private List<book> books;

        public AdminSearchBook(Panel mainPanel)
        {
            InitializeComponent();
            this.mainPanel = mainPanel;

            books = new List<book>();
            loadAvailableBook();
            populatePanel(books);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var search = books.Where(b=>b.title.Contains(txtSearch.Text.ToLower())).ToList();
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                populatePanel(search);
            }
            else
            {
                populatePanel(books);
            }
        }

        private void loadAvailableBook()
        {
            var availableBooks = dc.books.ToList().OrderBy(b => b.title);
            foreach (var book in availableBooks)
            {
                books.Add(book);
            }
        }

        private void populatePanel(List<book> books)
        {
            pBook.Controls.Clear();
            foreach (book book in books)
            {
                pBook.Controls.Add(new BookItem(book, mainPanel));
            }
        }
    }
}
