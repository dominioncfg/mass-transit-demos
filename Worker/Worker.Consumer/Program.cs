using MassTransit;
using System.Reflection;

namespace Worker.Consumer;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Receiver";
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddCustomMassTransit();
               services.AddTransient<IObjectsRepository,InMemoryObjectsRepository>();
           });
    }

}
