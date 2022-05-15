using Azure.Storage.Blobs;
using Contracts;
using MassTransit;
using System.Reflection;


namespace MessageData.Sender;

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
                var containerName = "message-data";
                var client = new BlobServiceClient("UseDevelopmentStorage=true");

                CreateContainerIfNotExist(client, containerName);

                var repository = client.CreateMessageDataRepository(containerName);
                cfg.UseMessageData(repository);

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            ConfigureSendCommands();
        });
    }

    private static void CreateContainerIfNotExist(BlobServiceClient client, string containerName)
    {
        //Container must be created before sending messages
        var containerClient = client.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExistsAsync().GetAwaiter().GetResult();
    }

    private static void ConfigureSendCommands()
    {
        EndpointConvention.Map<CreateUserWithLargeDataCommand>(new Uri($"queue:{ConfigurationConstants.MessageDataQueueName}"));
    }
}
