using MassTransit;
using Contracts.Sagas.StateMachine;

namespace Saga.StateMachine.Initiator;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{

#nullable disable
    private readonly ILogger<OrderStateMachine> _logger;
    public State SubmittedState { get; private set; }
    public State FailedFulfillementState { get; private set; }
    public Event<OrderSubmittedEvent> OrderSubmittedEvent { get; private set; }
    public Event<OrderFulfilledEvent> OrderFulfilledEvent { get; private set; }
    public Event<OrderFulfillmentFailedEvent> OrderFulfillmentFailedEvent { get; private set; }
    public Event<OrderCancelledEvent> OrderCancelledEvent { get; private set; }
#nullable enable

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        _logger = logger;
        Event(() => OrderSubmittedEvent, x => x.CorrelateById(m => m.Message.Id));
        Event(() => OrderFulfilledEvent, x => x.CorrelateById(m => m.Message.Id));
        Event(() => OrderFulfillmentFailedEvent, x => x.CorrelateById(m => m.Message.Id));
        Event(() => OrderCancelledEvent, x => x.CorrelateById(m => m.Message.Id));

        InstanceState(x => x.CurrentState);


        Initially(
            When(OrderSubmittedEvent)
                .Then(context => { _logger.LogInformation("Order {OrderId} Submitted", context.Message.Id); })
                .Then(context =>
                {
                    context.Saga.ClientName = context.Message.ClientName;
                })
                .Activity(x => x.OfInstanceType<SendRequestOrderFulfillmentStateMachineActivity>())
                .TransitionTo(SubmittedState));


        During(SubmittedState,
            Ignore(OrderSubmittedEvent),

            When(OrderFulfilledEvent)
                .Then(context => { _logger.LogInformation("Order {OrderId} Fulfilled", context.Message.Id); })
                .Finalize(),

            When(OrderFulfillmentFailedEvent)
                 .Then(context => { _logger.LogInformation("Order {OrderId} Fulfillment Failed", context.Message.Id); })
                 .ThenAsync(async context => await context.Send(new CancelOrderCommand()
                 {
                     Id = context.Message.Id,
                 }))
                .TransitionTo(FailedFulfillementState));



        During(FailedFulfillementState,
            Ignore(OrderFulfilledEvent),

            Ignore(OrderFulfillmentFailedEvent),

            When(OrderCancelledEvent)
                .Then(context => { _logger.LogInformation("Order {OrderId} Cancelled", context.Message.Id); })
                .Finalize()
         );


    }
}

