using System;
namespace Model
{
    public class SectionData
    {
        #region Properties

        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }

        #endregion

        #region Constructors

        public SectionData(IParticipant left, IParticipant right, int distanceLeft, int distanceRight)
        {
            Left = left;
            Right = right;
            DistanceLeft = distanceLeft;
            DistanceRight = distanceRight;
        }

        #endregion
    }
}

