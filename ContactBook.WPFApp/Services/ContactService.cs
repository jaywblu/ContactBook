using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Models;

namespace ContactBook.WPFApp.Services;

public class ContactService
{
    public IPerson CurrentContact { get; set; } = null!;

    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    /// <summary>
    /// Updates a contact in list
    /// </summary>
    /// <param name="contactToUpdate"></param>
    /// <param name="oldContact"></param>
    public void UpdateContact(IPerson contactToUpdate, IPerson oldContact)
    {
        IPerson newContact = new Person
        {
            FirstName = contactToUpdate.FirstName ?? "",
            LastName = contactToUpdate.LastName ?? "",
            PhoneNumber = contactToUpdate.PhoneNumber ?? "",
            Email = contactToUpdate.Email ?? ""
        };

        if (!string.IsNullOrWhiteSpace(contactToUpdate.Address?.AddressLine1) &&
            contactToUpdate.Address?.PostalCode != null &&
            !string.IsNullOrWhiteSpace(contactToUpdate.Address?.City) &&
            !string.IsNullOrWhiteSpace(contactToUpdate.Address?.Country))
        {
            newContact.Address = new Address
            {
                AddressLine1 = contactToUpdate.Address.AddressLine1,
                AddressLine2 = contactToUpdate.Address.AddressLine2 ?? "",
                PostalCode = contactToUpdate.Address.PostalCode,
                City = contactToUpdate.Address.City,
                Region = contactToUpdate.Address.Region ?? "",
                Country = contactToUpdate.Address.Country
            };
        }

        _contactRepository.UpdateContact(newContact, oldContact);
    }
}
