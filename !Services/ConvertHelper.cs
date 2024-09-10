using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_app._Services
{
    public class ConvertHelper
    {
        private Image image;
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
        public String ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image clonedImage = new Bitmap(image))
                {
                    clonedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }
    }
}
