using ContactBook.Shared.Enums;

namespace ContactBook.Shared.Interfaces
{
    public interface IServiceResponse
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}