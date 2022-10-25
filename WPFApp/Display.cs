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
    public const int TrackSize = 300;
    private const int TrackOffset = 15;
    private const int TrackWidth = (TrackSize / 2) - TrackOffset;
    private const int CarSize = 128;

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
        Debug.WriteLine("x: " + x + " y: " + y);
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
        Bitmap tempimage = new Bitmap(original.Width, original.Height);
        Graphics g = Graphics.FromImage(tempimage);
        int y = TrackOffset + (TrackWidth - CarSize) / 2;
        bool isCorner = section.SectionType == SectionTypes.LeftCorner || section.SectionType == SectionTypes.RightCorner;
        SectionData sectionData = Data.CurrentRace.GetSectionData(section);
        foreach (IParticipant? participant in sectionData.GetDrivers())
        {
            if (participant is not null)
            {
                int currentXPos = sectionData.GetParticipantPosition(participant);
                // * 1.00 so the result is a double and not an int
                int x = (int)((currentXPos / (Section.SectionLength * 1.00)) * (TrackSize - TrackWidth));
                Debug.WriteLine("currentXPos: " + currentXPos + " x " + x + "teamColor: " + participant.TeamColor);
                Point[] corners =
                {
                    new(x, y),
                    new(x + CarSize, y),
                    new(x, y + CarSize),
                };
                Bitmap carImg = Generator.GetBitmap($"Resources\\Cars\\car-{participant.TeamColor}.png");
                g.DrawImage(carImg, corners);
                if (participant.Equipment.IsBroken)
                    g.DrawImage(Generator.GetBitmap("Resources\\Cars\\car-destroyed.png"), corners);
                if(isCorner)
                    tempimage = RotateImage(tempimage, currentXPos / 100f * 90f * (section.SectionType == SectionTypes.LeftCorner ? -1 : 1));
            }
            y += TrackWidth;
        }
        
        Bitmap test = new Bitmap(original);
        Graphics g2 = Graphics.FromImage(test);
        g2.DrawImage(tempimage, 0, 0);
        return test;
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