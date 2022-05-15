using MessageData.Sender;

Console.Title = "Message Data - Sender";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCustomMassTransit();
        services.AddHostedService<CommandsPublisherBackgroundService>();
    })
    .Build();

await host.RunAsync();
