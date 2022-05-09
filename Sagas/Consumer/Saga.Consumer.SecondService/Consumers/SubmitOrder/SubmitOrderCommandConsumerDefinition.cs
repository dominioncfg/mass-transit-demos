using MassTransit;

namespace Saga.Consumer.SecondService;

public class SubmitOrderCommandConsumerDefinition : ConsumerDefinition<SubmitOrderCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubmitOrderCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
