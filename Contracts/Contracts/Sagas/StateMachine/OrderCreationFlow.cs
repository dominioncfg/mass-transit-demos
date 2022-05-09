namespace Contracts.Sagas.StateMachine;

public record SubmitOrderCommand
{
    public Guid Id { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public int Total { get; init; }

}

public record OrderSubmittedEvent
{
    public Guid Id { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public int Total { get; init; }
}


public record RequestOrderFulfillmentCommand
{
    public Guid Id { get; init; }
}

public record OrderFulfilledEvent
{
    public Guid Id { get; init; }
}

public record OrderFulfillmentFailedEvent
{
    public Guid Id { get; init; }
}

public record CancelOrderCommand
{
    public Guid Id { get; init; }

}

public class OrderCancelledEvent
{
    public Guid Id { get; init; }
}
