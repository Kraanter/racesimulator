using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model {
    public class Driver : IParticipant {
        #region Properties

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public LinkedListNode<Section> CurrentSection { get; set; }
        public  int Laps { get; set; }

        public string ImagePath
        {
            get => "Resources\\Cars\\car-" + TeamColor + ".png";
            set => ImagePath = value;
        }

        #endregion

        #region Constructors

        public Driver(string name, TeamColors teamColor)
        {
            Name = name;
            Points = 0;
            TeamColor = teamColor;
            Laps = 0;
        }

        #endregion

        #region Methods
        public ConsoleColor GetConsoleColor()
        {
            return TeamColor switch
            {
                TeamColors.Red => ConsoleColor.Red,
                TeamColors.Blue => ConsoleColor.Blue,
                TeamColors.Green => ConsoleColor.Green,
                TeamColors.Yellow => ConsoleColor.Yellow,
                TeamColors.Grey => ConsoleColor.Gray,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public override string ToString() => Name;

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

