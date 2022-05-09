using MassTransit;
using Contracts;
using Contracts.Sagas.Consumer;

namespace Saga.Consumer.Initiator;

public static class ConfigurationExtensions
{
    public static void AddCustomMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            
            x.AddSaga<OrderSaga>()
              .InMemoryRepository();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseConcurrencyLimit(1);

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        ConfigureSendCommands();
    }

    private static void ConfigureSendCommands()
    {
        EndpointConvention.Map<SubmitOrderCommand>(new Uri($"queue:{ConfigurationConstants.ConsumerMachineSagaSecondService}"));
        EndpointConvention.Map<CancelOrderCommand>(new Uri($"queue:{ConfigurationConstants.ConsumerMachineSagaSecondService}"));

        EndpointConvention.Map<RequestOrderFulfillmentCommand>(new Uri($"queue:{ConfigurationConstants.ConsumerMachineSagaThirdService}"));
    }
}
