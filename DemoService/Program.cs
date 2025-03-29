using System;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DemoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<BusSettings>(hostContext.Configuration.GetSection("BusSettings"));
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<DemoMessageConsumer>();
                        
                        x.UsingAmazonSqs((context, cfg) =>
                        {
                            var busSettings = context.GetRequiredService<IOptions<BusSettings>>().Value;

                            cfg.Host(new Uri(busSettings.HostAddress), h =>
                            {
                                h.AccessKey(busSettings.AccessKey);
                                h.SecretKey(busSettings.SecretKey);
                                h.Config(new AmazonSimpleNotificationServiceConfig
                                {
                                    ServiceURL = busSettings.ServiceUrl,
                                    AuthenticationRegion = busSettings.Region
                                });
                                h.Config(new AmazonSQSConfig
                                {
                                    ServiceURL = busSettings.ServiceUrl,
                                    AuthenticationRegion = busSettings.Region
                                });
                            });

                            cfg.Message<DemoMessage>(mtc =>
                            {
                                mtc.SetEntityName(busSettings.TopicName);
                            });
                            
                            cfg.ReceiveEndpoint(busSettings.QueueName, ec =>
                            {
                                ec.ConfigureConsumeTopology = false;
                                ec.Subscribe(busSettings.TopicName);
                                ec.ConfigureConsumer<DemoMessageConsumer>(context);
                            });
                        });
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
