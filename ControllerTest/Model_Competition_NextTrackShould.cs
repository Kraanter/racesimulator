using System;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        #region Attributes

        private Competition _competition;

        #endregion

        #region Methods

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("test", new SectionTypes[] {SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.RightCorner});
            _competition.Tracks.Enqueue(track);
            Track result = _competition.NextTrack();
            Assert.That(track, Is.EqualTo(result));
        }

        #endregion
    }
}

