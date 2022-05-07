using MassTransit;

namespace PublishSubcribe.Subscriber;

public class UserUpdatedEventConsumerDefinition : ConsumerDefinition<UserUpdatedEventConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserUpdatedEventConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}
