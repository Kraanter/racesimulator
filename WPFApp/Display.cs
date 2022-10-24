using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml.Schema;
using Controller;
using Model;

namespace WPFApp;

public static class Display
{
    #region Graphics
    private const string _cornerTL = "Resources\\Tracks\\cornerTL.png";
    private const string _cornerTR = "Resources\\Tracks\\cornerTR.png";
    private const string _cornerBL = "Resources\\Tracks\\cornerBL.png";
    private const string _cornerBR = "Resources\\Tracks\\cornerBR.png";
    private const string _finish = "Resources\\Tracks\\finish.png";
    private const string _start = "Resources\\Tracks\\start.png";
    private const string _straightV = "Resources\\Tracks\\straightV.png";
    private const string _straightH = "Resources\\Tracks\\straightH.png";
    public const int TrackSize = 300;
    #endregion


    #region methods

   public static BitmapSource DrawTrack(Track track)
   {
       int x = Math.Abs(track.MinMaxCords.minX); int y = Math.Abs(track.MinMaxCords.minY); Directions direction = Directions.Right;
       Bitmap bitmap = Generator.CreateBitmap(
           track.MinMaxCords.maxX - track.MinMaxCords.minX + 1,
           track.MinMaxCords.maxY - track.MinMaxCords.minY + 1
       );
       foreach (Section section in track.Sections)
       {
           SectionTypes sectionType = section.SectionType;
           Bitmap image = GetImage(sectionType, direction);
           AddImageToBitmap(bitmap, image, x, y);
           direction = section.GetNextDirection(direction);
           switch (direction)
            {
                case Directions.Up:
                    y--;
                    break;
                case Directions.Down:
                    y++;
                    break;
                case Directions.Left:
                    x--;
                    break;
                case Directions.Right:
                    x++;
                    break;
            }
       }
       return Generator.CreateBitmapSourceFromGdiBitmap(bitmap);
   }
   
   public static void AddImageToBitmap(Bitmap bitmap, Bitmap image, int x, int y)
   {
       x *= TrackSize;
       y *= TrackSize;
       Debug.WriteLine("x: " + x + " y: " + y);
       Graphics g = Graphics.FromImage(bitmap);
       Point[] corners = { new Point(x, y), new Point(x + image.Width, y), new Point(x, y + image.Height) };
       g.DrawImage(image, corners);
   }
   
    public static Bitmap GetImage(SectionTypes sectionType, Directions direction)
    {
        bool vertical = direction == Directions.Up || direction == Directions.Down;
        string path = GetImagePath(sectionType, direction, vertical);
        return Generator.GetBitmap(path);
    }
    public static string GetImagePath(SectionTypes sectionType, Directions direction, bool vertical)
    {
        return sectionType switch
        {
            SectionTypes.StartGrid => _start,
            SectionTypes.Finish => _finish,
            SectionTypes.Straight => vertical ? _straightV : _straightH,
            SectionTypes.RightCorner => direction switch
            {
                Directions.Down => _cornerBR,
                Directions.Up => _cornerTL,
                Directions.Left => _cornerBL,
                Directions.Right => _cornerTR,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            },
            SectionTypes.LeftCorner => direction switch
            {
                Directions.Down => _cornerBL,
                Directions.Up => _cornerTR,
                Directions.Left => _cornerTL,
                Directions.Right => _cornerBR,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null),
        };
    }

   public static void Initialize()
   {
       Generator.Initialize();
   }

   #endregion
}