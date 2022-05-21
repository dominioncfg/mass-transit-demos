using Contracts;
using MassTransit;

namespace Worker.Consumer;

public class UpdateUserCommandConsumer : IConsumer<UpdateUserCommand>
{
    readonly ILogger<UpdateUserCommandConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public UpdateUserCommandConsumer(ILogger<UpdateUserCommandConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<UpdateUserCommand> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);
    }
}
