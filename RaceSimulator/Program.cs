using Controller;
using RaceSimulator;

Data.Initialize();

Data.NextRace(); 
Thread.Sleep(1000);
Data.CurrentRace.Start();

for (; ; )
{
    Thread.Sleep(100);
}