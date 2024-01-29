using ContactBook.Shared.Enums;
using ContactBook.Shared.Interfaces;

namespace ContactBook.Shared.Models.Responses;

public class ServiceResponse : IServiceResponse
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
