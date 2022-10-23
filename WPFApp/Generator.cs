using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WPFApp
{
    public static class Generator
    {
        private static Dictionary<string, Bitmap> Images;

        public static void Initialize()
        {
            Images = new Dictionary<string, Bitmap>();
        }

        public static Bitmap GetBitmap(string Location)
        {
            if(Images.ContainsKey(Location))
                return Images[Location];
            Images[Location] = new Bitmap(Location);
            return Images[Location];
        }

        public static void clear()
        {
            Images?.Clear();
        }
        
        public static Bitmap CreateBitmap(int Width, int Height)
        {
            string key = Width + "x" + Height;
            if (Images.ContainsKey(key))
                return Images[key];
            Bitmap image = GetBitmap(key);
            Bitmap newImage = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(newImage);
            g.DrawImage(image, 0, 0, Width, Height);
            return newImage;
        }
        
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
