using MassTransit;

namespace Middlewares.Sender;

public class CommandsPublisherBackgroundService : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<CommandsPublisherBackgroundService> _logger;

    public CommandsPublisherBackgroundService(IBus bus, ILogger<CommandsPublisherBackgroundService> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await SendMessagesUnlessStopped(cancellationToken);
    }

    private async Task SendMessagesUnlessStopped(CancellationToken cancellationToken)
    {
        var r = new Random();

        while (!cancellationToken.IsCancellationRequested)
        {
            var durationInSeconds = r.Next(3, 11);
            var numberOfMessagePerSecond = r.Next(1, 20);
            await SendMessagesAtGivenCadence(durationInSeconds, numberOfMessagePerSecond, cancellationToken);
        }
    }

    protected async Task SendMessagesAtGivenCadence(int durationInSeconds, int messagesPerSecond, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending {NumberOfMessage} messages per second during {Duration} seconds", messagesPerSecond, durationInSeconds);

        var random = new Random();
        var initialTime = DateTime.Now;
        while (!cancellationToken.IsCancellationRequested && (DateTime.Now - initialTime).Seconds <= durationInSeconds)
        {
            await SendCreateUserCommand(random, cancellationToken);
            await SendUpdateUserCommand(random, cancellationToken);

            var delay = 1000 / messagesPerSecond;
            await Task.Delay(delay, cancellationToken);
        }
    }

    private async Task SendCreateUserCommand(Random random, CancellationToken cancellationToken)
    {
        var entrophy = random.Next();
        var message = new Contracts.CreateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = $"FN {entrophy}",
            LastName = $"LN {entrophy}"
        };

        await _bus.Send(message, cancellationToken);
        _logger.LogInformation("Sended message of type {MessageType} with content{Message}", message.GetType().Name, message);
    }

    private async Task SendUpdateUserCommand(Random random, CancellationToken cancellationToken)
    {
        var entrophy = random.Next();
        var message = new Contracts.UpdateUserCommand
        {
            Id = Guid.NewGuid(),
            FirstName = $"FN {entrophy}",
            LastName = $"LN {entrophy}"
        };

        await _bus.Send(message, cancellationToken);
        _logger.LogInformation("Sended message of type {MessageType} with content{Message}", message.GetType().Name, message);
    }
}

