using Controller;
using Model;

Data.Initialize();

Race test = new Race(Data.Competition.Tracks.Dequeue(), Data.Competition.Participants);