using System;
namespace Model {
    public class Section {
        #region Properties

        public static int SectionLength = 100;
        public SectionTypes SectionType { get; set; }

        #endregion

        #region Constructors

        public Section(SectionTypes sectionType) {
            SectionType = sectionType;
        }

        #endregion
        
        #region Methods

        public override bool Equals(object? obj)
        {
            if (obj is SectionTypes objSectionType)
                return SectionType.Equals(objSectionType);
            if (obj is Section objSection)
                return SectionType.Equals(objSection.SectionType);
            return false;
        }
        public Directions GetNextDirection(Directions direction)
        {
            switch (SectionType)
            {
                case SectionTypes.StartGrid:
                    return Directions.Right;
                case SectionTypes.Finish:
                case SectionTypes.Straight:
                    return direction;
                case SectionTypes.RightCorner:
                    return (Directions) (((int) direction + 1) % 4);
                case SectionTypes.LeftCorner:
                    return (Directions) (((int) direction + 3) % 4);
                default:
                    throw new ArgumentOutOfRangeException(nameof(SectionType), SectionType, null);
            }
        }
  
        #endregion
    }

    public enum SectionTypes {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }
}

