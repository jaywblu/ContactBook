using ContactBook.Interfaces;

namespace ContactBook.Models;

public class Address : IAddress
{
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string? Region { get; set; }
    public int PostalCode { get; set; } = -1;
    public string Country { get; set; } = null!;
}
