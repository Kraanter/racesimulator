using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using Model;
using static System.Collections.Specialized.BitVector32;

namespace Controller
{
    public static class Data
    {
        #region Properties

        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        #endregion

        #region Methods

        public static void Initialize()
        {
            Data.Competition = new Competition();

            Data.AddParticipants();
            Data.AddTracks();
        }

        public static void AddParticipants()
        {
            Car car = new Car(10, 10, 10, false);
            Data.Competition.Participants.Add(new Driver("Carlos Sainz jr.", 0, car, TeamColors.Red));
            Data.Competition.Participants.Add(new Driver("Lewis Hamilton", 0, car, TeamColors.Green));
            Data.Competition.Participants.Add(new Driver("Max Verstappen", 0, car, TeamColors.Blue));
        }

        public static void AddTracks()
        {
            SectionTypes[] sections = new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Finish
            };
            Data.Competition.Tracks.Enqueue(new Track("Monza", sections));
            Data.Competition.Tracks.Enqueue(new Track("Zandvoort", sections));
            Data.Competition.Tracks.Enqueue(new Track("Spa Francorchamps", sections));
        }

        public static void NextRace()
        {
            Track newTrack = Data.Competition.NextTrack();
            if (newTrack == null) return;
            CurrentRace = new Race(newTrack, Data.Competition.Participants);
        }

        #endregion
    }
}

