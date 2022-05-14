using MassTransit;
using MassTransitRoutingSlip = MassTransit.Courier.Contracts.RoutingSlip;

namespace RoutingSlip.Initiator;

public class RunRoutingSlipBackgroundService : BackgroundService
{
    private readonly ILogger<BackgroundService> _logger;
    private readonly IBus _bus;

    public RunRoutingSlipBackgroundService(ILogger<RunRoutingSlipBackgroundService> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var orderId = Guid.NewGuid();
        _logger.LogInformation("Starting Routing Slip for order {OrderId}...", orderId);

        var routingSlip = BuildRoutingSlip(orderId);

        await _bus.Execute(routingSlip);
    }

    private static MassTransitRoutingSlip BuildRoutingSlip(Guid orderId)
    {
        var trakingNumber = NewId.NextGuid();

        var builder = new RoutingSlipBuilder(trakingNumber);

        builder.AddActivity("FirstActivity", new Uri("rabbitmq://localhost/first_execute"));
        builder.AddActivity("SecondActivity", new Uri("rabbitmq://localhost/second_execute"));
        builder.AddActivity("ThirdActivity", new Uri("rabbitmq://localhost/third_execute"));

        builder.AddVariable("OrderId", orderId);

        return builder.Build();
    }
}
