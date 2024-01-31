using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactBook.Shared.Enums;
using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ContactBook.WPFApp.Mvvm.ViewModels;

public partial class AddContactViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IContactRepository _contactRepo;

    public AddContactViewModel(IServiceProvider sp, IContactRepository contactRepo)
    {
        _serviceProvider = sp;
        _contactRepo = contactRepo;
    }

    [ObservableProperty]
    public IPerson contact = new Person();

    [ObservableProperty]
    public IAddress address = new Address();


    [RelayCommand]
    public void SaveContact()
    {
        Contact.Address = Address;
        IServiceResponse response = _contactRepo.AddContactToList(Contact);
        switch (response.Status)
        {
            case ServiceStatus.SUCCESS:
                var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AllContactsViewModel>();
                break;
            case ServiceStatus.ALREADY_EXISTS:
                Debug.WriteLine("Contact already exists");
                break;
            case ServiceStatus.FAILED:
            default:
                Debug.WriteLine("SaveContact failed:");
                Debug.WriteLine(response.Status.ToString() + ": " + response.Result.ToString());
                break;
        }
    }

    [RelayCommand]
    public void NavTo_MainMenuView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<MainMenuViewModel>();
    }
}
