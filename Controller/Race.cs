using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
            _timer = new Timer(250);
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
                Car car = new Car(_random.Next(1, 10), _random.Next(4, 10), _random.Next(7, 10));
                participant.Equipment = car;
            }
        }

        private void RandomizeStartPositions(Track track, List<IParticipant> participants)
        {
            List<IParticipant> participantsCopy = new List<IParticipant>(participants);
            for (LinkedListNode<Section> section = track.Sections.Last;
                 section != null && participantsCopy.Count > 0;
                 section = section.Previous)
            {
                if (participantsCopy.Count == 0)
                    break;
                if (section.Value.SectionType != SectionTypes.StartGrid)
                    continue;
                int randomIndex = _random.Next(participantsCopy.Count);
                IParticipant randomParticipant = participantsCopy[randomIndex];
                SectionData sectionData = GetSectionData(section.ValueRef);
                sectionData.AddParticipant(randomParticipant, section, 0, _random);
                participantsCopy.RemoveAt(randomIndex); 
                if(participantsCopy.Count == 0)
                    break;
                randomIndex = _random.Next(participantsCopy.Count);
                IParticipant randomParticipant1 = participantsCopy[randomIndex];
                sectionData.AddParticipant(randomParticipant1, section, 0, _random);
                participantsCopy.RemoveAt(randomIndex);
            }
        }
        
        protected virtual void OnTimedEvent(object source, EventArgs e)
        {
            foreach (IParticipant participant in Participants)
            {
                LinkedListNode<Section> currentSection = participant.CurrentSection;
                int distanceTraveled = participant.Equipment.GetDistanceTraveled();
                SectionData sectionData = GetSectionData(currentSection.ValueRef);
                int newPosition = distanceTraveled + sectionData.GetParticipantPosition(participant);
                bool toNext = newPosition >= Section.SectionLength;
                if (toNext)
                {
                    do
                    {
                        participant.CurrentSection = currentSection.Next ?? currentSection.List.First;
                    } while (GetSectionData(participant.CurrentSection.ValueRef).IsFull);
                    // participant.CurrentSection = currentSection.Next ?? currentSection.List.First;
                    newPosition -= Section.SectionLength;
                    sectionData.RemoveParticipant(participant);
                    SectionData sectionDataNew = GetSectionData(participant.CurrentSection.ValueRef);
                    sectionDataNew.AddParticipant(participant, participant.CurrentSection, newPosition, _random);
                }
                else
                {
                    sectionData.MoveParticipant(participant, distanceTraveled);
                }
            }
            DriversChanged.Invoke(this, new DriversChangedEventArgs() {Track = this.Track});
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

