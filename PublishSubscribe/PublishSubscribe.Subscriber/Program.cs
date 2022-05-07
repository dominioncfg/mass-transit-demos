namespace PublishSubcribe.Subscriber;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Subscriber";
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddCustomMassTransit();
               services.AddTransient<IObjectsRepository, InMemoryObjectsRepository>();
           });
    }

}
