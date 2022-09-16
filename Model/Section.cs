using System;
namespace Model {
    public class Section {
        #region Properties

        public SectionTypes SectionType { get; set; }

        #endregion

        #region Constructors

        public Section(SectionTypes sectionType) {
            SectionType = sectionType;
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

