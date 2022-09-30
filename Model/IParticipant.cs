using System;
namespace Model {
    public interface IParticipant {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public LinkedListNode<Section> CurrentSection { get; set; }
        public ConsoleColor GetConsoleColor();
    }

    public enum TeamColors {
        Red, Green, Yellow, Grey, Blue
    }
}

