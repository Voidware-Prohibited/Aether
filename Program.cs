using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using System;
using NetSparkleUpdater.AppCastHandlers;
using NetSparkleUpdater.Enums;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using NetSparkleUpdater;
using TargetVectorLauncher.ViewModels;
using TargetVectorLauncher.Views;
using NetSparkleUpdater.SignatureVerifiers;

namespace TargetVectorLauncher;

sealed class Program
{
    // The main window will be shown after the update check.
    private static MainWindow? _mainWindow;
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    
    public static async void AppMain(Application app, string[] args)
    {
        var splash = new LoadingWindow();
        app.ApplicationLifetime = new ClassicDesktopStyleApplicationLifetime { MainWindow = splash };
        // splash.Show();
        
        _mainWindow = new MainWindow();
        (app.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)!.MainWindow = _mainWindow;
        // _mainWindow.Show();

        // Perform update check asynchronously to not block the UI thread.
        await CheckForUpdates(splash.DataContext as LoadingWindowModel);

        // After the update check, open the main window and close the splash.
        Dispatcher.UIThread.Post(() =>
        {
            // splash.Close();
            _mainWindow = new MainWindow();
            (app.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)!.MainWindow = _mainWindow;
            _mainWindow.Show();
        }, DispatcherPriority.Background);

        app.Run(_mainWindow);
    }

    
    static public async Task CheckForUpdates(LoadingWindowModel? viewModel)
    {
        if (viewModel == null)
        {
            // Handle a null view model if necessary, e.g., using a logger.
            return;
        }

        try
        {
            viewModel.IsChecking = true;
            viewModel.StatusText = "Checking for updates...";
        
            // Use Sparkle's built-in async check for updates.
            // Passing 'true' tells Sparkle to do a silent check first.
            // It will only show a UI if an update is found.
            var result = await Task.Run(() => viewModel.SparkleInstance.CheckForUpdatesAtUserRequest());

            // Process the result
            switch (result.Status)
            {
                case UpdateStatus.UpdateAvailable:
                    // Sparkle's UI will handle the download/install process.
                    viewModel.StatusText = "An update is available. Please follow the prompts.";
                    break;
                case UpdateStatus.UpdateNotAvailable:
                    viewModel.StatusText = "Your application is up to date.";
                    break;
                case UpdateStatus.UserSkipped:
                    viewModel.StatusText = "Update check skipped by the user.";
                    break;
                case UpdateStatus.CouldNotDetermine:
                    viewModel.StatusText = "An error occurred while checking for updates.";
                    break;
                default:
                    viewModel.StatusText = "Update check finished.";
                    break;
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions during the update process.
            viewModel.StatusText = $"An error occurred: {ex.Message}";
            // Log the exception for debugging purposes.
        }
        finally
        {
            viewModel.IsChecking = false;
        }
    }
}