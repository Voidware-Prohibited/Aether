using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using static TargetVectorLauncher.Views.Home;
using static TargetVectorLauncher.Views.Preferences;
using static TargetVectorLauncher.Views.About;
using static TargetVectorLauncher.Views.News;
using static TargetVectorLauncher.Views.Community;
using static TargetVectorLauncher.Views.Downloads;
using System.ComponentModel;


namespace TargetVectorLauncher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Closing += OnClosing;
    }
    
    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        // Implement your closing logic here
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