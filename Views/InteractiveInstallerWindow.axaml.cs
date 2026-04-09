using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace TargetVectorLauncher.Views;

public partial class InteractiveInstallWindow : Window
{
    public InteractiveInstallWindow()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }

    private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // Prevent the user from closing the window during an update
        e.Cancel = true;
    }

    public void UpdateProgress(double percentage)
    {
        this.Get<ProgressBar>("DownloadProgressBar").Value = percentage;
        this.Get<TextBlock>("ProgressText").Text = $"{percentage:F0}% Complete";
    }
}