using Contracts.Sagas.Consumer;
using MassTransit;

namespace Saga.Consumer.ThirdService;

public class RequestOrderFulfillmentCommandConsumer : IConsumer<RequestOrderFulfillmentCommand>
{
    readonly ILogger<RequestOrderFulfillmentCommandConsumer> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public RequestOrderFulfillmentCommandConsumer(ILogger<RequestOrderFulfillmentCommandConsumer> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task Consume(ConsumeContext<RequestOrderFulfillmentCommand> context)
    {
        _logger.LogInformation("Received {Type}: {Text}", nameof(CancelOrderCommand), context.Message);
        _objectsRepository.Add(context.Message);
        var random = new Random();
        var delayFor = random.Next(200, 1000);
        await Task.Delay(delayFor);

        var fail = random.Next(1, 11) == 3;

        if(!fail)
        {
            var eventToPublish = new OrderFulfilledEvent()
            {
                Id = context.Message.Id,
            };
            await context.Publish(eventToPublish);
            return;
        }

        var failEvent = new OrderFulfillmentFailedEvent()
        {
            Id = context.Message.Id,
        };
        await context.Publish(failEvent);
        return;
    }
}
