using MassTransit;

namespace MessageData.Sender;

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
            var numberOfMessagePerSecond = 1;
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

            var delay = 1000 / messagesPerSecond;
            await Task.Delay(delay, cancellationToken);
        }
    }

    private async Task SendCreateUserCommand(Random random, CancellationToken cancellationToken)
    {
        var entrophy = random.Next();
        var bigData = new string('*', 10000);
        var message = new
        {
            Id = Guid.NewGuid(),
            FirstName = $"FN {entrophy}",
            LastName = $"LN {entrophy}",
            BigData = bigData,
        };

        await _bus.Send<Contracts.CreateUserWithLargeDataCommand>(message, cancellationToken);
        _logger.LogInformation("Message Sended");
    }    
}

