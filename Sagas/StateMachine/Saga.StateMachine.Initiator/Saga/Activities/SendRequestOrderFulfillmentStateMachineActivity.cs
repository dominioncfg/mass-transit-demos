using MassTransit;
using Contracts.Sagas.StateMachine;

namespace Saga.StateMachine.Initiator;

public class SendRequestOrderFulfillmentStateMachineActivity : IStateMachineActivity<OrderState>
{
    private readonly ConsumeContext _context;
    private readonly ILogger<SendRequestOrderFulfillmentStateMachineActivity> _logger;

    public SendRequestOrderFulfillmentStateMachineActivity(ConsumeContext context, ILogger<SendRequestOrderFulfillmentStateMachineActivity> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("send-order-fullfilment-command");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState> context, IBehavior<OrderState> next)
    {
        await SendFullfillOrderCommand(context.Saga.CorrelationId);
        await next.Execute(context);
    }

    public async Task Execute<T>(BehaviorContext<OrderState, T> context, IBehavior<OrderState, T> next) where T : class
    {
        await SendFullfillOrderCommand(context.Saga.CorrelationId);
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, TException> context, IBehavior<OrderState> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public Task Faulted<T, TException>(BehaviorExceptionContext<OrderState, T, TException> context, IBehavior<OrderState, T> next)
        where T : class
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public async Task SendFullfillOrderCommand(Guid orderId)
    {
        _logger.LogInformation("Fullfilling order {OrderId}", orderId);
        await _context.Send(new RequestOrderFulfillmentCommand { Id = orderId });
    }
}

