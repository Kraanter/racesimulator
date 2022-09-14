﻿using System;
namespace Model {
    public interface IParticipant {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
    }

    public enum TeamColors {
        Red, Green, Yellow, Grey, Blue
    }
}

