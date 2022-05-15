using MassTransit;
using System.Text.Json;

namespace Contracts;

public record CreateUserWithLargeDataCommand
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public MessageData<byte[]>? BigData { get; set; } 

}
