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
        public MainWindow()
        {
            InitializeComponent();
            Data.Initialize();
            Display.Initialize();

            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.RaceChanged += OnRaceChanged;

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
    }
}