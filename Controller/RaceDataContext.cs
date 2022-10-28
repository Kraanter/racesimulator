using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller;
using Model;

namespace Controller;

public class RaceDataContext : INotifyPropertyChanged 
{
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public string TrackName => Data.CurrentRace.Track.Name;
    public string TotalTrackNum => Data.Competition.Tracks.Count.ToString();
    public string TrackLaps => Data.CurrentRace.Track.Laps.ToString();
    public string TrackNum => (Data.Competition.Tracks.ToList().FindIndex(i => i.Name == TrackName) + 1).ToString();
    public ObservableCollection<IParticipant> TrackLeaderboard => Data.CurrentRace.Leaderboard;

    public ObservableCollection<IParticipant> Participants =>
        new(Data.CurrentRace.Participants.OrderByDescending(x => x.CurrentPosition).ToList());

    public RaceDataContext()
    {
        Data.CurrentRace.RaceChanged += OnRaceChanged;
    }
    
    private void OnRaceChanged(Race oldRace, Race? newRace)
    {
        oldRace.RaceChanged -= OnRaceChanged;
        if(newRace is not null)
            newRace.RaceChanged += OnRaceChanged;
        OnPropertyChanged(nameof(TrackName));
        OnPropertyChanged(nameof(TrackLaps));
        OnPropertyChanged(nameof(TotalTrackNum));
        OnPropertyChanged(nameof(TrackNum));
        OnPropertyChanged(nameof(TrackLeaderboard));
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 