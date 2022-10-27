using System;
using System.ComponentModel;

namespace Model {
    public interface IParticipant: INotifyPropertyChanged {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public LinkedListNode<Section> CurrentSection { get; set; }
        public ConsoleColor GetConsoleColor();
        public int Laps { get; set; }
        public string ImagePath { get; set; }
    }

    public enum TeamColors {
        Red, Green, Yellow, Grey, Blue
    }
}

