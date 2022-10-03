using System;
namespace Model {
    public class Driver : IParticipant {
        #region Properties

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public LinkedListNode<Section> CurrentSection { get; set; }
        public  int Laps { get; set; }

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

        public override string ToString()
        {
            return $"{Name} | Points: {Points} | {Equipment}";
        }

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

        #endregion
    }
}

