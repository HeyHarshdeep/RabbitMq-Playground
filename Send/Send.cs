using RabbitMQ.Client;
using System.Text;
using System;

var factory = new ConnectionFactory
{
    // Use the AMQP host and port (default 5672). The management UI runs on 15672 and is not the broker endpoint.
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

string? message = null;
do
{
    Console.WriteLine("Enter Message. Press [enter] to exit.");
    message = Console.ReadLine();
    if (!string.IsNullOrEmpty(message))
        SendMessage(message, channel);
} while (!string.IsNullOrEmpty(message));

void SendMessage(string s, IModel channel)
{
    var body = Encoding.UTF8.GetBytes(s);

    channel.BasicPublish(exchange: string.Empty,
        routingKey: "hello",
        basicProperties: null,
        body: body);
    Console.WriteLine($" [x] Sent {s}");
}