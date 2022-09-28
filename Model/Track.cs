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
            if(sections[0] != SectionTypes.StartGrid) throw new ArgumentException("First section must be a start grid");
            Name = name;
            Sections = SectionTypesToSections(sections);
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
        
        #endregion
    }
}

