using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Model;
using Timer = System.Timers.Timer;

namespace Controller
{
    public delegate void RaceChangedDelegate(Race oldRace, Race newRace);
    public class Race: INotifyPropertyChanged
    {
        #region Attributes

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;
        private int _numOfLaps;
        private Queue<IParticipant> _finished;
        private int count = 0;
        private ObservableCollection<IParticipant> _leaderboard;

        #endregion

        #region Properties

        public Track Track { get; }
        public List<IParticipant> Participants { get; private set; }
        public DateTime StartTime { get; set; }

        public ObservableCollection<IParticipant> Leaderboard
        {
            get => _leaderboard;
            set
            {
                _leaderboard = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _finished = new Queue<IParticipant>();
            // todo: Random is niet random hoort DateTime.Now.Millisecond
            _random = new Random(69);
            _positions = new Dictionary<Section, SectionData>();
            _timer = new Timer(250);
            _timer.Elapsed += OnTimedEvent;
            _numOfLaps = track.Laps + 1;
            _leaderboard = new ObservableCollection<IParticipant>(participants);

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
                Car car = new Car(_random.Next(1, 20), _random.Next(4, 10), _random.Next(7, 10));
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
                    IParticipant randomParticipant = DropRandomItemFromList(participantsCopy);
                    SectionData sectionData = GetSectionData(section.ValueRef);
                    sectionData.AddParticipant(randomParticipant, section, 0, _random);
                } while (participantsCopy.Count % 2 != startCount && participantsCopy.Count != 0);
            }
        }

        private T DropRandomItemFromList<T>(List<T> list)
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
                participant.Equipment.WearAndTear(_random);
                if(participant.Equipment.IsBroken)
                    continue;
                int distanceTraveled = participant.Equipment.GetDistanceTraveled();
                SectionData sectionData = GetSectionData(participant.CurrentSection.ValueRef);
                int newPosition = distanceTraveled + sectionData.GetParticipantPosition(participant);
                bool toNext = newPosition >= Section.SectionLength;
                if (toNext)
                {
                    do 
                    {
                        if (participant.CurrentSection.Value.SectionType == SectionTypes.Finish)
                        {
                            participant.Laps++;
                        }
                        
                        participant.CurrentSection = participant.CurrentSection.Next ?? participant.CurrentSection.List.First;
                    } while (GetSectionData(participant.CurrentSection.ValueRef).IsFull());
                    sectionData.RemoveParticipant(participant);
                    if (participant.Laps == _numOfLaps)
                    {
                        participant.Points += Participants.Count - _finished.Count;
                        _finished.Enqueue(participant);
                        continue;
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

            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
            _timer.Start();

            Leaderboard = GetParticipantsLeaderboard();
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            _timer.Start();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = Track });
        }
        
        public ObservableCollection<IParticipant> GetParticipantsLeaderboard()
        {
            List<(IParticipant, int)> participants = new List<(IParticipant, int)>();
            int i = 0;
            foreach (Section section in Track.Sections)
            {
                SectionData sectionData = GetSectionData(section);
                foreach (IParticipant? participant in sectionData.GetDrivers())
                    if(participant is not null)
                        participants.Add((participant, i));
                i++;
            }
            
            List<IParticipant> sortedParticipants = participants
                .OrderBy(x => x.Item1.Laps)
                .ThenBy(x => x.Item2)
                .Select(x => x.Item1)
                .ToList();
           sortedParticipants.ForEach(a => a.CurrentPosition = sortedParticipants.IndexOf(a) + 1); 
            return new ObservableCollection<IParticipant>(sortedParticipants);
        }

        public void End()
        {
            _timer.Enabled = false;
            while (_finished.Count > 0)
            {
                IParticipant participant = _finished.Dequeue();
                participant.Laps = 0;
            }
            Data.NextRace();
            RaceChanged.Invoke(this, Data.CurrentRace);
        }

        public event RaceChangedDelegate RaceChanged;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}