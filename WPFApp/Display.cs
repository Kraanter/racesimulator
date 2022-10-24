using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace WPFApp;

public static class Display
{
   #region methods

   public static BitmapSource DrawTrack(Track track)
   {
       BitmapSource returnSource;
       returnSource = Generator.CreateBitmapSourceFromGdiBitmap(Generator.CreateBitmap(300, 100));
       return returnSource;
   }

   public static void Initialize()
   {
       Generator.Initialize();
   }

   #endregion
}