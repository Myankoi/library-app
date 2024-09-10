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
using System.IO;
using System.Data.Linq;
using System.Drawing.Imaging;
using library_app._Services;

namespace library_app
{
    public partial class AdminMasterBook : UserControl
    {
        private LibraryDCDataContext dc = new LibraryDCDataContext();
        private ConvertHelper convertHelper = new ConvertHelper();
        private int id = 0;

        public AdminMasterBook()
        {
            InitializeComponent();
            loadAvailableBook();
        }

        public AdminMasterBook(book book)
        {
            InitializeComponent();
            loadAvailableBook();

            txtTitle.Text = book.title;
            txtAuthor.Text = book.author;
            txtPublisher.Text = book.publisher;
            datePublished.Text = book.published.ToString();
            txtPages.Text = book.pages.ToString();
            txtCopies.Text = book.available_copies.ToString();
            picCover.Image = convertHelper.ConvertBase64ToImage(book.image.ToString());
            id = int.Parse(book.id.ToString());
        }

        private Image coverImage;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string publisher = txtPublisher.Text;
            DateTime publishedDate = datePublished.Value.Date;
            int pages;
            int availableCopies;
            Image bookCover = picCover.Image;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(publisher))
            {
                MessageBox.Show("Please fill all data book!");
                return;
            }

            if (DateTime.Today < datePublished.Value.Date)
            {
                MessageBox.Show("Please pick a valid date!");
                return;
            }

            if (!int.TryParse(txtPages.Text, out pages) || !int.TryParse(txtCopies.Text, out availableCopies))
            {
                MessageBox.Show("Please input a valid number for pages and copies");
                return;
            }

            if (picCover.Image == null)
            {
                MessageBox.Show("Please upload the book cover!");
                return;
            }

            if(isExist(title))
            {
                MessageBox.Show("The title is already exist");
                return;
            }

            book newBook = new book
            {
                title = title,
                author = author,
                publisher = publisher,
                published = publishedDate,
                pages = pages,
                available_copies = availableCopies,
                image = convertHelper.ConvertImageToBase64(bookCover)
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
            loadAvailableBook();
        }

        private bool isExist(string title)
        {
            return dc.books.Where(x  => x.title == title).FirstOrDefault() != null;
        }

        private void loadAvailableBook()
        {
            dgvAvailableBook.Columns.Clear();
            var availableBooks = dc.books.ToList().OrderBy(b => b.title);
            dgvAvailableBook.Columns.Add("Title", "Title");
            dgvAvailableBook.Columns.Add("Author", "Author");
            dgvAvailableBook.Columns.Add("Publisher", "Publisher");
            dgvAvailableBook.Columns.Add("Publish At", "Publish At");
            dgvAvailableBook.Columns.Add("Total Page", "Total Page");
            dgvAvailableBook.Columns.Add("Available", "Available");
            dgvAvailableBook.Columns.Add("Image", "Image");
            dgvAvailableBook.Columns.Add("ID", "ID");
            dgvAvailableBook.Columns["ID"].Visible = false;
            dgvAvailableBook.Columns["Image"].Visible = false;

            foreach (var book in availableBooks)
            {
                dgvAvailableBook.Rows.Add(
                    book.title, book.author,
                    book.publisher, book.published.ToShortDateString(),
                    book.pages, book.available_copies, book.image, book.id
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
            picCover.Image = convertHelper.ConvertBase64ToImage(dgvAvailableBook.CurrentRow.Cells[6].Value.ToString());
            id = int.Parse(dgvAvailableBook.CurrentRow.Cells[7].Value.ToString());
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string publisher = txtPublisher.Text;
            DateTime publishedDate = datePublished.Value.Date;
            int pages;
            int availableCopies;
            Image bookCover = picCover.Image;

            if (dgvAvailableBook.CurrentRow == null)
            {
                MessageBox.Show("Please select a book to update!");
                return;
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(publisher))
            {
                MessageBox.Show("Please fill all data book!");
                return;
            }

            if (DateTime.Today < datePublished.Value.Date)
            {
                MessageBox.Show("Please pick a valid date!");
                return;
            }

            if (!int.TryParse(txtPages.Text, out pages) || !int.TryParse(txtCopies.Text, out availableCopies))
            {
                MessageBox.Show("Please input a valid number for pages and copies");
                return;
            }

            if (picCover.Image == null)
            {
                MessageBox.Show("Please upload the book cover!");
                return;
            }

            string selectedTitle = dgvAvailableBook.CurrentRow.Cells[0].Value.ToString();
            string selectedCover = dgvAvailableBook.CurrentRow.Cells[6].Value.ToString();

            var bookUpdate = dc.books.SingleOrDefault(b => b.title == selectedTitle);

            if (bookUpdate != null)
            {
                bookUpdate.title = title;
                bookUpdate.author = author;
                bookUpdate.publisher = publisher;
                bookUpdate.published = publishedDate;
                bookUpdate.pages = pages;
                bookUpdate.available_copies = availableCopies;

                if (selectedCover == convertHelper.ConvertImageToBase64(bookCover))
                {
                    bookUpdate.image = convertHelper.ConvertImageToBase64(bookCover);
                } else
                {
                    bookUpdate.image = convertHelper.ConvertImageToBase64(bookCover);
                }
                
                try
                {
                    dc.SubmitChanges();
                    MessageBox.Show("Book updated successfully");
                    loadAvailableBook();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Updating Book: " + ex.Message);
                }
            } else
            {
                MessageBox.Show("Book is not found in database!");
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtPublisher.Clear();
            datePublished.ResetText();
            txtPages.Clear();
            txtCopies.Clear();
            picCover.Image = null;
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.bmp;) | *.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                coverImage = Image.FromFile(openFileDialog.FileName);
                picCover.Image = coverImage;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var book = dc.books.Where(b => b.id == id).FirstOrDefault();
            if (book == null)
            {
                MessageBox.Show("Book is not found in database!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure want to delete this book?", "Delete Book", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                dc.books.DeleteOnSubmit(book);
                dc.SubmitChanges();
                loadAvailableBook();
            } else
            {
                return;
            }
        }
    }
}