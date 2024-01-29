using ContactBook.Interfaces;
using ContactBook.Models;
using ContactBook.Repositories;
using System.Reflection;

namespace ContactBook.Services;

public interface IMenuService
{
    void Show_MainMenu();
}

public class MenuService : IMenuService
{

    private readonly IContactRepository _contactRepository;

    public MenuService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public void Show_MainMenu()
    {
        while (true)
        {
            DisplayMenuTitle("Main Menu");
            List<string> optionsList = [
                "1. Add New Contact",
                "2. List Existing Contacts",
                "3. Remove Contact",
                "4. Update Contact",
                "5. Show Contact Details",
                "0. Exit Application"
            ];
            foreach (var item in optionsList)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nSelect an option:");
            var option = Console.ReadKey(true).KeyChar.ToString();
            switch (option)
            {
                case "0":
                    Show_ExitApplicationOption();
                    break;
                case "1":
                    Show_AddContactOption();
                    break;
                case "2":
                    Show_ListAllContactsOption();
                    break;
                case "3":
                    Show_DeleteContactOption();
                    break;
                case "4":
                    Show_UpdateContactOption();
                    break;
                case "5":
                    Show_ContactDetailsOption();
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("\nInvalid selection, please select one of the options above. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void Show_AddContactOption()
    {
        //IPerson contact = new Person();

        DisplayMenuTitle("Add a new comtact");
        IPerson contact = EnterContactInformation();

        IServiceResponse response = _contactRepository.AddContactToList(contact);

        switch (response.Status)
        {
            case Enums.ServiceStatus.SUCCESS:
                Console.Clear();
                Console.WriteLine("The contact was added successfully with the following information.\n");

                if (response.Result is IPerson newContact)
                {
                    Console.WriteLine($"Name: {newContact.FirstName} {newContact.LastName}");
                    Console.WriteLine("Email: " + newContact.Email);
                    Console.WriteLine("Phone: " + newContact.PhoneNumber);
                    if (newContact.Address is IAddress)
                    {
                        foreach (PropertyInfo prop in newContact.Address.GetType().GetProperties())
                        {
                            var value = prop.GetValue(newContact.Address, null);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(prop.Name + ": " + value);
                            }
                        }
                    }
                }
                break;
            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"\nA contact with e-mail adress {contact.Email} already exists:");
                break;
            case Enums.ServiceStatus.FAILED:
            default:
                Console.WriteLine("An error occured with the following error message.");
                Console.WriteLine(response.Result.ToString());
                break;
        }

        Console.WriteLine("\nPress any key to return to main menu.");
        Console.ReadKey();
    }

    private void Show_ContactDetailsOption()
    {
        DisplayMenuTitle("Show detailed contact information");

        Console.WriteLine("Type the email of the contact you wish to find:");
        IServiceResponse response = GetContactByEmail();

        switch (response.Status)
        {
            case Enums.ServiceStatus.SUCCESS:
                Console.Clear();
                Console.WriteLine("\nFound a contact with the provided email address:\n");
                if (response.Result is IPerson foundContact)
                {
                    Console.WriteLine($"{foundContact.FirstName} {foundContact.LastName}");
                    Console.WriteLine("-------");
                    Console.WriteLine("Email: " + foundContact.Email);
                    Console.WriteLine("Phone: " + foundContact.PhoneNumber);
                    if (foundContact.Address is IAddress)
                    {
                        Console.WriteLine("Address:");
                        foreach (PropertyInfo prop in foundContact.Address.GetType().GetProperties())
                        {
                            var value = prop.GetValue(foundContact.Address, null);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(value);
                            }
                        }
                    }
                }
                break;
            case Enums.ServiceStatus.NOT_FOUND:
                Console.WriteLine("No match found with the provided email address.");
                break;
            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("An error occurred witth the following error message:");
                Console.WriteLine(response.Result.ToString());
                break;
        }

        //Console.Write("\nSearch for available contacts: ");
        //var searchString = Console.ReadLine()!;
        //string propertyName = null!;
        //IServiceResponse contactList = _contactRepository.GetAllContacts();

        //if (contactList.Result is List<IPerson> allContacts)
        //{
        //    foreach (IPerson contact in allContacts)
        //    {
        //        foreach (PropertyInfo prop in contact.GetType().GetProperties())
        //        {
        //            var value = prop.GetValue(contact, null);
        //            if (contact.Address is IAddress && prop.Name.ToString() == "Address")
        //            {
        //                foreach (PropertyInfo addressProp in contact.Address.GetType().GetProperties())
        //                {
        //                    value = addressProp.GetValue(contact.Address, null);
        //                    if (value!.ToString()!.Contains(searchString))
        //                    {
        //                        propertyName = addressProp.Name;
        //                        break;
        //                    }
        //                }
        //            } else if (value!.ToString()!.Contains(searchString))
        //            {
        //                propertyName = prop.Name;
        //                break;
        //            }
        //        }
        //    }
        //}

        //if (!string.IsNullOrEmpty(propertyName))
        //{
        //    Console.WriteLine(propertyName);
        //    IServiceResponse response = _contactRepository.GetContactFromList((x) => x[propertyName]);
        //}
        //else
        //{
        //    Console.WriteLine("No macthes were found.");
        //}

        Console.WriteLine("\nPress any key to return to main menu.");
        Console.ReadKey();
    }

    private void Show_DeleteContactOption()
    {
        DisplayMenuTitle("Delete contact:");
        Console.WriteLine("Type the email of the contact you wish to delete\n");
        var emailToRemove = Console.ReadLine();
        IServiceResponse response = _contactRepository.DeleteContactFromList(x => x.Email == emailToRemove);

        switch (response.Status)
        {
            case Enums.ServiceStatus.DELETED:
                if (response.Result is IPerson contact)
                {
                    Console.WriteLine($"Contact {contact.FirstName} {contact.LastName} was deleted.");
                }
                break;
            case Enums.ServiceStatus.NOT_FOUND:
                Console.WriteLine($"Could not find a contact with the email address {emailToRemove}");
                break;
            case Enums.ServiceStatus.FAILED:
            default:
                Console.WriteLine("An error occurred witth the following error message:");
                Console.WriteLine(response.Result.ToString());
                break;

        }

        Console.WriteLine("\nPress any key to return to main menu.");
        Console.ReadKey();
    }

    private void Show_ListAllContactsOption()
    {
        DisplayMenuTitle("All contacts:");
        IServiceResponse response = _contactRepository.GetAllContacts();

        if (response.Status == Enums.ServiceStatus.SUCCESS)
        {
            if (response.Result is List<IPerson> contactList && contactList.Any())
            {
                var counter = 0;
                foreach (IPerson contact in contactList)
                {
                    counter++;
                    Console.WriteLine($"\n{counter}. {contact.FirstName} {contact.LastName}");
                    Console.WriteLine("-------");
                    Console.WriteLine("Email: " + contact.Email);
                    Console.WriteLine("Phone: " + contact.PhoneNumber);
                    if (contact.Address is IAddress)
                    {
                        Console.WriteLine("Address:");
                        foreach (PropertyInfo prop in contact.Address.GetType().GetProperties())
                        {
                            var value = prop.GetValue(contact.Address, null);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(value);
                            }
                        }
                    }
                    if (counter % 5 == 0 && counter < contactList.Count)
                    {
                        Console.WriteLine("\nPress any key to see the next 5 contacts.");
                        Console.ReadKey();
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("There are currently no contacts to show.");
            }
        }
        else
        {
            Console.WriteLine("An error occured.");
            Console.WriteLine(response.Result.ToString());
        }
        Console.WriteLine("\nPress any key to return to main menu.");
        Console.ReadKey();
    }

    private void Show_UpdateContactOption()
    {
        DisplayMenuTitle("Update contact:");
        Console.WriteLine("Type the email of the contact you wish to update\n");

        IServiceResponse response = GetContactByEmail();

        switch (response.Status)
        {
            case Enums.ServiceStatus.SUCCESS:
                Console.Clear();
                Console.WriteLine("\nFound a contact with the provided email address:\n");
                if (response.Result is IPerson foundContact)
                {
                    Console.WriteLine($"{foundContact.FirstName} {foundContact.LastName}");
                    Console.WriteLine("-------");
                    Console.WriteLine("Email: " + foundContact.Email);
                    Console.WriteLine("Phone: " + foundContact.PhoneNumber);
                    if (foundContact.Address is IAddress)
                    {
                        Console.WriteLine("Address:");
                        foreach (PropertyInfo prop in foundContact.Address.GetType().GetProperties())
                        {
                            var value = prop.GetValue(foundContact.Address, null);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(value);
                            }
                        }
                    }
                    Console.WriteLine("\nDo you want to update this contact? (y/n)");
                    var option = Console.ReadKey(true).KeyChar.ToString();
                    if (option == "y")
                    {
                        Console.WriteLine();
                        UpdateContact(foundContact);
                    }
                }
                break;
            case Enums.ServiceStatus.NOT_FOUND:
                Console.WriteLine("No match found with the provided email address.");
                break;
            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("An error occurred witth the following error message:");
                Console.WriteLine(response.Result.ToString());
                break;
        }
        Console.WriteLine("\nPress any key to return to main menu.");
        Console.ReadKey();
    }

    private void Show_ExitApplicationOption()
    {
        Console.Clear();
        Console.WriteLine("Are you sure you wish to exit? (y/n): ");
        var option = Console.ReadKey().KeyChar.ToString();

        if (option == "y")
        {
            Environment.Exit(0);
        }
    }

    private void DisplayMenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"### {title} ###");
        Console.WriteLine();
    }

    private IServiceResponse GetContactByEmail()
    {
        string matchString = "";

        while (matchString.Length == 0)
        {
            matchString = Console.ReadLine()!;
        }

        return _contactRepository.GetContactFromList((x) => x.Email == matchString);
    }

    private void UpdateContact(IPerson oldContact)
    {
        IPerson newContactData = EnterContactInformation();
        IServiceResponse response = _contactRepository.UpdateContact(newContactData, oldContact);

        switch (response.Status)
        {
            case Enums.ServiceStatus.UPDATED:
                Console.Clear();
                Console.WriteLine("The contact was updated with the following infromation:");
                if (response.Result is IPerson newContact)
                {
                    Console.WriteLine($"Name: {newContact.FirstName} {newContact.LastName}");
                    Console.WriteLine("Email: " + newContact.Email);
                    Console.WriteLine("Phone: " + newContact.PhoneNumber);
                    if (newContact.Address is IAddress)
                    {
                        foreach (PropertyInfo prop in newContact.Address.GetType().GetProperties())
                        {
                            var value = prop.GetValue(newContact.Address, null);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                Console.WriteLine(prop.Name + ": " + value);
                            }
                        }
                    }
                }
                break;
            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"\nA contact with e-mail adress {newContactData.Email} already exists:");
                break;
            case Enums.ServiceStatus.FAILED:
            default:
                Console.WriteLine("An error occured with the following error message.");
                Console.WriteLine(response.Result.ToString());
                break;
        }
    }

    private IPerson EnterContactInformation()
    {
        IPerson contact = new Person();

        Console.Write("First Name: ");
        contact.FirstName = Console.ReadLine()!;
        Console.Write("Last Name: ");
        contact.LastName = Console.ReadLine()!;
        Console.Write("Email: ");
        contact.Email = Console.ReadLine()!;
        Console.Write("Phone Number: ");
        contact.PhoneNumber = Console.ReadLine()!;
        contact.Id = Guid.NewGuid();
        Console.WriteLine("\nDo you want to add an address for this contact? (y/n)\n");
        var keyPressed = Console.ReadKey(true).KeyChar.ToString();
        switch (keyPressed)
        {
            case "y":
                IAddress address = new Address();
                Console.WriteLine("Address Line 1: ");
                address.AddressLine1 = Console.ReadLine()!;
                Console.WriteLine("Address Line 2 (optional, press Enter to skip): ");
                address.AddressLine2 = Console.ReadLine() ?? null;
                int n;
                string input;
                do
                {
                    Console.WriteLine("Postal Code (numericals only): ");
                    input = Console.ReadLine()!;
                } while (int.TryParse(input, out n) == false);
                address.PostalCode = n;
                Console.WriteLine("City: ");
                address.City = Console.ReadLine()!;
                Console.WriteLine("Region (optional, press Enter to skip): ");
                address.Region = Console.ReadLine()!;
                Console.WriteLine("Country: ");
                address.Country = Console.ReadLine()!;
                contact.Address = address;
                break;
            case "n":
            default:
                break;
        }

        return contact;
    }
}