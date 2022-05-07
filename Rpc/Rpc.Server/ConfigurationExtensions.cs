using Contracts;
using MassTransit;
using System.Reflection;

namespace Rpc.Server;

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

                cfg.ReceiveEndpoint(ConfigurationConstants.RpcServerQueueName, e =>
                {
                    e.ConfigureConsumer<ExistUserQueryConsumer>(context);
                    e.ConfigureConsumer<GetUserQueryConsumer>(context);
                });
            });
        });

    }
}
