namespace ContactBook.Interfaces
{
    public interface IPerson
    {
        IAddress? Address { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        Guid Id { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
    }
}