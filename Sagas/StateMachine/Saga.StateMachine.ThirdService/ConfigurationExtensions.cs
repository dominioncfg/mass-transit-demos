using Contracts;
using MassTransit;
using System.Reflection;

namespace Saga.StateMachine.ThirdService;

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
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ReceiveEndpoint(ConfigurationConstants.StateMachineSagaThirdService, e =>
                {
                    e.ConfigureConsumer<RequestOrderFulfillmentCommandConsumer>(context);
                });
            });
        });
    }
}
