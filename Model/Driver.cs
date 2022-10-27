using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model {
    public class Driver : IParticipant {
        #region Fields
        
        private string _name;
        private int _points;
        private IEquipment _equipment;
        private LinkedListNode<Section> _currentSection;
        private int _laps;
        private int _position;
       
        #endregion
        
        #region Properties

        public int CurrentPosition {
            get => _position;
            set { _position = value; OnPropertyChanged();}
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        public IEquipment Equipment
        {
            get => _equipment;
            set
            {
                _equipment = value;
                OnPropertyChanged();
            }
        }

        public TeamColors TeamColor { get; set; }

        public LinkedListNode<Section> CurrentSection
        {
            get => _currentSection;
            set
            {
                _currentSection = value;
                OnPropertyChanged();
            }
        }

        public int Laps
        {
            get => _laps == 0 ? 1 : _laps;
            set
            {
                _laps = _laps == 0 ? 1 : value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => "Resources\\Cars\\car-" + TeamColor + ".png";
            set => ImagePath = value;
        }

        #endregion

        #region Constructors

        public Driver(string name, TeamColors teamColor)
        {
            _name = name;
            _points = 0;
            TeamColor = teamColor;
            _laps = 0;
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
    }
}

