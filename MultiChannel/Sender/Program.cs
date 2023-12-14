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
                    var exchangeName = "Chat";
                    channel.ExchangeDeclare(
                        exchange: exchangeName,
                        type: ExchangeType.Direct);

                    while (true)
                    {
                        Console.Write("Message: ");
                        string message = Console.ReadLine();

                        if (message == "/exit")
                            break;

                        var messageBody = Encoding.UTF8.GetBytes(message);

                        Console.Write("Channel: ");
                        string routingKey = Console.ReadLine();

                        channel.BasicPublish(
                            exchange: exchangeName,
                            routingKey: routingKey,
                            basicProperties: null,
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
