using MassTransit;
using Contracts;

namespace Saga.StateMachine.Initiator;

public static class ConfigurationExtensions
{
    public static void AddCustomMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .RedisRepository("127.0.0.1");

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
        EndpointConvention.Map<SubmitOrderCommand>(new Uri($"queue:{ConfigurationConstants.StateMachineSagaSecondService}"));
        EndpointConvention.Map<CancelOrderCommand>(new Uri($"queue:{ConfigurationConstants.StateMachineSagaSecondService}"));

        EndpointConvention.Map<RequestOrderFulfillmentCommand>(new Uri($"queue:{ConfigurationConstants.StateMachineSagaThirdService}"));
    }
}
