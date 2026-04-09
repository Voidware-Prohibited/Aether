using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Interactivity;
using Avalonia.Threading;
using TargetVectorLauncher.ViewModels;
using NetSparkleUpdater;
using NetSparkleUpdater.Interfaces;
using System;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;

namespace TargetVectorLauncher.Views;

public partial class LoadingWindow : Window
{
    // private readonly Action? _mainAction;
    private readonly LoadingWindowModel _viewModel;
    
    public LoadingWindow()
    {
        InitializeComponent();
        // _mainAction = mainAction;
        _viewModel = new LoadingWindowModel();
        DataContext = _viewModel;
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    string base64PublicKey = "your_base_64_public_key_from_generate_app_cast_tool";
    
    public async Task CheckForUpdatesAsync()
    {
        try
        {
            var sparkle = new SparkleUpdater("https://example.com/updates.json", new Ed25519Checker(SecurityMode.Strict, base64PublicKey));
            sparkle.LogWriter = new NetSparkleUpdater.LogWriter();

            // Start the update process
            var result = await sparkle.CheckForUpdatesAtUserRequest();

            if (result.Status == UpdateStatus.UpdateAvailable)
            {
                // Update is available, prompt and install
                this.Get<TextBlock>("StatusText").Text = "Update found! Downloading...";

                // Create a minimal status window for download progress
                var statusWindow = new UpdateStatusWindow();
                statusWindow.Show();
                Close(); // Close the splash screen

                // Set up NetSparkle events for progress
               //  sparkle.UIFactory.ShowUI();
               // Use a dummy or a UI for local testing
               // UIFactory = new NetSparkleUpdater.UI.WinForms.UIFactory();
            }
            else
            {
                // No update, proceed to the main window
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close(); // Close the splash screen
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            // Handle exceptions, proceed to the main app
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        // DummyLoad();
    }
    
    private async void DummyLoad()
    {
        // Do some background stuff here.
        await Task.Delay(4000);

        // await Dispatcher.UIThread.InvokeAsync(() =>
        // {
        //     // _mainAction?.Invoke();
        //     Close();
        // });
    }
}