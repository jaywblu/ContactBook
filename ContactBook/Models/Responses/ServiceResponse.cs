using ContactBook.Enums;
using ContactBook.Interfaces;

namespace ContactBook.Models.Responses;

public class ServiceResponse : IServiceResponse
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
