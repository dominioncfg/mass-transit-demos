using MassTransit;

namespace RoutingSlip.Initiator;

public class FirstActivity : IActivity<FirstActivityArgument, FirstActivityLog>
{
    private readonly ILogger<FirstActivity> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public FirstActivity(ILogger<FirstActivity> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<FirstActivityArgument> context)
    {
        _objectsRepository.Add(new { context.Arguments.OrderId });
        _logger.LogInformation("Executing {Activity} with Order Id {Order}", nameof(FirstActivity), context.Arguments.OrderId);

        await Task.Delay(230);

        return context.Completed(new FirstActivityLog()
        {
            OrderId = context.Arguments.OrderId
        });
    }
    public async Task<CompensationResult> Compensate(CompensateContext<FirstActivityLog> context)
    {
        _objectsRepository.Add(new { context.Log.OrderId });
        _logger.LogInformation("Compensating {Activity} with Order Id {Order}", nameof(FirstActivity), context.Log.OrderId);
        await Task.Delay(230);
        return context.Compensated();
    }
}
