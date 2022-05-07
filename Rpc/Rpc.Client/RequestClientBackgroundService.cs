using MassTransit;
using Contracts;

namespace Rpc.Client;

public class RequestClientBackgroundService : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<RequestClientBackgroundService> _logger;

    public RequestClientBackgroundService(IBus bus, ILogger<RequestClientBackgroundService> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await SendMessagesUnlessStopped(cancellationToken);
    }

    private async Task SendMessagesUnlessStopped(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Sending Two Messages...");

            var existUserQuery = SendExistUserQuery(cancellationToken);
            var userResponse = SendGetUserByIdQuery(cancellationToken);

            await Task.WhenAll(existUserQuery, userResponse);

            _logger.LogInformation("Received Server Response {MessageType} with Content {Content}", existUserQuery.Result.GetType().Name, existUserQuery.Result);
            _logger.LogInformation("Received Server Response {MessageType} with Content {Content}", userResponse.Result.GetType().Name, userResponse.Result);

            var random = new Random();
            await Task.Delay(random.Next(200, 1000), cancellationToken);
        }
    }

    private async Task<GetUserQueryResponse> SendGetUserByIdQuery(CancellationToken cancellationToken)
    {
        var message = new GetUserQuery
        {
            Id = Guid.NewGuid(),
        };
        _logger.LogInformation("Sending message of type {MessageType} with content {Message}", message.GetType().Name, message);
        var client = _bus.CreateRequestClient<GetUserQuery>();
        var response = await client.GetResponse<GetUserQueryResponse>(message, cancellationToken);
        _logger.LogInformation("Received response from server of type {MessageType} with content {Message}", response.Message.GetType().Name, response.Message);
        return response.Message;
    }

    private async Task<ExistUserQueryResponse> SendExistUserQuery(CancellationToken cancellationToken)
    {
        var message = new ExistUserQuery
        {
            Id = Guid.NewGuid(),
        };

        var client = _bus.CreateRequestClient<ExistUserQuery>();
        _logger.LogInformation("Sending message of type {MessageType} with content {Message}", message.GetType().Name, message);
        var response = await client.GetResponse<ExistUserQueryResponse>(message, cancellationToken);
        _logger.LogInformation("Received response from server of type {MessageType} with content {Message}", response.Message.GetType().Name, response.Message);

        return response.Message;
    }
}

