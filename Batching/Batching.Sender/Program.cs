using Batching.Sender;

Console.Title = "Batching - Sender";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCustomMassTransit();
        services.AddHostedService<CommandsPublisherBackgroundService>();
    })
    .Build();

await host.RunAsync();
