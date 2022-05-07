using System.Text.Json;

namespace Contracts;

public record GetUserQuery
{
    public Guid Id { get; init; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
public record GetUserQueryResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
