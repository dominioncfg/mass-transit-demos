using Saga.StateMachine.Initiator;

Console.Title = "State Machine Saga - Initiator";


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCustomMassTransit();
        services.AddHostedService<InitializeSagaBackgroundService>();
    })
    .Build();

await host.RunAsync();
