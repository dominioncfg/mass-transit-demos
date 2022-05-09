using MassTransit;


namespace Saga.StateMachine.Initiator;

public class OrderState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }
    public string? ClientName { get; set; }
}

