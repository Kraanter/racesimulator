using System;
using System.Diagnostics;

namespace Model
{
    public class Track
    {
        #region Properties

        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public (int minX, int minY, int maxX, int maxY) MinMaxCords { get; set; }
        public int Laps { get; set; }

        #endregion

        #region Constructors

        public Track(string name, int laps, SectionTypes[] sections)
        {
            if(sections[0] != SectionTypes.StartGrid) throw new ArgumentException("First section must be a start grid");
            if(sections[1] != SectionTypes.StartGrid) throw new ArgumentException("Second section must be a start grid");
            if(sections[2] != SectionTypes.Finish) throw new ArgumentException("Third section must be a Finish line");
            if(laps < 2) throw new ArgumentException("Laps must be at least 2");
            Name = name;
            Laps = laps;
            Sections = SectionTypesToSections(sections);
            GetMinMaxCords();
        }

        #endregion
        
        #region Methods
        
        private LinkedList<Section> SectionTypesToSections(SectionTypes[] sections)
        {
            var result = new LinkedList<Section>();
            foreach (var section in sections)
            {
                result.AddLast(new Section(section));
            }
            return result;
        }

        private void GetMinMaxCords()
        {
            int minX = 0; int minY = 0; int maxX = 0; int maxY = 0; int x = 0; int y = 0; 
            Directions direction = Directions.Right;
            foreach (var section in Sections)
            {
                SectionTypes sectionType = section.SectionType;
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
                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
            }
            MinMaxCords = (minX, minY, maxX, maxY);
        }
        
      
        #endregion
    }
}

public enum Directions
{
    Right,
    Down,
    Left,
    Up
}
