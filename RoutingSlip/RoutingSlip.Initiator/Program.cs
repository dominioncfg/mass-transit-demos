using RoutingSlip.Initiator;

Console.Title = "Routing Slip - Initiator";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IObjectsRepository,InMemoryObjectsRepository>();
        services.AddCustomMassTransit ();
        services.AddHostedService<RunRoutingSlipBackgroundService>();
    })
    .Build();

await host.RunAsync();
