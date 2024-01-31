using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Models;
using ContactBook.WPFApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ContactBook.WPFApp.Mvvm.ViewModels;

public partial class EditContactViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IContactRepository _contactRepo;
    private readonly ContactService _contactService;
    private IPerson _oldContact;

    public EditContactViewModel(IServiceProvider sp, IContactRepository contactRepo, ContactService contactService)
    {
        _serviceProvider = sp;
        _contactRepo = contactRepo;
        _contactService = contactService;

        Contact = _contactService.CurrentContact;
        Address = Contact.Address ?? new Address();
        _oldContact = contact;
    }

    [ObservableProperty]
    private IPerson contact = new Person();

    [ObservableProperty]
    private IAddress address = new Address();

    [RelayCommand]
    private void UpdateContact()
    {
        if (Contact != null)
        {
            Contact.Address = Address;
            _contactService.UpdateContact(Contact, _oldContact);
        }

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AllContactsViewModel>();

    }

    [RelayCommand]
    public void NavTo_AllContactsView()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AllContactsViewModel>();
    }

}
    
