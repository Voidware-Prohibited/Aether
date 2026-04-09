using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TargetVectorLauncher.ViewModels;

namespace TargetVectorLauncher.Views;

public partial class Landing : UserControl
{
    private readonly LandingViewModel _viewModel;
    public Landing()
    {
        InitializeComponent();
        _viewModel = new LandingViewModel();
    }
}