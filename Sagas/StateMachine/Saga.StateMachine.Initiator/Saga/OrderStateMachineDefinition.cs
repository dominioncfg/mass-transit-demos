using MassTransit;


namespace Saga.StateMachine.Initiator;

public class OrderStateMachineDefinition : SagaDefinition<OrderState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
    {
        sagaConfigurator.UseInMemoryOutbox();
    }
}