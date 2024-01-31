using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactBook.Shared.Enums;
using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Repositories;
using ContactBook.WPFApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ContactBook.WPFApp.Mvvm.ViewModels;

public partial class AllContactsViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IContactRepository _contactRepo;
    private readonly ContactService _contactService;

    public AllContactsViewModel(IServiceProvider sp, IContactRepository contactRepo, ContactService contactService)
    {
        _serviceProvider = sp;
        _contactRepo = contactRepo;
        _contactService = contactService;

        //ContactsList = new ObservableCollection<IPerson>(_contactRepo.GetAllContacts());
        GetAllContactsFromRepo();
    }

    [ObservableProperty]
    private ObservableCollection<IPerson> _contactsList = [];

    [RelayCommand]
    private void NavTo_MainMenu()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<MainMenuViewModel>();
    }

    [RelayCommand]
    private void NavTo_AddContactView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddContactViewModel>();
    }

    [RelayCommand]
    private void NavTo_EditContact(IPerson contact)
    {
        _contactService.CurrentContact = contact;
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<EditContactViewModel>();
    }

    [RelayCommand]
    private void RemoveContact(IPerson contact)
    {
        IServiceResponse response = _contactRepo.DeleteContactFromList(x => x.Email == contact.Email);
        switch (response.Status)
        {
            case ServiceStatus.DELETED:
                Debug.WriteLine("Contact was removed");
                GetAllContactsFromRepo();
                break;
            case ServiceStatus.NOT_FOUND:
                Debug.WriteLine("Contact was not found");
                break;
            case ServiceStatus.FAILED:
            default:
                Debug.WriteLine("RemoveContact failed:");
                Debug.WriteLine(response.Status.ToString() + ": " + response.Result.ToString());
                break;
        }
    }

    private void GetAllContactsFromRepo()
    {
        IServiceResponse response = _contactRepo.GetAllContacts();
        if (response.Status == ServiceStatus.SUCCESS)
        {
            if (response.Result is List<IPerson> contactList && contactList.Any())
            {
                ContactsList = new ObservableCollection<IPerson>((IEnumerable<IPerson>)response.Result);
            }
        }
        else
        {
            Debug.WriteLine("GetAllContactsFromRepo failed:");
            Debug.WriteLine(response.Status.ToString() + ": " + response.Result.ToString());
        }
    }
}
