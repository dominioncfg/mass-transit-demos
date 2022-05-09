using Contracts.Sagas.Consumer;
using MassTransit;

namespace Saga.Consumer.SecondService;

public class CancelOrderCommandConsumer : IConsumer<CancelOrderCommand>
{
    readonly ILogger<CancelOrderCommandConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public CancelOrderCommandConsumer(ILogger<CancelOrderCommandConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<CancelOrderCommand> context)
    {
        _logger.LogInformation("Received {Type}: {Text}", nameof(CancelOrderCommand), context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);

        var eventToPublish = new OrderCancelledEvent()
        {
            Id = context.Message.Id,
        };
        await context.Publish(eventToPublish);
    }
}
