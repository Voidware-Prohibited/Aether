using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TargetVectorLauncher.ViewModels;

public partial class LoadingWindowModel : ViewModelBase
{
    [ObservableProperty]
    private string _startupMessage = "Starting application...";
    
    public void Cancel()
    {
        StartupMessage = "Cancelling...";
        _cts.Cancel();
    }
    
    private readonly CancellationTokenSource _cts = new();
    
    public CancellationToken CancellationToken => _cts.Token;
}