using ContactBook.Shared.Enums;
using ContactBook.Shared.Interfaces;
using ContactBook.Shared.Models;
using ContactBook.Shared.Repositories;
using ContactBook.Shared.Services;
using Moq;
using Newtonsoft.Json;

namespace ContactBook.Tests;

public class ContactBook_Tests
{
    [Fact]
    public void AddContactToList_Should_AddOneContactToContactsList_ThenReturnTrue()
    {
        // Arrange
        IPerson contact = new Person 
        { 
            Id = Guid.NewGuid(),
            FirstName = "Test", 
            LastName = "Testman", 
            Email = "test1@domain.com", 
            PhoneNumber = "0707123456",
            Address = new Address
            {
                AddressLine1 = "Testmakargatan 12",
                AddressLine2 = "4tr",
                PostalCode = 12312,
                City = "Testholmen",
                Region = "Testrikland",
                Country = "Testmenistan"
            }
        };
        var mockFileService = new Mock<IFileService>();
        IContactRepository contactRepository = new ContactRepository(mockFileService.Object);

        // Act
        IServiceResponse result = contactRepository.AddContactToList(contact);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
    }

    [Fact]
    public void DeleteContactFromList_Should_DeleteOneContactFromList_ThenReturnTrue()
    {
        // Arrange
        IPerson contact = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Testman",
            Email = "test2@domain.com",
            PhoneNumber = "0707123456",
            Address = new Address
            {
                AddressLine1 = "Testmakargatan 12",
                AddressLine2 = "4tr",
                PostalCode = 12312,
                City = "Testholmen",
                Region = "Testrikland",
                Country = "Testmenistan"
            }
        };
        var mockFileService = new Mock<IFileService>();
        IContactRepository contactRepository = new ContactRepository(mockFileService.Object);
        contactRepository.AddContactToList(contact);

        // Act
        IServiceResponse result = contactRepository.DeleteContactFromList(x => x.Email == contact.Email);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(ServiceStatus.DELETED, result.Status);
        Assert.Equal(contact, result.Result);
    }

    [Fact]
    public void GetAllContactsFromList_Should_GetAllContactsInContactsList_ThenReturnTrue()
    {
        // Arrange
        List<IPerson> contacts =
        [
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "Testman",
                Email = "test3@domain.com",
                PhoneNumber = "0707123456",
                Address = new Address
                {
                    AddressLine1 = "Testmakargatan 12",
                    AddressLine2 = "4tr",
                    PostalCode = 12312,
                    City = "Testholmen",
                    Region = "Testrikland",
                    Country = "Testmenistan"
                }
            }
        ];
        var json = JsonConvert.SerializeObject(contacts, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        var mockFileService = new Mock<IFileService>();
        mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(json);
        IContactRepository contactRepository = new ContactRepository(mockFileService.Object);

        // Act
        IServiceResponse result = contactRepository.GetAllContacts();

        // Assert
        Assert.True(((IEnumerable<IPerson>)result.Result).Any());
        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        IPerson returnedContact = ((IEnumerable<IPerson>)result.Result).FirstOrDefault()!;
        Assert.Equal(returnedContact.Email, contacts[0].Email);
    }

    [Fact]
    public void GetContactFromList_Should_GetOneContactFromContactList_ThenReturnTrue()
    {
        // Arrange
        IPerson contact = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Testman",
            Email = "test4@domain.com",
            PhoneNumber = "0707123456",
            Address = new Address
            {
                AddressLine1 = "Testmakargatan 12",
                AddressLine2 = "4tr",
                PostalCode = 12312,
                City = "Testholmen",
                Region = "Testrikland",
                Country = "Testmenistan"
            }
        };
        var mockFileService = new Mock<IFileService>();
        IContactRepository contactRepository = new ContactRepository(mockFileService.Object);
        contactRepository.AddContactToList(contact);

        // Act
        IServiceResponse result = contactRepository.GetContactFromList(x => x.Email == contact.Email);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        Assert.Equal(contact, result.Result);
    }

    [Fact]
    public void UpdateContact_Should_UpdateOneContactInContactsList_ThenReturnTrue()
    {
        // Arrange
        List<IPerson> contacts =
        [
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "Testman",
                Email = "test5@domain.com",
                PhoneNumber = "0707123456",
                Address = new Address
                {
                    AddressLine1 = "Testmakargatan 12",
                    AddressLine2 = "4tr",
                    PostalCode = 12312,
                    City = "Testholmen",
                    Region = "Testrikland",
                    Country = "Testmenistan"
                }
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Anna",
                LastName = "Andersson",
                Email = "anna@domain.com",
                PhoneNumber = "0707123456",
                Address = new Address
                {
                    AddressLine1 = "Gatuvägen 12",
                    AddressLine2 = "",
                    PostalCode = 12312,
                    City = "Hufvudstaden",
                    Region = "",
                    Country = "Potatislandet"
                }
            }
        ];
        IPerson newContact = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Testsson",
            Email = "test5@domain.com",
            PhoneNumber = "0707123456",
            Address = null
        };
        var mockFileService = new Mock<IFileService>();
        IContactRepository contactRepository = new ContactRepository(mockFileService.Object);
        contactRepository.AddContactToList(contacts[0]);
        contactRepository.AddContactToList(contacts[1]);

        // Act
        IServiceResponse result = contactRepository.UpdateContact(newContact, contacts[0]);

        // Assert
        Assert.NotNull(result.Result);
        Assert.Equal(ServiceStatus.UPDATED, result.Status);
        Assert.Equal(result.Result, newContact);
    }
}

public class FileService_Tests
{
    [Fact]
    public void SaveContentToFile_Should_SaveContentToFile_ThenReturnTrue()
    {
        // Arrange
        string _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string _file = Path.Combine(_currentDirectory, @"..\..\..\test.txt");
        string _filePath = Path.GetFullPath(_file);
        IFileService fileService = new FileService();
        string content = "Test content";

        // Act
        bool result = fileService.SaveContentToFile(_filePath, content);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetContentFromFile_Should_GetContentFromFile_ThenReturnTrue()
    {
        // Arrange
        string _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string _file = Path.Combine(_currentDirectory, @"..\..\..\test.txt");
        string _filePath = Path.GetFullPath(_file);
        IFileService fileService = new FileService();
        string content = "Test content";
        fileService.SaveContentToFile(_filePath, content);

        // Act
        string result = fileService.GetContentFromFile(_filePath);

        // Assert
        Assert.Equal(content, result);
    }
}