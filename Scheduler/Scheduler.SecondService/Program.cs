using Scheduler.SecondService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCustomMassTransit();
        services.AddTransient<IObjectsRepository, InMemoryObjectsRepository>();
        services.AddHostedService<ScheduleServiceBackgroundService>();
    })
    .Build();

await host.RunAsync();
