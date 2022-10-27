using System;
using System.ComponentModel;

namespace Model {
    public interface IEquipment: INotifyPropertyChanged {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
        public int GetDistanceTraveled();
        public void WearAndTear(Random random);
    }
}

