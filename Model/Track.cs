using System;
namespace Model
{
    public class Track
    {
        #region Properties

        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        #endregion

        #region Constructors

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = new LinkedList<Section>();
            foreach (var section in sections)
            {
                Sections.Append(new Section(section));
            }
        }

        #endregion
    }
}

