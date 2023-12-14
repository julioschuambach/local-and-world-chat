using RabbitMQ.Client;
using System.Text;

namespace Sender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sender started!");
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

                    while (true)
                    {
                        Console.Write("Message: ");

                        string message = Console.ReadLine();

                        if (message == "/exit")
                            break;

                        var messageBody = Encoding.UTF8.GetBytes(message);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish(
                            exchange: "",
                            routingKey: "Chat",
                            basicProperties: properties,
                            body: messageBody);

                        Console.WriteLine("[x] Sent message: {0}", message);
                    }
                }

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
