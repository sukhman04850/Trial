using Confluent.Kafka;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OrderService.Interfaces;
using OrderService.Model;
using System;
using System.Threading;
using static Confluent.Kafka.ConfigPropertyNames;

namespace OrderService.KafkaConsumer
{
    public class KafkaConsumers /*: IDisposable*/
    {
        private readonly IConsumer<string, string> consumer;
        private readonly IServiceScopeFactory _serviceScope;
        private readonly string topic;

        public KafkaConsumers(IConfiguration configuration, IServiceScopeFactory serviceScope)
        {
            var config = new ConsumerConfig
            {
                GroupId = "order-consumer-94",
                BootstrapServers = configuration["KafkaConfig:BootstrapServers"],
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = configuration["KafkaConfig:SaslUsername"],
                SaslPassword = configuration["KafkaConfig:SaslPassword"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            consumer = new ConsumerBuilder<string, string>(config).Build();
            topic = configuration["KafkaConfig:TopicName"] ?? throw new Exception("Error: Kafka is null");
            _serviceScope = serviceScope;
            consumer.Subscribe(topic);
        }
        public async Task ExecuteAsync(CancellationToken tok)
        {

            try
            {
                while (!tok.IsCancellationRequested)
                {

                    await Task.Yield();
                    using (var scope = _serviceScope.CreateScope())
                    {
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderInterface>();

                        try
                        {

                            var consumeResult = await Task.Run(() => consumer.Consume(tok), tok);

                            if (consumeResult != null && consumeResult.Message != null)
                            {
                                Console.WriteLine(consumeResult.Message);
                                var consumerMessage = JsonConvert.DeserializeObject<Orders>(consumeResult.Message.Value);
                                var key = consumeResult.Message.Key;

                                Console.WriteLine($"Message as Recieved by Consumer: {consumerMessage}");
                                if (consumerMessage == null)
                                {

                                }
                                else
                                {
                                    var id = consumerMessage.ProductId;
                                    var price = consumerMessage.Price;
                                    Console.WriteLine(price);
                                    var quant = consumerMessage.Quantity;
                                    Console.WriteLine(quant);


                                    var table = new Orders()
                                    {
                                        ProductId = id,
                                        Price = price * quant,
                                        Quantity = quant,
                                    };


                                    await orderService.AddOrder(table);
                                    consumer.Commit();
                                }


                                /*var order = consumerMessage;

                                if (order == null)
                                {
                                    Console.WriteLine("order object is null can't proceed forward");
                                }
                                else
                                {
                                    await orderService.AddOrder(order);
                                }

                                consumer.Commit();*/
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error : {e.Error.Reason}");
                        }
                    }
                }
            }
            finally
            {
               /* consumer.Dispose();*/
                consumer.Close();
            }
        }
        /*  public void Dispose()
        {
            consumer.Close();
            consumer.Dispose();

        }
*/


    }
}
