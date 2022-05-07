using MassTransit;

namespace Worker.Consumer;

public class UpdateUserCommandConsumerDefinition : ConsumerDefinition<UpdateUserCommandConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UpdateUserCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
