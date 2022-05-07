using MassTransit;

namespace PublishSubcribe.Publisher;

public class EventsPublisherBackgroundService : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<EventsPublisherBackgroundService> _logger;

    public EventsPublisherBackgroundService(IBus bus, ILogger<EventsPublisherBackgroundService> logger)
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
            await SendUserCreatedEvent(random, cancellationToken);
            await SendUserUpdatedEvent(random, cancellationToken);

            var delay = 1000 / messagesPerSecond;
            await Task.Delay(delay, cancellationToken);
        }
    }

    private async Task SendUserCreatedEvent(Random random, CancellationToken cancellationToken)
    {
        var entrophy = random.Next();
        var message = new Contracts.UserCreatedEvent
        {
            Id = Guid.NewGuid(),
            FirstName = $"FN {entrophy}",
            LastName = $"LN {entrophy}"
        };

        await _bus.Publish(message, cancellationToken);
        _logger.LogInformation("Sended message of type {MessageType} with content{Message}", message.GetType().Name, message);
    }

    private async Task SendUserUpdatedEvent(Random random, CancellationToken cancellationToken)
    {
        var entrophy = random.Next();
        var message = new Contracts.UserUpdatedEvent
        {
            Id = Guid.NewGuid(),
            FirstName = $"FN {entrophy}",
            LastName = $"LN {entrophy}"
        };

        await _bus.Publish(message, cancellationToken);
        _logger.LogInformation("Sended message of type {MessageType} with content{Message}", message.GetType().Name, message);
    }
}

