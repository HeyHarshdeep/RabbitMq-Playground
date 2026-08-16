using RabbitMQ.Client;
using System.Text;
using System;
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

// Ensure the queue exists (matches the sender)
channel.QueueDeclare(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};
channel.BasicConsume(queue: "hello",
    autoAck: true,
    consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();