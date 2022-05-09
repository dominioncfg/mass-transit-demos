using Contracts.Sagas.Consumer;
using MassTransit;

namespace Saga.Consumer.Initiator;

public class InitializeSagaBackgroundService : BackgroundService
{
    private readonly ILogger<BackgroundService> _logger;
    private readonly IBus _bus;

    public InitializeSagaBackgroundService(ILogger<BackgroundService> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting Saga...");
        var command = new SubmitOrderCommand()
        {
            Id = Guid.NewGuid(),
            ClientName = "Perico Perez",
            Total = 24,
        };
        await _bus.Send(command,stoppingToken);
    }
}
