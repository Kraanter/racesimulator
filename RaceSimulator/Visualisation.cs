using Model;

namespace RaceSimulator;

public static class Visualisation
{
    private static int minX = 0;
    private static int minY = 0;
    
    #region Graphics
    
    private static string[] _finishH =
    {
        "────",
        "  ┆ ",
        "  ┆ ",
        "────"
    };
    private static string[] _finishV =
    {
        "│  │",
        "│  │",
        "│┄┄│",
        "│  │"
    };
    private static string[] _start = { 
        "────", 
        "  ] ", 
        "  ] ", 
        "────" 
    };
    private static string[] _cornerTR = { 
        "───┐", 
        "   │", 
        "   │", 
        "┐  │" 
    };
    private static string[] _cornerBR = { 
        "┘  │", 
        "   │", 
        "   │", 
        "───┘" 
    };
    private static string[] _cornerBL = { 
        "│  └", 
        "│   ", 
        "│   ", 
        "└───" 
    };
    private static string[] _cornerTL = { 
        "┌───", 
        "│   ", 
        "│   ", 
        "│  ┌" 
    };
    private static string[] _straightH = { 
        "────", 
        "    ", 
        "    ", 
        "────" 
    };
    private static string[] _straightV = { 
        "│  │", 
        "│  │", 
        "│  │", 
        "│  │" 
    };

    #endregion
    
    #region Methods

    public static void DrawTrack(Track track)
    {
        GetMinXY(track);
        Directions direction = Directions.Right;
        int yOff = Console.CursorTop + 1;
        int x = Math.Abs(minX);
        int y = Math.Abs(minY);
        
        foreach (Section section in track.Sections)
        {
            SectionTypes sectionType = section.SectionType;
            DrawSection(sectionType, direction, x * 4, (y * 4) + yOff);
            direction = getDirection(direction, sectionType);
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
        Console.CursorVisible = false;
        Console.ResetColor();
    }
    
    private static void DrawSection(SectionTypes sectionType, Directions direction, int x, int y)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
        
        foreach(string sectionPart in GetSection(sectionType, direction))
        {
            Console.SetCursorPosition(x , y);
            Console.Write(sectionPart);
            y++;
        }
    }

    private static void GetMinXY(Track track)
    {
        Directions direction = Directions.Right;
        int x = 0;
        int y = 0;

        foreach (Section section in track.Sections)
        {
            SectionTypes sectionType = section.SectionType;
            direction = getDirection(direction, sectionType);
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
            if(x < minX)
                minX = x;
            if (y < minY)
                minY = y;
        }
    }
    
    private static Directions getDirection(Directions direction, SectionTypes sectionType)
    {
        switch (sectionType)
        {
            case SectionTypes.StartGrid:
                return Directions.Right;
            case SectionTypes.Finish:
                return direction;
            case SectionTypes.Straight:
                return direction;
            case SectionTypes.RightCorner:
                return (Directions) (((int) direction + 1) % 4);
            case SectionTypes.LeftCorner:
                return (Directions) (((int) direction + 3) % 4);
            default:
                throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
        }
    }
    private static string[] GetSection(SectionTypes sectionType, Directions direction)
    {
        bool vertical = direction == Directions.Up || direction == Directions.Down;
        switch (sectionType)
        {
            case SectionTypes.StartGrid:
                return _start;
            case SectionTypes.Finish:
                return vertical ? _finishV : _finishH;
            case SectionTypes.Straight:
                return vertical ? _straightV : _straightH;
            case SectionTypes.RightCorner:
                switch (direction)
                {
                    case Directions.Up:
                        return _cornerTL;
                    case Directions.Right:
                        return _cornerTR;
                    case Directions.Down:
                        return _cornerBR;
                    case Directions.Left:
                        return _cornerBL;
                }
                break;
            case SectionTypes.LeftCorner:
                switch (direction)
                {
                    case Directions.Up:
                        return _cornerTR;
                    case Directions.Right:
                        return _cornerBR;
                    case Directions.Down:
                        return _cornerBL;
                    case Directions.Left:
                        return _cornerTL;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            default:
                throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
        }
        return null;
    }
    
    #endregion
}

public enum Directions
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}