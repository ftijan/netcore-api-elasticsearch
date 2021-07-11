using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Example.Elasticsearch.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostBuilder, loggingBuilder) => {
                    loggingBuilder.AddConsole();
                })
                .UseSerilog((serviceProvider, loggerConfiguration) => {
                    var config = serviceProvider.Configuration;

                    loggerConfiguration.Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config["Elasticsearch:BaseUri"]))
                        {
                            IndexFormat = GetIndexFormat(serviceProvider),
                            AutoRegisterTemplate = true
                            //NumberOfShards = 2,
                            //NumberOfReplicas = 1
                        })
                        .Enrich.WithProperty("Environment", serviceProvider.HostingEnvironment.EnvironmentName)
                        .ReadFrom.Configuration(serviceProvider.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static string GetIndexFormat(HostBuilderContext serviceProvider)
        {
            var config = serviceProvider.Configuration;

            var appName = config["Elasticsearch:AppName"];
            var envName = serviceProvider.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-");
            var dateFormat = $"{DateTime.UtcNow:yyyy-MM}";

            return $"{appName}-logs-{envName}-{dateFormat}";
        }
    }
}
