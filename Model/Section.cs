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

