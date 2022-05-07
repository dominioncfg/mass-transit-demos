using System.Text.Json;

namespace Contracts;

public record UserUpdatedEvent
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;

    public override string ToString() => JsonSerializer.Serialize(this);
}
