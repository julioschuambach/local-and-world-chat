using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorldChatReceiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("World chat started!");
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
                    channel.QueueDeclare(
                        queue: "Chat",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.BasicQos(
                        prefetchSize: 0,
                        prefetchCount: 1,
                        global: false);

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
                        queue: "Chat",
                        autoAck: false,
                        consumer: consumer);

                    Console.WriteLine("Press enter to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
