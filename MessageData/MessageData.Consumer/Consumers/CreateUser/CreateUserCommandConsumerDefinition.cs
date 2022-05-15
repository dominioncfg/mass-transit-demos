using MassTransit;

namespace MessageData.Consumer;

public class CreateUserWithLargeDataCommandConsumerDefinition : ConsumerDefinition<CreateUserWithLargeDataCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateUserWithLargeDataCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
