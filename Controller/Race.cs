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
        public List<IParticipant> Participants { get; private set; }
        public DateTime StartTime { get; set; }

        #endregion

        #region Constructors

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            // todo: Random is niet random hoort DateTime.Now.Millisecond
            _random = new Random(69);
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
                int startCount = participantsCopy.Count % 2;
                do
                {
                    IParticipant randomParticipant = GetRandomItem(participantsCopy);
                    SectionData sectionData = GetSectionData(section.ValueRef);
                    sectionData.AddParticipant(randomParticipant, section, 0, _random);
                } while (participantsCopy.Count % 2 != startCount && participantsCopy.Count != 0);
            }
        }

        private T GetRandomItem<T>(List<T> list)
        {
            int randomIndex = _random.Next(list.Count);
            T randomItem = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomItem;
        }
        
        protected virtual void OnTimedEvent(object source, EventArgs e)
        {
            foreach (IParticipant participant in Participants)
            {
                if(participant.Laps == 4) continue;
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
                    sectionData.RemoveParticipant(participant);
                    if (currentSection.Value.SectionType == SectionTypes.Finish)
                    {
                        participant.Laps++;
                        if (participant.Laps == 4)
                        {
                            continue;
                        }
                    }
                    newPosition -= Section.SectionLength;
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

