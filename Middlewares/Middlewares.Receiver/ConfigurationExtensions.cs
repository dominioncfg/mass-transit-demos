using Contracts;
using MassTransit;
using System.Reflection;

namespace Middlewares.Receiver;

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

                cfg.UseConsumeFilter(typeof(SkipMessageIfSkipHeaderExistFilter<>), context);

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(ConfigurationConstants.MiddlewaresQueueName, e =>
                {
                    e.ConfigureConsumer<CreateUserCommandConsumer>(context);
                    e.ConfigureConsumer<UpdateUserCommandConsumer>(context);
                });               
            });
        });
    }
}
