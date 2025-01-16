using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using System;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace TargetVectorLauncher.Views;

public partial class LoadingWindow : Window
{
    private readonly Action? _mainAction;
    
    public LoadingWindow()
    {
    }
    public LoadingWindow(Action mainAction)
    {
        InitializeComponent();
        _mainAction = mainAction;
    }
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        DummyLoad();
    }
    
    private async void DummyLoad()
    {
        // Do some background stuff here.
        await Task.Delay(1000);

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _mainAction?.Invoke();
            Close();
        });
    }
}