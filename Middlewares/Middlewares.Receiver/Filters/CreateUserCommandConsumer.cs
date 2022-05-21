using MassTransit;

namespace Middlewares.Receiver;

public class SkipMessageIfSkipHeaderExistFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope(nameof(SkipMessageIfSkipHeaderExistFilter<T>));
    }

    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        if (context.Headers.Get<bool>("skip_this_message", false) ?? false)
        {
            return Task.CompletedTask;
        }
        return next.Send(context);
    }
}