﻿using ContactBook.Shared.Enums;
using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ContactBook.Shared.Repositories;

public class ContactRepository : IContactRepository
{
    private IFileService _fileService;
    private static List<IPerson> _contactList = [];
    private static readonly string _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string _file = Path.Combine(_currentDirectory, @"..\..\..\..\content.json");
    private static readonly string _filePath = Path.GetFullPath(_file);

    /// <summary>
    /// Constructor for ContactRepository
    /// </summary>
    /// <param name="fileService"></param>
    public ContactRepository(IFileService fileService)
    {
        _fileService = fileService;
        GetContactsFromFile();
    }

    /// <summary>
    /// Adds a new contact to list
    /// </summary>
    /// <param name="person"></param>
    /// <returns>A ServiceResponse containing a status message and a Person object</returns>
    public IServiceResponse AddContactToList(IPerson person)
    {
        IServiceResponse response = new ServiceResponse();

        try
        {
            if (!_contactList.Any(existingContact => existingContact.Email == person.Email))
            {
                _contactList.Add(person);
                SaveContactsToFile();
                GetContactsFromFile();
                response.Status = ServiceStatus.SUCCESS;
                response.Result = person;
            }
            else
            {
                response.Status = ServiceStatus.ALREADY_EXISTS;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
    }

    /// <summary>
    /// Removes an existing contact by callback function
    /// </summary>
    /// <param name="condition"></param>
    /// <returns>A ServiceResponse containing a status message and a Person object</returns>
    public IServiceResponse DeleteContactFromList(Func<IPerson, bool> condition)
    {
        IServiceResponse response = new ServiceResponse();

        try
        {
            IPerson contact = _contactList.FirstOrDefault(condition)!;
            if (contact != null)
            {
                _contactList.Remove(contact);
                SaveContactsToFile();
                GetContactsFromFile();
                response.Status = ServiceStatus.DELETED;
                response.Result = contact;
            }
            else
            {
                response.Status = ServiceStatus.NOT_FOUND;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
    }

    /// <summary>
    /// Retrieves all existing contacts in list
    /// </summary>
    /// <returns>A ServiceResponse containing a status message and a Person List</returns>
    public IServiceResponse GetAllContacts()
    {
        IServiceResponse response = new ServiceResponse();

        try
        {
            GetContactsFromFile();
            response.Status = ServiceStatus.SUCCESS;
            response.Result = _contactList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
    }

    /// <summary>
    /// Retrieves a single contact by callback function
    /// </summary>
    /// <param name="condition"></param>
    /// <returns>A ServiceResponse containing a status message and a Person object</returns>
    public IServiceResponse GetContactFromList(Func<IPerson, bool> condition)
    {
        IServiceResponse response = new ServiceResponse();

        try
        {
            GetContactsFromFile();
            IPerson contact = _contactList.FirstOrDefault(condition)!;
            if (contact != null)
            {
                response.Status = ServiceStatus.SUCCESS;
                response.Result = contact;
            }
            else
            {
                response.Status = ServiceStatus.NOT_FOUND;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
    }

    /// <summary>
    /// Updates an existing contact
    /// </summary>
    /// <param name="newContact"></param>
    /// <param name="oldContact"></param>
    /// <returns>A ServiceResponse containing a status message and a Person object</returns>
    public IServiceResponse UpdateContact(IPerson newContact, IPerson oldContact)
    {
        GetContactsFromFile();
        IServiceResponse response = new ServiceResponse();
        try
        {
            IPerson contactToUpdate = _contactList.FirstOrDefault(x => x.Email == oldContact.Email)!;
            if (contactToUpdate != null)
            {
                _contactList.Remove(contactToUpdate);
                if (!_contactList.Any(existingContact => existingContact.Email == newContact.Email))
                {
                    _contactList.Add(newContact);
                    SaveContactsToFile();
                    response.Status = ServiceStatus.UPDATED;
                    response.Result = newContact;
                    GetContactsFromFile();
                }
                else
                {
                    _contactList.Add(contactToUpdate);
                    response.Status = ServiceStatus.ALREADY_EXISTS;
                    response.Result = contactToUpdate;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            response.Status = ServiceStatus.FAILED;
            response.Result = ex.Message;
        }

        return response;
    }

    /// <summary>
    /// Populates contacts list from file
    /// </summary>
    /// <returns>An IEnumerable of Person objects</returns>
    private IEnumerable<IPerson> GetContactsFromFile()
    {
        try
        {
            var content = _fileService.GetContentFromFile(_filePath);
            if (!string.IsNullOrEmpty(content))
            {
                _contactList = JsonConvert.DeserializeObject<List<IPerson>>(content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                })!;
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return _contactList;
    }

    /// <summary>
    /// Saves all contacts in list to file
    /// </summary>
    private void SaveContactsToFile()
    {
        _fileService.SaveContentToFile(_filePath, JsonConvert.SerializeObject(_contactList, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        }));
    }
}
