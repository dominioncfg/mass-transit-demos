using Contracts;
using MassTransit;

namespace PublishSubcribe.Subscriber;

public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
{
    readonly ILogger<UserUpdatedEventConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public UserUpdatedEventConsumer(ILogger<UserUpdatedEventConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);
    }
}
