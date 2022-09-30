using System;
namespace Model {
    public class Driver : IParticipant {
        #region Properties

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public LinkedListNode<Section> CurrentSection { get; set; }

        #endregion

        #region Constructors

        public Driver(string name, TeamColors teamColor)
        {
            Name = name;
            Points = 0;
            TeamColor = teamColor;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{Name} | Points: {Points} | {Equipment}";
        }

        #endregion
    }
}

