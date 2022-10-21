using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Images.Clear();
        }
    }
}
