using System;
using Model;
namespace Controller
{
    public class Race
    {
        #region Attribute
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        #endregion

        #region Properties

        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        #endregion

        #region Constructors

        public Race(Track Track, List<IParticipant> Participants)
        {
            this.Track = Track;
            this.Participants = Participants;
            _random = new Random(DateTime.Now.Millisecond);

            RandomizeEquipment();
        }

        #endregion

        #region Methods

        private SectionData GetSectionData (Section section)
        {
            if (_positions.ContainsKey(section))
                return _positions[section];
            SectionData newSecData = new SectionData();
            _positions.Add(section, newSecData);
            return newSecData;
        }

        private void RandomizeEquipment()
        {
            foreach (var participant in this.Participants)
            {
                participant.Equipment.Quality = _random.Next(50);
                participant.Equipment.Speed = _random.Next(50);
            }
        }

        #endregion
    }
}

