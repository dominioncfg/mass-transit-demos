using Saga.StateMachine.SecondService;

Console.Title = "State Machine Saga - Second Service";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IObjectsRepository,InMemoryObjectsRepository>();
        services.AddCustomMassTransit();
    })
    .Build();

await host.RunAsync();
