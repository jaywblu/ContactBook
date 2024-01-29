namespace ContactBook.Shared.Interfaces
{
    public interface IContactRepository
    {
        IServiceResponse AddContactToList(IPerson person);
        IServiceResponse DeleteContactFromList(Func<IPerson, bool> condition);
        IServiceResponse GetAllContacts();
        IServiceResponse GetContactFromList(Func<IPerson, bool> condition);
        IServiceResponse UpdateContact(IPerson newContact, IPerson oldContact);
    }
}