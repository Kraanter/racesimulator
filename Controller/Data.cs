using Model;

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
            Data.NextRace();
        }

        public static void AddParticipants()
        {
            Data.Competition.Participants.Add(new Driver("Carlos Sainz jr.", TeamColors.Red));
            Data.Competition.Participants.Add(new Driver("Lewis Hamilton", TeamColors.Green));
            Data.Competition.Participants.Add(new Driver("Max Verstappen", TeamColors.Blue));
        }

        public static void AddTracks()
        {
            SectionTypes[] Monza = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
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
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,
            };
            SectionTypes[] Zandvoort = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Data.Competition.Tracks.Enqueue(new Track("Monza", 2, Zandvoort));
            Data.Competition.Tracks.Enqueue(new Track("Zandvoort", 2, Monza));
            Data.Competition.Tracks.Enqueue(new Track("Spa Francorchamps", 2, Zandvoort));
        }

        public static void NextRace()
        {
            Track newTrack = Data.Competition.NextTrack();
            if (newTrack == null)
            {
                CurrentRace = null;
                return;
            }
            CurrentRace = new Race(newTrack, Data.Competition.Participants);
            CurrentRace.Start();
        }

        #endregion
    }
}

