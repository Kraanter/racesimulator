using System.Windows;

namespace WPFApp;

public partial class CompetitionWindow : Window
{
    public CompetitionWindow()
    {
        InitializeComponent();
    }
    
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        this.Hide();
    }
}