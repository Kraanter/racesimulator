using Controller;
using RaceSimulator;

Data.Initialize();

Data.NextRace();
Visualisation.Initialize();
Data.CurrentRace.Start();

for (; ; )
{
    Thread.Sleep(100);
}