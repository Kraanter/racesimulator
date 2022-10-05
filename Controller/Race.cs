using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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
        private int _numOfLaps;
        private Queue<IParticipant> _finished;
        private int count = 0;

        #endregion

        #region Properties

        public Track Track { get; }
        public List<IParticipant> Participants { get; private set; }
        public DateTime StartTime { get; set; }

        #endregion

        #region Constructors

        public Race(Track track, int numOfLaps, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _finished = new Queue<IParticipant>();
            // todo: Random is niet random hoort DateTime.Now.Millisecond
            _random = new Random(69);
            _positions = new Dictionary<Section, SectionData>();
            _timer = new Timer(250);
            _timer.Elapsed += OnTimedEvent;
            _numOfLaps = numOfLaps + 1;

            RandomizeEquipment();
            RandomizeStartPositions(track, participants);
        }

        #endregion

        #region Methods

        public SectionData GetSectionData(Section section)
        {
            count++;
            if (_positions.TryGetValue(section, out SectionData sec))
                return sec;
            else
            {
                _positions.Add(section, new SectionData());
                return _positions[section];
            }
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
            if (_finished.Count == Participants.Count)
            {
                End();
                return;
            }

            _timer.Stop();
            foreach (IParticipant participant in Participants)
            {
                if (participant.Laps == _numOfLaps) continue;
                LinkedListNode<Section> currentSection = participant.CurrentSection;
                int distanceTraveled = participant.Equipment.GetDistanceTraveled();
                SectionData sectionData = GetSectionData(currentSection.ValueRef);
                Debug.WriteLine(
                    $"{participant.Name} => {sectionData} => {sectionData.GetParticipantPosition(participant)}");
                int newPosition = distanceTraveled + sectionData.GetParticipantPosition(participant);
                bool toNext = newPosition >= Section.SectionLength;
                if (toNext)
                {
                    participant.CurrentSection = currentSection.Next ?? currentSection.List.First;
                    while (GetSectionData(participant.CurrentSection.ValueRef).IsFull())
                    {
                        participant.CurrentSection = participant.CurrentSection.Next ?? currentSection.List.First;
                    }

                    ;
                    sectionData.RemoveParticipant(participant);
                    if (currentSection.Value.SectionType == SectionTypes.Finish)
                    {
                        participant.Laps++;
                        if (participant.Laps == _numOfLaps)
                        {
                            participant.Points = Participants.Count - _finished.Count;
                            _finished.Enqueue(participant);
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

            DriversChanged.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
            _timer.Start();
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public void End()
        {
            _timer.Enabled = false;
            Thread.Sleep(500);
            Debug.WriteLine("COUNT " + _finished.Count);
            Console.Clear();
            for (IParticipant participant = _finished.Dequeue();
                 _finished.Count >= 0;
                 participant = _finished.Dequeue())
            {
                Console.WriteLine(participant);
            }
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