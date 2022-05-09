using Contracts.Sagas.Consumer;
using MassTransit;

namespace Saga.Consumer.Initiator;


public class OrderSaga :
    ISaga,
    InitiatedBy<OrderSubmittedEvent>,
    Orchestrates<OrderFulfilledEvent>,
    Orchestrates<OrderFulfillmentFailedEvent>,
    Orchestrates<OrderCancelledEvent>
{

    public Guid CorrelationId { get; set; }


    public OrderSaga()
    {
    }

    public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
    {
        LogMessage(context.Message, CorrelationId);
        var command = new RequestOrderFulfillmentCommand()
        {
            Id = context.CorrelationId!.Value
        };
        await context.Send(command);

    }

    public async Task Consume(ConsumeContext<OrderFulfillmentFailedEvent> context)
    {
        LogMessage(context.Message, CorrelationId);
        var command = new CancelOrderCommand()
        {
            Id = context.CorrelationId!.Value
        };
        await context.Send(command);
    }

    public Task Consume(ConsumeContext<OrderFulfilledEvent> context)
    {
        LogMessage(context.Message, CorrelationId);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        LogMessage(context.Message, CorrelationId);
        return Task.CompletedTask;
    }

    private void LogMessage(object message, Guid id)
    {
        //Consumer Sagas dont support dependency injeciton
        Console.WriteLine($"Handled {message.GetType()} with id {id}");
    }
}
