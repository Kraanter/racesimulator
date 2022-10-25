using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml.Schema;
using Controller;
using Model;

namespace WPFApp;

public static class Display
{
    #region Graphics

    private const string _cornerTR = "Resources\\Tracks\\cornerTR.png";
    private const string _cornerBR = "Resources\\Tracks\\cornerBR.png";
    private const string _finish = "Resources\\Tracks\\finish.png";
    private const string _start = "Resources\\Tracks\\start.png";
    private const string _straightH = "Resources\\Tracks\\straightH.png";
    private const string _carDestroyed = "Resources\\Cars\\car-destroyed.png";
    private const string _carPrefix = "Resources\\Cars\\car-";
    private const string _carSuffix = ".png";
    public const int TrackSize = 300;
    private const int _trackOffset = 15;
    private const int _trackWidth = (TrackSize / 2) - _trackOffset;
    private const int _carSize = 128;

    #endregion


    #region methods

    public static BitmapSource DrawTrack(Track track)
    {
        int x = Math.Abs(track.MinMaxCords.minX);
        int y = Math.Abs(track.MinMaxCords.minY);
        Directions direction = Directions.Right;
        Bitmap bitmap = Generator.CreateBitmap(
            track.MinMaxCords.maxX - track.MinMaxCords.minX + 1,
            track.MinMaxCords.maxY - track.MinMaxCords.minY + 1
        );
        foreach (Section section in track.Sections)
        {
            Bitmap image = GenImage(section, direction);
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
        Graphics g = Graphics.FromImage(bitmap);
        Point[] corners =
        {
            new(x, y),
            new(x + image.Width, y),
            new(x, y + image.Height),
        };
        g.DrawImage(image, corners);
    }

    public static Bitmap GenImage(Section section, Directions direction)
    {
        string path = GetImagePath(section.SectionType);
        Bitmap sectionImg = Generator.GetBitmap(path);
        sectionImg = AddParticpants(sectionImg, section);
        sectionImg = RotateImage(sectionImg, direction);
        return sectionImg;
    }

    public static string GetImagePath(SectionTypes sectionType)
    {
        return sectionType switch
        {
            SectionTypes.StartGrid => _start,
            SectionTypes.Finish => _finish,
            SectionTypes.Straight => _straightH,
            SectionTypes.RightCorner => _cornerTR,
            SectionTypes.LeftCorner => _cornerBR,
            _ => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null),
        };
    }
    
    public static Bitmap GenParticipantImage(IParticipant participant)
    {
        string path = _carPrefix + participant.TeamColor + _carSuffix;
        // Without the type cast the image is not drawn correctly (it keeps the cross) WHY?
        Bitmap carImg = (Bitmap) Generator.GetBitmap(path).Clone();
        if (participant.Equipment.IsBroken)
        {
            Debug.WriteLine("Broken " + participant.TeamColor);
            Graphics g = Graphics.FromImage(carImg);
            g.DrawImage(Generator.GetBitmap(_carDestroyed), 0, 0);
        }
        return carImg;
    }

    public static Bitmap RotateImage(Bitmap image, Directions direction)
    {
        image.RotateFlip(direction switch
        {
            Directions.Right => RotateFlipType.RotateNoneFlipNone,
            Directions.Left => RotateFlipType.Rotate180FlipNone,
            Directions.Up => RotateFlipType.Rotate270FlipNone,
            Directions.Down => RotateFlipType.Rotate90FlipNone,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        });
        return image;
    }

    public static Bitmap AddParticpants(Bitmap original, Section section)
    {
        Bitmap returnBitmap = new Bitmap(original);
        Graphics g = Graphics.FromImage(returnBitmap);
        int y = _trackOffset + (_trackWidth - _carSize) / 2;
        bool isCorner = section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner;
        SectionData sectionData = Data.CurrentRace.GetSectionData(section);
        
        foreach (IParticipant? participant in sectionData.GetDrivers())
        {
            if (participant is not null)
            {
                Bitmap layer = new Bitmap(original.Width, original.Height);
                Graphics gLayer = Graphics.FromImage(layer);
                int currentXPos = sectionData.GetParticipantPosition(participant);
                // * 1.00 so the result is a double and not an int
                int x = (int)(currentXPos / (Section.SectionLength * 1.00) * (TrackSize - _trackWidth));
                
                Point[] corners =
                {
                    new(x, y),
                    new(x + _carSize, y),
                    new(x, y + _carSize),
                };
                Bitmap participantImage = GenParticipantImage(participant);
                gLayer.DrawImage(participantImage, corners);
                
                if(isCorner)
                    // if the section is a corner the cars are rotated based on their position on the track
                    layer = RotateImage(layer, currentXPos / 100f * 90f * (section.SectionType == SectionTypes.LeftCorner ? -1 : 1));
                // add the current layer to the final image
                g.DrawImage(layer, 0, 0);
            }
            y += _trackWidth;
        }
        return returnBitmap;
    }


    public static Bitmap RotateImage(Bitmap b, float angle)
    {
        //create a new empty bitmap to hold rotated image
        Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
        //make a graphics object from the empty bitmap
        using (Graphics g = Graphics.FromImage(returnBitmap))
        {
            //move rotation point to center of image
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            g.RotateTransform(angle);
            //move image back
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            //draw passed in image onto graphics object
            g.DrawImage(b, new Point(0, 0));
        }

        return returnBitmap;
    }

    public static void Initialize()
    {
        Generator.Initialize();
    }

    #endregion
}