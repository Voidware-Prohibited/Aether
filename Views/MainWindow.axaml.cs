using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using static TargetVectorLauncher.Views.Preferences;
using static TargetVectorLauncher.Views.About;


namespace TargetVectorLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    public void ShowPreferencesWindow(object sender, RoutedEventArgs args)
    {
        Preferences PreferencesWindow = new Preferences();
        PreferencesWindow.Show();
    }
    public void ShowAboutWindow(object sender, RoutedEventArgs args)
    {
        About AboutWindow = new About();
        AboutWindow.Show();
    }
}