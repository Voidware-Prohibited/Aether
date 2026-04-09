using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using NetSparkleUpdater.UI.Avalonia;
using System;
using NetSparkleUpdater.AppCastHandlers;
namespace TargetVectorLauncher.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NetSparkleUpdater.Interfaces;
using NetSparkleUpdater;
using NetSparkleUpdater.UI.Avalonia;

public partial class LoadingWindowModel : INotifyPropertyChanged
{
    private readonly CancellationTokenSource _cts = new();
    
    public CancellationToken CancellationToken => _cts.Token;
    
    private string _statusText = "Checking for updates...";
    private bool _isChecking = true;
    private SparkleUpdater _sparkle;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsChecking
    {
        get => _isChecking;
        set
        {
            _isChecking = value;
            OnPropertyChanged();
        }
    }
    
    public SparkleUpdater SparkleInstance => _sparkle;

    private bool _isProgressVisible;
    public bool IsProgressVisible
    {
        get => _isProgressVisible;
        set => SetProperty(ref _isProgressVisible, value);
    }

    private double _progressValue;
    public double ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }
    
    public LoadingWindowModel()
    {
        // Initialize SparkleUpdater here or via an injection.
        // Replace with your actual appcast URL and public key.
        _sparkle = new SparkleUpdater(
            "http://example.com/appcast.xml", 
            new Ed25519Checker(SecurityMode.Strict, "YOUR_BASE64_PUBLIC_KEY")
        )
        {
            UIFactory = new UIFactory(), // Use WPF UI factory
            // ShowsUIOnMainThread = true,
            // Configure other options as needed
        };
    }
    
    public async Task CheckForUpdates(LoadingWindowModel? viewModel)
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
                    viewModel.StatusText = "Update check cancelled by the user.";
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

    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}