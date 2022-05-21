using MassTransit;

namespace Middlewares.Sender;

public class RandomSkipperMessageFilter<TMessage> :
    IFilter<SendContext<TMessage>> where TMessage : class
{
    public void Probe(ProbeContext context)
    {
        context.CreateScope(nameof(RandomSkipperMessageFilter<TMessage>));
    }

    public Task Send(SendContext<TMessage> context, IPipe<SendContext<TMessage>> next)
    {
        var random = new Random();
        
        var skip = random.Next(1, 11) % 5 == 0;
        context.Headers.Set("skip_this_message", skip);
        return next.Send(context);
    }
}