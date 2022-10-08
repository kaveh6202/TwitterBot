using TwitterFeed.Worker;
using TwitterFeed.Bootstrapper;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Debug()
       .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
       .Enrich.FromLogContext()
       .WriteTo.Console()
       //.WriteTo.EventLog(ServiceName, ServiceName, restrictedToMinimumLevel: LogEventLevel.Warning, manageEventSource: true)
       //.WriteTo.File($"{Path.Combine(Environment.CurrentDirectory, "MyErrorLogs-{Date}.txt")}", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error)
       .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddConsole();
    })
    .Build();

await host.RunAsync();
