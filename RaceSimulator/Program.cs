using Controller;
using RaceSimulator;

Data.Initialize();

Data.NextRace();

Visualisation.DrawTrack(Data.CurrentRace.Track);

for (; ; )
{
    Thread.Sleep(100);
}