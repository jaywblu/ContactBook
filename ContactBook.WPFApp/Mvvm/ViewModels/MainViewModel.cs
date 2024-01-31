using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ContactBook.WPFApp.Mvvm.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? _currentViewModel;

    private readonly IServiceProvider _serviceProvider;

    public MainViewModel(IServiceProvider sp)
    {
        _serviceProvider = sp;
        CurrentViewModel = _serviceProvider.GetRequiredService<MainMenuViewModel>();
    }

}
