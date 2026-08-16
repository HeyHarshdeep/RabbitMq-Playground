using RabbitMQ.Client;
using System.Text;
using System;
using RabbitMQ.Client.Events;
using System.Threading;

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

Console.WriteLine("Please enter queue name");
var queueName = Console.ReadLine();

if (string.IsNullOrEmpty(queueName))
{
    Console.WriteLine("Queue name is required.");
    return;
}

channel.QueueDeclare(queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

// Ensure the headers exchange exists before binding.
var exchangeName = "weather_headers";
channel.ExchangeDeclare(exchangeName, ExchangeType.Headers);

Console.WriteLine("Enter Header Match Type. It could be Any or All");
var match = Console.ReadLine();
if (string.IsNullOrEmpty(match))
    match = "all";
match = match.Trim().ToLowerInvariant();
if (match != "any" && match != "all")
    match = "all";

Console.WriteLine("Enter Location");
var location = Console.ReadLine();

Console.WriteLine("Enter Temperature");
var temperature = Console.ReadLine();


var bindingHeaders = new Dictionary<string, object>()
{
    {"x-match", match },
};

// Use the same byte[] encoding for header values as the sender so matching works.
if (!string.IsNullOrEmpty(location))
    bindingHeaders.Add("location", Encoding.UTF8.GetBytes(location));
if (!string.IsNullOrEmpty(temperature))
    bindingHeaders.Add("temperature", Encoding.UTF8.GetBytes(temperature));

// Bind the queue to the headers exchange with the provided headers to match.
channel.QueueBind(queueName, exchangeName, string.Empty, bindingHeaders);


channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");

    if (message.Contains("exception"))
    {
        Console.WriteLine("Error in processing");
        channel.BasicReject(ea.DeliveryTag, false);
        throw new Exception("Error in processing");
    }

    if (int.TryParse(message, out var delayTime))
        Thread.Sleep(delayTime);


    Console.WriteLine($"Processed messaege {message}");
    channel.BasicAck(deliveryTag: ea.DeliveryTag, false);
};
channel.BasicConsume(queue: queueName,
    autoAck: false,
    consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();