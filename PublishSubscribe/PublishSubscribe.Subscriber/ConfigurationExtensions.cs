using Contracts;
using MassTransit;
using System.Reflection;

namespace PublishSubcribe.Subscriber;

public static class ConfigurationExtensions
{
    public static void AddCustomMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(entryAssembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseConcurrencyLimit(1);

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(ConfigurationConstants.PublishSubscribeSubscriberQueueName, e =>
                {
                    e.ConfigureConsumer<UserCreatedEventConsumer>(context);
                    e.ConfigureConsumer<UserUpdatedEventConsumer>(context);
                });               
            });
        });
    }
}
