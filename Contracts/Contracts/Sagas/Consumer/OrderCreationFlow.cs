using MassTransit;

namespace Contracts.Sagas.Consumer;

public record SubmitOrderCommand
{
    public Guid Id { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public int Total { get; init; }

}

public record OrderSubmittedEvent: CorrelatedBy<Guid>
{
    public Guid Id { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public int Total { get; init; }

    public Guid CorrelationId => Id;
}


public record RequestOrderFulfillmentCommand
{
    public Guid Id { get; init; }
}

public record OrderFulfilledEvent : CorrelatedBy<Guid>
{
    public Guid Id { get; init; }
    public Guid CorrelationId => Id;
}

public record OrderFulfillmentFailedEvent : CorrelatedBy<Guid>
{
    public Guid Id { get; init; }
    public Guid CorrelationId => Id;
}

public record CancelOrderCommand
{
    public Guid Id { get; init; }

}

public class OrderCancelledEvent : CorrelatedBy<Guid>
{
    public Guid Id { get; init; }
    public Guid CorrelationId => Id;
}
