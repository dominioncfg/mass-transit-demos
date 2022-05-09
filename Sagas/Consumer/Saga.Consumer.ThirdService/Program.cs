using Saga.Consumer.ThirdService;

Console.Title = "Consumer Saga - Third Service";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IObjectsRepository, InMemoryObjectsRepository>();
        services.AddCustomMassTransit();
    })
    .Build();

await host.RunAsync();
