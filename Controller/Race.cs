using Model;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        #region Attributes
        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;
        #endregion

        #region Properties

        public Track Track { get; }
        public List<IParticipant> Participants { get; }
        public DateTime StartTime { get; set; }

        #endregion

        #region Constructors

        public Race(Track track, List<IParticipant> participants)
        {
            this.Track = track;
            this.Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;

            RandomizeEquipment();
            RandomizeStartPositions(track, participants);
        }

        #endregion

        #region Methods

        public SectionData GetSectionData (Section section)
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
            foreach (Section section in track.Sections.Reverse())
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
        
        protected virtual void OnTimedEvent(object source, EventArgs e)
        {

        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public event EventHandler<DriversChangedEventArgs> DriversChanged;

        public override string ToString()
        {
            return this.Track.Name;
        }

        #endregion
        
        public class DriversChangedEventArgs : EventArgs
        {
            public Track Track { get; set; }
        }
    }
}

