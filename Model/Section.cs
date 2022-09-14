using System;
namespace Model {
    public class Section {
        public SectionTypes SectionType { get; set; }

        public Section() {
        }
    }

    public enum SectionTypes {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }
}

