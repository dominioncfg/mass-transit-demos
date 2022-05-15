using Azure.Storage.Blobs;
using Contracts;
using MassTransit;
using System.Reflection;

namespace MessageData.Consumer;

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
                var client = new BlobServiceClient("UseDevelopmentStorage=true");
                var repository = client.CreateMessageDataRepository("message-data");
                cfg.UseMessageData(repository);
                cfg.UseConcurrencyLimit(1);

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(ConfigurationConstants.MessageDataQueueName, e =>
                {
                    e.ConfigureConsumer<CreateUserWithLargeDataCommandConsumer>(context);
                });               
            });
        });
    }
}
