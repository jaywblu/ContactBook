using ContactBook.Enums;

namespace ContactBook.Interfaces
{
    public interface IServiceResponse
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}