using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace LocalChatReceiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Local chat started!");
            Console.WriteLine();

            var server = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "user",
                Password = "Pass@123"
            };

            var connection = server.CreateConnection();
            {
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "Chat";
                    channel.ExchangeDeclare(
                    exchange: exchangeName,
                    type: ExchangeType.Direct);

                    var queueName = channel.QueueDeclare("LocalChat");
                    channel.QueueBind(
                        queue: queueName,
                        exchange: exchangeName,
                        routingKey: "Local");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine("[x] Received message: {0}", message);

                        channel.BasicAck(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    };

                    channel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);

                    Console.WriteLine("Press enter to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
