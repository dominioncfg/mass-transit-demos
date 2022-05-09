using Saga.Consumer.Initiator;

Console.Title = "Consumer Saga - Initiator";


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCustomMassTransit();
        services.AddHostedService<InitializeSagaBackgroundService>();
    })
    .Build();

await host.RunAsync();
