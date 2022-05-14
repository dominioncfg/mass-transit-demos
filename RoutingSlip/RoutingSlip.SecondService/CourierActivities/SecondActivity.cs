using MassTransit;

namespace RoutingSlip.SecondService;

public class SecondActivity : IActivity<SecondActivityArgument, SecondActivityLog>
{
    private readonly ILogger<SecondActivity> _logger;
    private readonly IObjectsRepository _objectsRepository;

    public SecondActivity(ILogger<SecondActivity> logger, IObjectsRepository objectsRepository)
    {
        _logger = logger;
        _objectsRepository = objectsRepository;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<SecondActivityArgument> context)
    {
        _objectsRepository.Add(new { context.Arguments.OrderId });
        _logger.LogInformation("Executing {Activity} with Order Id {Order}", nameof(SecondActivity), context.Arguments.OrderId);

        await Task.Delay(230);

        return context.Completed(new SecondActivityLog()
        {
            OrderId = context.Arguments.OrderId
        });
    }
    public async Task<CompensationResult> Compensate(CompensateContext<SecondActivityLog> context)
    {
        _objectsRepository.Add(new { context.Log.OrderId });
        _logger.LogInformation("Compensating {Activity} with Order Id {Order}", nameof(SecondActivity), context.Log.OrderId);
        await Task.Delay(230);
        return context.Compensated();
    }
}
