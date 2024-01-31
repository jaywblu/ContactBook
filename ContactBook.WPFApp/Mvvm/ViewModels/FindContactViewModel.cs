using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactBook.Shared.Enums;
using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Models;
using ContactBook.WPFApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ContactBook.WPFApp.Mvvm.ViewModels;

public partial class FindContactViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IContactRepository _contactRepo;
    private readonly ContactService _contactService;

    public FindContactViewModel(IServiceProvider sp, IContactRepository contactRepo, ContactService contactService)
    {
        _serviceProvider = sp;
        _contactRepo = contactRepo;
        _contactService = contactService;
    }

    [ObservableProperty]
    public string email = string.Empty;

    [ObservableProperty]
    public IPerson contact = null!;

    [RelayCommand]
    public void FindContact()
    {
        if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrEmpty(Email))
        {
            IServiceResponse response = _contactRepo.GetContactFromList(x => x.Email == Email);

            switch (response.Status)
            {
                case ServiceStatus.SUCCESS:
                    if (response.Result is IPerson foundContact)
                    {
                        Contact = foundContact;
                    }
                    break;
                case ServiceStatus.NOT_FOUND:
                    Debug.WriteLine("No match found with the provided email address.");
                    break;
                case ServiceStatus.FAILED:
                    Debug.WriteLine("FindContact failed:");
                    Debug.WriteLine(response.Status.ToString() + ": " + response.Result.ToString());
                    break;
            }
        }
    }

    [RelayCommand]
    private void NavTo_MainMenu()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<MainMenuViewModel>();
    }

    [RelayCommand]
    private void NavTo_EditContact()
    {
        if (Contact != null)
        {
            _contactService.CurrentContact = Contact;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<EditContactViewModel>();
        }
    }

    [RelayCommand]
    private void RemoveContact()
    {
        if (Contact != null)
        {
            IServiceResponse response = _contactRepo.DeleteContactFromList(x => x.Email == Contact.Email);
            switch (response.Status)
            {
                case ServiceStatus.DELETED:
                    Debug.WriteLine("Contact was removed");
                    var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                    mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AllContactsViewModel>();
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
    }
}