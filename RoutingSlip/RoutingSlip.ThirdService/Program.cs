using RoutingSlip.ThirdService;

Console.Title = "Routing Slip - Third Service";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IObjectsRepository, InMemoryObjectsRepository>();
        services.AddCustomMassTransit();
    })
    .Build();

await host.RunAsync();
