using System;
using System.Linq;
using Controller;
using Model;

namespace RaceSimulator;

public static class Visualisation
{
    public static ConsoleColor BackgroundColor = ConsoleColor.Black;
    public static ConsoleColor ForegroundColor = ConsoleColor.White;
    public static ConsoleColor PlayerColor = ConsoleColor.Black;
    private static int minX = 0;
    private static int minY = 0;
    
    #region Graphics
    
    private static string[] _finishH =
    {
        "─────┰─",
        "  L  ┊ ",
        "   R ┊ ",
        "─────┸─"
    };
    private static string[] _finishV =
    {
        "│ R   │",
        "│   L │",
        "┝┄┄┄┄┄┥",
        "│     │"
    };
    private static string[] _start = { 
        "───────", 
        "   L]  ", 
        " R]    ", 
        "───────" 
    };
    private static string[] _cornerTR = { 
        "──────┐", 
        "    L │", 
        "  R   │", 
        "┐     │" 
    };
    private static string[] _cornerBR = { 
        "┘     │", 
        "  R   │", 
        "    L │", 
        "──────┘" 
    };
    private static string[] _cornerBL = { 
        "│     └", 
        "│   L  ", 
        "│ R    ", 
        "└──────" 
    };
    private static string[] _cornerTL = { 
        "┌──────", 
        "│ R    ", 
        "│   L  ", 
        "│     ┌" 
    };
    private static string[] _straightH = { 
        "───────", 
        "  L    ", 
        "    R  ", 
        "───────" 
    };
    private static string[] _straightV = { 
        "│     │", 
        "│ L   │", 
        "│   R │", 
        "│     │" 
    };

    #endregion
    
    #region Methods

    public static void Initialize()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Data.CurrentRace.DriversChanged += OnDriversChanged;
        Data.CurrentRace.RaceChanged += OnRaceChanged;
    }

    public static void OnRaceChanged(Race oldRace, Race newRace)
    {
        oldRace.DriversChanged -= OnDriversChanged;
        oldRace.RaceChanged -= OnRaceChanged;
        DrawLeaderboard();
        if (newRace is null)
            return;
        Thread.Sleep(1000);
        Console.Clear();
        newRace.DriversChanged += OnDriversChanged;
        newRace.RaceChanged += OnRaceChanged;
    }

    public static void DrawLeaderboard()
    {
        Console.Clear();
        foreach (IParticipant participant in Data.Competition.Participants.OrderBy(p => p.Points).Reverse())
        {
            Console.WriteLine(participant);
        }
    }
    
    public static void DrawTrack(Track track)
    {
        Directions direction = Directions.Right;
        int yOff = 2;
        int xOff = 2;
        int x = Math.Abs(track.MinMaxCords.minX);
        int y = Math.Abs(track.MinMaxCords.minY);
        foreach (Section section in track.Sections)
        {
            DrawSection(section, direction, x * 7 + xOff, (y * 4) + yOff);
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
        Console.ResetColor();
        Console.SetCursorPosition(0, Console.WindowHeight - 2);
    }
    
    private static void DrawSection(Section section, Directions direction, int x, int y)
    {
        Console.BackgroundColor = BackgroundColor;
        Console.ForegroundColor = ForegroundColor;
        SectionData sectionData = Data.CurrentRace.GetSectionData(section);
        foreach(string sectionPart in GetSection(section.SectionType, direction))
        {
            Console.SetCursorPosition(x , y);
            DrawSectionPart(sectionPart, sectionData);
            y++;
        }
    }
    
    private static void DrawSectionPart(string part, SectionData sectionData)
    {
        foreach (char ch in part)
        {
            switch (ch)
            {
                case 'L':
                    _DrawParticipantPart(sectionData.Left);
                    break;
                case 'R':
                    _DrawParticipantPart(sectionData.Right);
                    break;
                default:
                    Console.Write(ch);
                    break;
            }
        }
    }

    private static void _DrawParticipantPart(IParticipant? participant)
    {
        char name = participant is null ? ' ' : (participant.Equipment.IsBroken ? '.' : participant.Name[0]);
        Console.BackgroundColor = participant?.GetConsoleColor() ?? BackgroundColor;
        Console.ForegroundColor = PlayerColor;
        Console.Write(name);
        Console.ForegroundColor = ForegroundColor;
        Console.BackgroundColor = BackgroundColor;
    }
    
    private static string[] GetSection(SectionTypes sectionType, Directions direction)
    {
        bool reverse = sectionType != SectionTypes.LeftCorner && sectionType != SectionTypes.RightCorner && 
                       (direction == Directions.Down || direction == Directions.Left);
        bool vertical = direction == Directions.Up || direction == Directions.Down;
        string[] section = _GetSection(sectionType, direction, vertical);
        if (reverse)
            if (vertical) section = section.Reverse().ToArray();
            else section = section.Select(s => new string(s.Reverse().ToArray())).ToArray();
        return section;
    }

    private static string[] _GetSection(SectionTypes sectionType, Directions direction, bool vertical)
    {
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
        throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
    }

    public static void OnDriversChanged(object? obj, Race.DriversChangedEventArgs args)
    {
        DrawTrack(args.Track);
    }

    #endregion
}
