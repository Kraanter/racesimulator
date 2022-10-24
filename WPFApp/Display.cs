using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace WPFApp;

public static class Display
{
    #region Graphics
    private const string _cornerTL = ".\\Resources\\Tracks\\cornerTL.png";
    private const string _cornerTR = ".\\Resources\\Tracks\\cornerTR.png";
    private const string _cornerBL = ".\\Resources\\Tracks\\cornerBL.png";
    private const string _cornerBR = ".\\Resources\\Tracks\\cornerBR.png";
    private const string _finish = ".\\Resources\\Tracks\\finish.png";
    private const string _start = ".\\Resources\\Tracks\\start.png";
    private const string _straightV = ".\\Resources\\Tracks\\straightV.png";
    private const string _straightH = ".\\Resources\\Tracks\\straightH.png";
    
    #endregion


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