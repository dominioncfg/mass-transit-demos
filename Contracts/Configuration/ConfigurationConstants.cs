namespace Contracts;

public class ConfigurationConstants
{
    public const string WorkerQueueName = "worker-queue";
    public const string PublishSubscribeSubscriberQueueName = "publish-subscribe-subscriber-queue";
    public const string RpcServerQueueName = "rpc-server-queue";
    public const string StateMachineSagaSecondService = "state-machine-saga-order-management-service";
    public const string StateMachineSagaThirdService = "state-machine-saga-order-fullfilment-service";
}
