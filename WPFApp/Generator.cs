using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace WPFApp
{
    public static class Generator
    {
        private static Dictionary<string, Bitmap> Cache;

        public static void Initialize()
        {
            Cache = new Dictionary<string, Bitmap>();
        }

        public static Bitmap GetBitmap(string Location)
        {
            if(Cache.ContainsKey(Location))
                return Cache[Location];
            Cache[Location] = new Bitmap(Location);
            return (Bitmap) Cache[Location].Clone();
        }

        public static void Clear()
        {
            Cache?.Clear();
        }
        
        public static Bitmap CreateBitmap(int Width, int Height)
        {
            Width *= Display.TrackSize;
            Height *= Display.TrackSize;
            string key = Width + "x" + Height;
            if (Cache.ContainsKey(key))
                return GetBitmap(key);
            Bitmap newImage = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(newImage);
            g.FillRectangle(new SolidBrush(Color.SeaGreen), 0, 0, Width, Height);
            Cache[key] = newImage;
            Debug.WriteLine("Created new bitmap: " + key);
            return (Bitmap) newImage.Clone();
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
