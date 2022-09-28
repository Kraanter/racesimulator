using System;
using Model;
namespace Controller
{
    public class Race
    {
        #region Attributes
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
            _positions = new Dictionary<Section, SectionData>();

            RandomizeEquipment();
            RandomizeStartPositions(Track, Participants);
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

        private void RandomizeStartPositions(Track track, List<IParticipant> participants)
        {
            List<IParticipant> participantsCopy = new List<IParticipant>(participants);
            foreach (Section section in track.Sections)
            {
                if (section.SectionType != SectionTypes.StartGrid)
                    continue;
                if (participantsCopy.Count == 0)
                    return;
                SectionData data = GetSectionData(section);
                if (participantsCopy.Count > 0)
                {
                    data.Left = participantsCopy[_random.Next(participantsCopy.Count)];
                    participantsCopy.Remove(data.Left);
                }

                if (participantsCopy.Count > 0)
                {
                    data.Right = participantsCopy[_random.Next(participantsCopy.Count)];
                    participantsCopy.Remove(data.Right);
                }
            }
        }

        public override string ToString()
        {
            return this.Track.Name;
        }

        #endregion
    }
}

