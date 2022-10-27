using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller;
using Model;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _raceWindow;
        private Window _competitionWindow;
        public MainWindow()
        {
            Data.Initialize();
            Display.Initialize();

            _raceWindow = new RaceWindow();
            _competitionWindow = new CompetitionWindow();
            
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.RaceChanged += OnRaceChanged;
            InitializeComponent();
        }


        public void OnDriversChanged(object? obj, Race.DriversChangedEventArgs args)
        {
            this.TrackScreen.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    TrackScreen.Source = null;
                    TrackScreen.Source = Display.DrawTrack(args.Track);
                }));
        }
        public void OnRaceChanged(Race oldRace, Race? newRace)
        {
            Generator.Clear();
            oldRace.DriversChanged -= OnDriversChanged;
            oldRace.RaceChanged -= OnRaceChanged;
            if (newRace is null)
                return;
            Thread.Sleep(1000);
            newRace.DriversChanged += OnDriversChanged;
            newRace.RaceChanged += OnRaceChanged;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem item))
                return;
            Window? window = item.Name switch
            {
                "Race" => _raceWindow,
                "Competition" => _competitionWindow,
                _ => null
            };

            if (window is null)
            {
                switch (item.Name)
                {
                    case "Close":
                        Application.Current.Shutdown();
                        break;
                    default:
                        MessageBox.Show($"{item.Name} is not implemented");
                        break;
                }
                return;
            }
            window.Show();
            window.Focus();
        }
    }
}