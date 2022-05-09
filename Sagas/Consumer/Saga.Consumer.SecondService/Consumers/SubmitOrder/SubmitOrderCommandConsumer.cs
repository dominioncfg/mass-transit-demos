using Contracts.Sagas.Consumer;
using MassTransit;

namespace Saga.Consumer.SecondService;

public class SubmitOrderCommandConsumer : IConsumer<SubmitOrderCommand>
{
    readonly ILogger<SubmitOrderCommandConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public SubmitOrderCommandConsumer(ILogger<SubmitOrderCommandConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
    {
        _logger.LogInformation("Received {Type}: {Text}", nameof(SubmitOrderCommand), context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);

        var eventToPublish = new OrderSubmittedEvent()
        {
            Id = context.Message.Id,
            ClientName = context.Message.ClientName,
            Total = context.Message.Total,
        };
        await context.Publish(eventToPublish);
    }
}
