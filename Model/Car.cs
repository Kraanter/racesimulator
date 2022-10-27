using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model {
    public class Car : IEquipment {

        #region Fields

        private bool _isBroken;
        
        #endregion
        
        #region Properties

        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }

        public bool IsBroken
        {
            get => _isBroken;
            set
            {
                _isBroken = value;
                OnPropertyChanged();
            }
        }

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

        public void WearAndTear(Random random)
        {
            if (IsBroken)
            {
                if (random.Next(0, Quality / 2) == 0)
                {
                    IsBroken = false;
                }
            }
            else
            {
                if (random.Next(0, Quality) == 0)
                {
                    IsBroken = true;
                }
            }
        }

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

