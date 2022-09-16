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

        public Competition(List<IParticipant> participants, Queue<Track> tracks)
        {
            Participants = participants;
            Tracks = tracks;
        }

        #endregion

        #region Methods

        // todo: Create method NextTrack
        public Track NextTrack()
        {

        }

        #endregion
    }
}

