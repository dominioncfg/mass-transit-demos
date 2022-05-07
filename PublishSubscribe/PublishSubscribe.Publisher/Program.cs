namespace PublishSubcribe.Publisher;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Publisher";
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddCustomMassTransit();
               services.AddHostedService<EventsPublisherBackgroundService>();
           });
    }
}
