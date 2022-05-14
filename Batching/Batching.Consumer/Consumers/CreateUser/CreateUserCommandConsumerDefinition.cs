using MassTransit;

namespace Batching.Consumer;

public class CreateUserCommandConsumerDefinition : ConsumerDefinition<CreateUserCommandConsumer>
{
    public CreateUserCommandConsumerDefinition()
    {
        Endpoint(x => x.PrefetchCount = 1000);
    }
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateUserCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

        consumerConfigurator.Options<BatchOptions>(options => options
          .SetMessageLimit(25)
          .SetTimeLimit(5000)
          .SetConcurrencyLimit(10));
    }
}