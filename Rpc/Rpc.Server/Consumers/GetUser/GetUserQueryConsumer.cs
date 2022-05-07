using Contracts;
using MassTransit;

namespace Rpc.Server;

public class GetUserQueryConsumer : IConsumer<GetUserQuery>
{
    readonly ILogger<GetUserQueryConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public GetUserQueryConsumer(ILogger<GetUserQueryConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<GetUserQuery> context)
    {
        _logger.LogInformation("Received Text: {Text}", context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);

        var entrophy = random.Next();
        var response = new GetUserQueryResponse()
        {
            Id = context.Message.Id,
            FirstName = $"FN {entrophy}",
            LastName = $"LN {entrophy}",

        };
        await context.RespondAsync(response);
    }
}
