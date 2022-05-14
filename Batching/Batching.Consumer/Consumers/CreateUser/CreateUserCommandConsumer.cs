using Contracts;
using MassTransit;

namespace Batching.Consumer;


public class CreateUserCommandConsumer : IConsumer<Batch<CreateUserCommand>>
{
    readonly ILogger<CreateUserCommandConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public CreateUserCommandConsumer(ILogger<CreateUserCommandConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<Batch<CreateUserCommand>> context)
    {
        _logger.LogInformation("Received {N} Messages: ", context.Message.Length);

        context.Message.ToList().ForEach(x => _objectsRepository.Add(x));

        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);
    }
}
