using Saga.Consumer.SecondService;

Console.Title = "Consumer Saga - Second Service";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IObjectsRepository, InMemoryObjectsRepository>();
        services.AddCustomMassTransit();
    })
    .Build();

await host.RunAsync();
