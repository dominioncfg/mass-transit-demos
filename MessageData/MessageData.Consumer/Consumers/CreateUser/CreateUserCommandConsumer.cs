using Contracts;
using MassTransit;
using System.Text;

namespace MessageData.Consumer;

public class CreateUserWithLargeDataCommandConsumer : IConsumer<CreateUserWithLargeDataCommand>
{
    readonly ILogger<CreateUserWithLargeDataCommandConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public CreateUserWithLargeDataCommandConsumer(ILogger<CreateUserWithLargeDataCommandConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<CreateUserWithLargeDataCommand> context)
    {
        _logger.LogInformation("Received Message...");
        if(context.Message.BigData is not null && context.Message.BigData.HasValue)
        {
            var rawContent = await context.Message.BigData.Value;
            var value = Encoding.UTF8.GetString(rawContent);
            _logger.LogInformation("Received Message with data {Char}", value.First());
        }
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(100, 200);
        await Task.Delay(delayFor);
    }
}
