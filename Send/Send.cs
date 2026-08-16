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


var exchangeName = "weather_fanout";
channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

string? message = null;
do
{
    Console.WriteLine("Enter Message. Press [enter] to exit.");
    message = Console.ReadLine();


    if (!string.IsNullOrEmpty(message))
        SendMessage(channel, message);
} while (!string.IsNullOrEmpty(message));

void SendMessage(IModel channel, string message)
{
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: exchangeName,
        routingKey: string.Empty,
        basicProperties: null,
        body: body);
    Console.WriteLine($" [x] Sent {message}");
}