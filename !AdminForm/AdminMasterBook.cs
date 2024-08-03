using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace library_app
{
    public partial class AdminMasterBook : UserControl
    {
        private LibraryDCDataContext dc = new LibraryDCDataContext();
        private Image coverImage;
        public AdminMasterBook()
        {
            InitializeComponent();
            loadAvailableBook();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string publisher = txtPublisher.Text;
            DateTime publishedDate = datePublished.Value.Date;
            int pages;
            int availableCopies;

            if (!int.TryParse(txtPages.Text, out pages) || !int.TryParse(txtCopies.Text, out availableCopies))
            {
                MessageBox.Show("Please input a valid number for pages and copies");
                return;

            }

            book newBook = new book
            {
                title = title,
                author = author,
                publisher = publisher,
                published = publishedDate,
                pages = pages,
                available_copies = availableCopies
            };

                dc.books.InsertOnSubmit(newBook);
            try
            {
                dc.SubmitChanges();
                MessageBox.Show("Book added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding book: " + ex.Message);
            }
        }

            private void picCover_Click(object sender, EventArgs e)
        {

        }

        private void loadAvailableBook()
        {
            var availableBooks = dc.books.ToList();
            dgvAvailableBook.Columns.Add("Title", "Title");
            dgvAvailableBook.Columns.Add("Author", "Author");
            dgvAvailableBook.Columns.Add("Publisher", "Publisher");
            dgvAvailableBook.Columns.Add("Publish At", "Publish At");
            dgvAvailableBook.Columns.Add("Total Page", "Total Page");
            dgvAvailableBook.Columns.Add("Available", "Available");
            foreach (var book in availableBooks)
            {
                dgvAvailableBook.Rows.Add(
                    book.title, book.author, 
                    book.publisher, book.published.ToShortDateString(), 
                    book.pages, book.available_copies
                );
            }
        }

        private void dgvAvailableBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            txtTitle.Text = dgvAvailableBook.CurrentRow.Cells[0].Value.ToString();
            txtAuthor.Text = dgvAvailableBook.CurrentRow.Cells[1].Value.ToString();
            txtPublisher.Text = dgvAvailableBook.CurrentRow.Cells[2].Value.ToString();
            datePublished.Text = dgvAvailableBook.CurrentRow.Cells[3].Value.ToString();
            txtPages.Text = dgvAvailableBook.CurrentRow.Cells[4].Value.ToString();
            txtCopies.Text = dgvAvailableBook.CurrentRow.Cells[5].Value.ToString();
        }
    }
}
