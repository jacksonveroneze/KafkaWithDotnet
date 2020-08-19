using System;
using System.Threading;
using Confluent.Kafka;

namespace KafkaWithDotnet.Consumer.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsumerConfig config = new ConsumerConfig
            {
                GroupId = "new-producer-group-3",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("new-producer");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);

                        Console.WriteLine("New item {0}", cr.Value);
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine("Error {0}", ex.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}