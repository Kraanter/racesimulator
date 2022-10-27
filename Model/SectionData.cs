using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class SectionData : INotifyPropertyChanged
    {
        #region Properties

        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }
        public bool IsFull() => Right is not null && Left is not null;
        #endregion

        #region Methods
        
        public void AddParticipant(IParticipant participant, LinkedListNode<Section> currentSection, int distance, Random random)
        {
            if (IsFull()) return;
            if (Left == null && random.Next(2) == 1 || Right != null)
            {
                Left = participant;
                DistanceLeft = distance;
            }
            else if (Right == null)
            {
                Right = participant;
                DistanceRight = distance;
            }
            participant.CurrentSection = currentSection;
            OnPropertyChanged();
        }
        
        public void MoveParticipant(IParticipant participant, int distance)
        {
            if (participant == Left)
            {
                DistanceLeft += distance;
            }
            else if (participant == Right)
            {
                DistanceRight += distance;
            }
            OnPropertyChanged();
        }
        
        public void RemoveParticipant(IParticipant participant)
        {
            if (Left == participant)
            {
                Left = null;
                DistanceLeft = 0;
                // IsFull = false;
            }
            else if (Right == participant)
            {
                Right = null;
                DistanceRight = 0;
                // IsFull = false;
            }
            OnPropertyChanged();
        }
        
        public int GetParticipantPosition(IParticipant participant)
        {
            if (Left == participant)
                return DistanceLeft;
            else if (Right == participant)
                return DistanceRight;
            return 0;
        }

        public override string ToString()
        {
            return $"Left: {Left?.Name} {DistanceLeft}m, Right: {Right?.Name} {DistanceRight}m";
        }
        
        public IParticipant?[] GetDrivers()
        {
            return new []{Left, Right};
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

