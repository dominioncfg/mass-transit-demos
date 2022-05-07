using System.Text.Json;

namespace Contracts;

public record ExistUserQuery
{
    public Guid Id { get; init; }
    public override string ToString() => JsonSerializer.Serialize(this);
}

public record ExistUserQueryResponse
{
    public bool Exist { get; init; }
    public override string ToString() => JsonSerializer.Serialize(this);
}
