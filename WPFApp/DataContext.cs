using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller;
using Model;

namespace WPFApp;

public class DataContext : INotifyPropertyChanged, INotifyCollectionChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public string TrackName => Data.CurrentRace.Track.Name;
    public string TrackLength => Data.Competition.Tracks.Count.ToString();
    public string TrackLaps => Data.CurrentRace.Track.Laps.ToString();
    public ObservableCollection<IParticipant> TrackLeaderboard => Data.CurrentRace.Leaderboard;

    public ObservableCollection<IParticipant> Participants =>
        new (Data.CurrentRace.Participants.OrderByDescending(x => x.CurrentPosition).ToList());


    #region Constructor

    public DataContext()
    {
        Data.CurrentRace.DriversChanged += OnDriversChanged;
        Data.CurrentRace.RaceChanged += OnRaceChanged;
    }

    #endregion
    
    public void OnDriversChanged(object? obj, Race.DriversChangedEventArgs args)
    {
        // PropertyChanged?.Invoke(this, new (""));
        // CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    public void OnRaceChanged(Race oldRace, Race? newRace)
    {
        oldRace.DriversChanged -= OnDriversChanged;
        oldRace.RaceChanged -= OnRaceChanged;
        PropertyChanged?.Invoke(this, new (""));
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        if (newRace is null)
            return;
        oldRace.DriversChanged += OnDriversChanged;
        oldRace.RaceChanged += OnRaceChanged;
    }
}