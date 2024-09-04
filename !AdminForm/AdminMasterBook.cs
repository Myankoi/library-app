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

namespace library_app
{
    public partial class AdminMasterBook : UserControl
    {
        private LibraryDCDataContext dc = new LibraryDCDataContext();
        public AdminMasterBook()
        {
            InitializeComponent();
            loadAvailableBook();
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

            if (DateTime.Today > datePublished.Value.Date)
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

            book newBook = new book
            {
                title = title,
                author = author,
                publisher = publisher,
                published = publishedDate,
                pages = pages,
                available_copies = availableCopies,
                image = ConvertImageToBase64(bookCover)
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
            this.Invalidate();
        }

        public String ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
        public Image ConvertBase64ToImage(string imageString)
        {
            byte[] bytes = Convert.FromBase64String(imageString);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
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
            dgvAvailableBook.Columns.Add("Image", "Image");
            dgvAvailableBook.Columns["Image"].Visible = false;
            foreach (var book in availableBooks)
            {
                dgvAvailableBook.Rows.Add(
                    book.title, book.author,
                    book.publisher, book.published.ToShortDateString(),
                    book.pages, book.available_copies, book.image
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
            picCover.Image = ConvertBase64ToImage(dgvAvailableBook.CurrentRow.Cells[6].Value.ToString());
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.bmp)|*.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                coverImage = Image.FromFile(openFileDialog.FileName);
                picCover.Image = coverImage;
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
    }
}
