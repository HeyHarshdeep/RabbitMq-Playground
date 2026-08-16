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


var exchangeName = "weather_headers";
channel.ExchangeDeclare(exchangeName, ExchangeType.Headers);

string? message = null;
string? location = null;
string? temperature = null;

do
{
    Console.WriteLine("Enter Message.");
    message = Console.ReadLine();


    Console.WriteLine("Enter Location");
    location = Console.ReadLine();

    Console.WriteLine("Enter Temperature");
    temperature = Console.ReadLine();


    if (!string.IsNullOrEmpty(message))
        SendMessage(channel, message, location, temperature);

} while (!string.IsNullOrEmpty(message));

void SendMessage(IModel channel, string message, string? location, string? temperature)
{
    var body = Encoding.UTF8.GetBytes(message);
    var basicProps = channel.CreateBasicProperties();

    // Only add headers that have values. Use byte[] for header values so they are encoded consistently.
    var headers = new Dictionary<string, object>();
    if (!string.IsNullOrEmpty(location))
        headers.Add("location", Encoding.UTF8.GetBytes(location));
    if (!string.IsNullOrEmpty(temperature))
        headers.Add("temperature", Encoding.UTF8.GetBytes(temperature));

    if (headers.Count > 0)
        basicProps.Headers = headers;

    channel.BasicPublish(exchange: exchangeName,
        routingKey: string.Empty,
        basicProperties: basicProps,
        body: body);
    Console.WriteLine($" [x] Sent {message}");
}
