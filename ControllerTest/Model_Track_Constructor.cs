using Controller;
using Model;

namespace ControllerTest;

[TestFixture]
public class Model_Track_Constructor
{
   private Race _testRace { get; set; }
   private Track track { get; set; }
   private List<IParticipant> Participants { get; set; }
   
   
    [Test]
    public void GetSectionData_ReturnsData()
      {
         track = new Track("test", 2,new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish });
         Participants = new List<IParticipant>();
         Participants.Add(new Driver("Lola", TeamColors.Blue));
         Participants.Add(new Driver("Thijmen", TeamColors.Yellow));
         Participants.Add(new Driver("Joost", TeamColors.Red));
         _testRace = new Race(track, Participants);
         Section section = _testRace.Track.Sections.First.ValueRef;
         SectionData data = _testRace.GetSectionData(section);
         Assert.NotNull(data); 
      }
      [Test]
      public void ParticipantsCount_3OrMore_ShouldPass()
      {
         try
         {
            track = new Track("test", 2, new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish });
            Participants = new List<IParticipant>();
            Participants.Add(new Driver("Lola", TeamColors.Blue));
            Participants.Add(new Driver("Thijmen", TeamColors.Yellow));
            Participants.Add(new Driver("Tom", TeamColors.Red));
            _testRace = new Race(track, Participants);
         }
         catch (Exception)
         {
            Assert.Fail("Exception thrown");
         }
      }

      [Test]
      public void ParticipantsCount_LessThan3_ReturnException()
      {
         try
         {
            track = new Track("test", 2,new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish });
            Participants = new List<IParticipant>();
            Participants.Add(new Driver("Lola", TeamColors.Blue));
            Participants.Add(new Driver("Thijmen", TeamColors.Yellow));
            _testRace = new Race(track, Participants);
         }
         catch(System.ArgumentException e)
         {
            StringAssert.Contains(e.Message, "There must be 3 participants");
            return;
         }
         Assert.Fail("No Exception thrown");
      }
}