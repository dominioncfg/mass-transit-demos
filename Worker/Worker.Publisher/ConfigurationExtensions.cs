using Contracts;
using MassTransit;
using System.Reflection;


namespace Worker.Publisher;

public static class ConfigurationExtensions
{
    public static void AddCustomMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
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
        EndpointConvention.Map<CreateUserCommand>(new Uri($"queue:{ConfigurationConstants.WorkerQueueName}"));
        EndpointConvention.Map<UpdateUserCommand>(new Uri($"queue:{ConfigurationConstants.WorkerQueueName}"));
    }
}
