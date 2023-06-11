using ODR.LiveTiming.UploadService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Hosting;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.Configuration;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
.UseWindowsService(options =>
{
    options.ServiceName = "ODR Live Timing Service";
})
.ConfigureServices((context, services) =>
{
    var options = context.Configuration.GetSection("Options").Get<Options>();
    LoggerProviderOptions.RegisterProviderOptions<
        EventLogSettings, EventLogLoggerProvider>(services);

    services.AddSingleton<Options>(options);
    services.AddSingleton<GitUploadService>();
    services.AddHostedService<Worker>();

    // See: https://github.com/dotnet/runtime/issues/47303
    services.AddLogging(builder =>
    {
        builder.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    });
});


IHost host = builder.Build();
host.Run();