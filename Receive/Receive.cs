using RabbitMQ.Client;
using System.Text;
using System;
using System.Threading;
using RabbitMQ.Client.Events;

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

// Declare the fanout exchange used by producer/consumers
channel.ExchangeDeclare(exchange: "weather_fanout", type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);

Console.WriteLine("Please enter queue name");
var queueName = Console.ReadLine();

channel.QueueDeclare(queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);


// For fanout exchanges the routing key is ignored; bind the queue to the exchange
channel.QueueBind(queueName, "weather_fanout", string.Empty);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" [*] Waiting for messages (fanout exchange).");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");

    //this are just for some testing and has no relation with fanout
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