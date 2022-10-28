using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace Controller;

public class CompetitionDataContext: INotifyPropertyChanged
{
    public BindingList<string> Tracks => new (Data.Competition.Tracks.Select(p => p.Name).ToList());
    public BindingList<IParticipant> Drivers => new (Data.Competition.Participants.OrderByDescending(p => p.Points).ToList());
    
    public CompetitionDataContext() => Data.CurrentRace.RaceChanged += OnRaceChanged;

    private void OnRaceChanged(Race oldRace, Race? newRace)
    {
        oldRace.RaceChanged -= OnRaceChanged;
        if(newRace is not null)
            newRace.RaceChanged += OnRaceChanged;
        OnPropertyChanged(nameof(Drivers));
        OnPropertyChanged(nameof(Tracks));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}