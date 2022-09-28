using Controller;
using RaceSimulator;

Data.Initialize();

Data.NextRace();

for (; ; )
{
    Visualisation.DrawTrack(Data.CurrentRace.Track);
    Thread.Sleep(100);
}