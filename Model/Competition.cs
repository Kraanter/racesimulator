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

        public Track NextTrack()
        {
            if (Tracks.Count < 1) return null;
            return Tracks.Dequeue();
        }

        #endregion
    }
}

