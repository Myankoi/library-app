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
    public partial class AdminIssueBook : UserControl
    {
        private book book;
        private Panel mainPanel;
        public AdminIssueBook()
        {
            InitializeComponent();
        }

        public AdminIssueBook(Panel mainPanel)
        {
            InitializeComponent();
            this.mainPanel = mainPanel;
        }
        public AdminIssueBook(book book, Panel mainPanel)
        {
            InitializeComponent();
            this.book = book;
            textTitle.Text = book.title;
            textAuthor.Text = book.author;
            textPublisher.Text = book.publisher;
            picBook.Image = ConvertBase64ToImage(book.image);
            this.mainPanel = mainPanel;
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
    }
}
