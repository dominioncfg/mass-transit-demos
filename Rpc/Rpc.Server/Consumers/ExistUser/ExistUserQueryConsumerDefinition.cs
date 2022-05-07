using MassTransit;

namespace Rpc.Server;

public class ExistUserQueryConsumerDefinition : ConsumerDefinition<ExistUserQueryConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ExistUserQueryConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
