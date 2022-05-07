using Contracts;
using MassTransit;

namespace Rpc.Server;

public class ExistUserQueryConsumer : IConsumer<ExistUserQuery>
{
    readonly ILogger<ExistUserQueryConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public ExistUserQueryConsumer(ILogger<ExistUserQueryConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<ExistUserQuery> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);

        var exist = random.Next() % 2 == 0;
        var response = new ExistUserQueryResponse()
        {
            Exist = exist,
        };
        await context.RespondAsync(response);
    }
}
