namespace ContactBook.Interfaces
{
    public interface IAddress
    {
        string AddressLine1 { get; set; }
        string? AddressLine2 { get; set; }
        string City { get; set; }
        string Country { get; set; }
        int PostalCode { get; set; }
        string? Region { get; set; }
    }
}