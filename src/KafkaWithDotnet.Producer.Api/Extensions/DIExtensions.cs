using Confluent.Kafka;
using KafkaWithDotnet.Producer.Api.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaWithDotnet.Producer.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            KafkaSettings settings = new KafkaSettings();
            
            configuration.Bind("KafkaSettings", settings);
            
            services.AddTransient<ProducerConfig>(x => new ProducerConfig() {BootstrapServers = settings.Host });
        }
    }
}