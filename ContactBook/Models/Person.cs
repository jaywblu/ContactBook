using ContactBook.Interfaces;

namespace ContactBook.Models;

public class Person : IPerson
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IAddress? Address { get; set; }

}
