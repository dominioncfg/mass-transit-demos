namespace Rpc.Server;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Server";
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddTransient<IObjectsRepository,InMemoryObjectsRepository>();
               services.AddCustomMassTransit();
           });
    }
}