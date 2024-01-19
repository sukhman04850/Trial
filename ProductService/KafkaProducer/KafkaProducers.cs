using Confluent.Kafka;
using Newtonsoft.Json;
namespace ProductService.KafkaProducer
{
    public class KafkaProducers
    {
        private readonly IProducer<string, string> producer;
        private readonly string topic;

        public KafkaProducers(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["KafkaConfig:BootstrapServers"],
                
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = configuration["KafkaConfig:SaslUsername"],
                SaslPassword = configuration["KafkaConfig:SaslPassword"]
            };

            producer = new ProducerBuilder<string, string>(config).Build();
            topic = configuration["KafkaConfig:TopicName"] ?? throw new Exception("Error: TOpic Is Null");
           
        }

        public async Task Message(string key, int productId, float productPrice, int quantity)
        {
            var realMessage = new { ProductID = productId, Price = productPrice, Quantity = quantity };
            var newKey = JsonConvert.SerializeObject(key);
            var newMessage = JsonConvert.SerializeObject(realMessage);

            await producer.ProduceAsync(topic, new Message<string, string> { Key = newKey, Value = newMessage });
            Console.WriteLine("This is the message for Kafka:" + key + realMessage);
        }

    }
}
