using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace TargetVectorLauncher.Views;

public partial class Preferences : Window
{
    public Preferences()
    {
        InitializeComponent();
    }
    
    public void Close(object sender, RoutedEventArgs args)
    {
        this.Close();
    }
}