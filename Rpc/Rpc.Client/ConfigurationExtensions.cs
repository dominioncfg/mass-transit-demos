using MassTransit;
using System.Reflection;
using Contracts;

namespace Rpc.Client;

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
            });

            ConfigureSendCommands();
        });
    }

    private static void ConfigureSendCommands()
    {
        EndpointConvention.Map<ExistUserQuery>(new Uri($"queue:{ConfigurationConstants.RpcServerQueueName}"));
        EndpointConvention.Map<GetUserQuery>(new Uri($"queue:{ConfigurationConstants.RpcServerQueueName}"));
    }
}
