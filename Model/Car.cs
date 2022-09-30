using System;
namespace Model {
    public class Car : IEquipment {

        #region Properties

        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        #endregion

        #region Constructors

        public Car(int quality, int performance, int speed) {
            Quality = quality;
            Performance = performance;
            Speed = speed;
            IsBroken = false;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Car[ Quality: {Quality}, Performance: {Performance}, Speed: {Speed}, IsBroken: {IsBroken}]";
        }
        
        public int GetDistanceTraveled()
        {
            return Performance * Speed;
        }

        #endregion
    }
}

