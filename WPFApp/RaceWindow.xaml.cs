using System.Windows;

namespace WPFApp;

public partial class RaceWindow : Window
{
    public RaceWindow()
    {
        InitializeComponent();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        this.Hide();
    }
}