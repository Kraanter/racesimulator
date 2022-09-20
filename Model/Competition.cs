using System;
namespace Model
{
    public class Competition
    {
        #region Properties

        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        #endregion

        #region Constructors

        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }

        #endregion

        #region Methods

        // todo: Create method NextTrack
        //public Track NextTrack()
        //{

        //}

        #endregion
    }
}

