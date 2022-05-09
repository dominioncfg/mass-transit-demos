using MassTransit;

namespace Saga.Consumer.ThirdService;

public class RequestOrderFulfillmentCommandConsumerDefinition : ConsumerDefinition<RequestOrderFulfillmentCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RequestOrderFulfillmentCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
