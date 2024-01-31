using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ContactBook.WPFApp.Mvvm.ViewModels;

public partial class MainMenuViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;

    public MainMenuViewModel(IServiceProvider sp)
    {
        _serviceProvider = sp;
    }

    [RelayCommand]
    public void NavTo_AllContactsView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AllContactsViewModel>();
    }

    [RelayCommand]
    private void NavTo_AddContactView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddContactViewModel>();
    }

    [RelayCommand]
    private void NavTo_FindContactView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<FindContactViewModel>();
    }

    [RelayCommand]
    private void ExitApplication()
    {
        System.Windows.Application.Current.Shutdown();
    }

}
