using Contracts;
using MassTransit;

namespace Scheduler.SecondService;

public class ScheduleServiceBackgroundService : BackgroundService
{
    private readonly ILogger<ScheduleServiceBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ScheduleServiceBackgroundService(ILogger<ScheduleServiceBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Scheduling order...");
        using var scope = _serviceProvider.CreateScope();
        var shceduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();

        var scheduleAt = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        var shceduleMessage = new CreateUserCommand()
        {
            Id = Guid.NewGuid(),
            FirstName = "Perico",
            LastName = "Perez",
        };
        await shceduler.ScheduleSend(new Uri("queue:create-user-command"), scheduleAt, shceduleMessage,stoppingToken);
    }
}
