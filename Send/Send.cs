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

//channel.QueueDeclare(queue: "hello",
//    durable: false,
//    exclusive: false,
//    autoDelete: false,
//    arguments: null);
var exchangeName = "weather_direct";
channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

string? message = null;
do
{
    Console.WriteLine("Enter Message. Press [enter] to exit.");
    message = Console.ReadLine();

    Console.WriteLine("Please enter your routing key");
    var routingKey = Console.ReadLine();

    if (!string.IsNullOrEmpty(message))
        SendMessage(channel, message, routingKey ?? string.Empty);
} while (!string.IsNullOrEmpty(message));

void SendMessage(IModel channel, string message, string routingKey)
{
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: exchangeName,
        routingKey: routingKey,
        basicProperties: null,
        body: body);
    Console.WriteLine($" [x] Sent {message}");
}