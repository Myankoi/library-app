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

namespace library_app._AdminForm
{
    public partial class BookItem : UserControl
    {
        private Panel mainPanel;
        private book book;

        public BookItem(book book, Panel mainPanel)
        {
            InitializeComponent();
            this.book = book;
            this.mainPanel = mainPanel;
            lblBookName.Text = book.title;
            pbBook.Image = ConvertBase64ToImage(book.image);
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

        private void BookItem_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            AdminMasterBook frmSearchBook = new AdminMasterBook(book);
            frmSearchBook.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(frmSearchBook);
        }
    }
}
