using MassTransit;

namespace Saga.Consumer.SecondService;

public class CancelOrderCommandConsumerDefinition : ConsumerDefinition<CancelOrderCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CancelOrderCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
