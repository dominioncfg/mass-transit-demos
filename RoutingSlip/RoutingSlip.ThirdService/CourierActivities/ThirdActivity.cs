using MassTransit;

namespace RoutingSlip.ThirdService;

public class ThirdActivity : IExecuteActivity<ThirdActivityArgument>
{
    private readonly ILogger<ThirdActivity> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public ThirdActivity(ILogger<ThirdActivity> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<ThirdActivityArgument> context)
    {
        _objectsRepository.Add(new { context.Arguments.OrderId });
        _logger.LogInformation("Executing {Activity} with Order Id {Order}", nameof(ThirdActivity), context.Arguments.OrderId);

        await Task.Delay(230);

        var random = new Random().Next(1, 11);

        if (random % 3 == 0)
        {
            _logger.LogInformation("Returning faulted.");
            return context.Faulted(new Exception("Ups this thing failed"));
        }

        _logger.LogInformation("Returning completed.");
        return context.Completed();
    }
}
