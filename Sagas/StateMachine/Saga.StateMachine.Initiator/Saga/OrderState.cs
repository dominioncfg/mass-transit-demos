using MassTransit;


namespace Saga.StateMachine.Initiator;

public class OrderState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
#nullable disable
    public string CurrentState { get; set; }
#nullable enable
    public string? ClientName { get; set; }
}

