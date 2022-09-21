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

        #endregion

        #region Methods

        public static void Initialize()
        {
            Data.Competition = new Competition();

            Data.AddParticipants();
            Data.AddTracks();
        }

        //public static void AddParticipants(List<IParticipant> participants)
        public static void AddParticipants()
        {
            //participants.ForEach(participant => Data.Competition.Participants.Add(participant));
            Car car = new Car(10, 10, 10, false);
            Data.Competition.Participants.Add(new Driver("Carlos Sainz jr.", 0, car, TeamColors.Red));
            Data.Competition.Participants.Add(new Driver("Lewis Hamilton", 0, car, TeamColors.Green));
            Data.Competition.Participants.Add(new Driver("Max Verstappen", 0, car, TeamColors.Blue));
        }

        //public static void AddTracks(List<Track> tracks)
        public static void AddTracks()
        {
            //tracks.ForEach(track => Data.Competition.Tracks.Append(track));
            SectionTypes[] sections = new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.Finish
            };
            Data.Competition.Tracks.Enqueue(new Track("Monza", sections));
            Data.Competition.Tracks.Enqueue(new Track("Zandvoort", sections));
            Data.Competition.Tracks.Enqueue(new Track("Spa Francorchamps", sections));
        }

        #endregion
    }
}

