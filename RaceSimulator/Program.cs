using Controller;
using RaceSimulator;

Data.Initialize();

Data.NextRace();
Data.CurrentRace.Start();

for (; ; )
{
    Thread.Sleep(100);
}