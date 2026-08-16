# RabbitMq-Playground — Topic Exchange Example

This repository demonstrates RabbitMQ Topic Exchange usage with two small .NET 8 console apps:

- `Send/Send.cs` — publisher that sends messages to a topic exchange (`weather_topic`).
- `Receive/Receive.cs` — consumer that declares a queue, binds one or more routing keys to the `weather_topic` exchange, and processes messages.

Official RabbitMQ documentation about Topic Exchanges

- Topic exchanges tutorial: https://www.rabbitmq.com/tutorials/amqp-topics.html
- Exchanges overview: https://www.rabbitmq.com/exchanges.html

How the example works (short)

- The publisher sends messages to the `weather_topic` exchange using routing keys (for example `us.east.rain`).
- The consumer creates/declares a named queue, then binds it to `weather_topic` with one or more routing keys entered at runtime. Binding keys can include wildcards (`*` and `#`) as explained in the RabbitMQ docs linked above.
- The consumer uses manual acknowledgements (`autoAck: false`) and `BasicQos` with a prefetch count of `1` so messages are processed one at a time.
- If the consumer receives a message that contains the word `exception` it rejects that message and throws an error. If the message text can be parsed as an integer it sleeps for that many milliseconds to simulate work.

Key implementation details (from `Receive/Receive.cs`)

- Connection config uses default RabbitMQ host/port and credentials (`localhost:5672`, `guest/guest`).
- The consumer prompts for a queue name and a comma-separated list of routing keys.
- `channel.QueueDeclare(...)` creates the named queue (non-durable, non-exclusive, no auto-delete).
- For each routing key provided the code calls: `channel.QueueBind(queueName, "weather_topic", key)`.
- If no routing keys are provided the queue is bound using an empty string key.
- Prefetch set with `channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false)` to process one message at a time.
- Consumer is `EventingBasicConsumer`:
  - On `Received` it decodes the message body with UTF-8 and prints it.
  - If the message contains `exception` it logs, rejects the message (no requeue) and throws an exception.
  - If the message parses as an integer the handler sleeps that many milliseconds to simulate processing time.
  - Otherwise it acknowledges the message with `channel.BasicAck(ea.DeliveryTag, false)`.

Why this matters

- Topic exchanges allow flexible routing using routing patterns and wildcards (`*` matches one word, `#` matches zero or more words). This makes them suitable for hierarchical routing schemes like `region.city.weather`.
- Manual acks and reject semantics let consumers control retries and dead-lettering policies.
- Prefetch count of `1` is useful to implement fair dispatch among multiple consumers.

Run instructions

1. Start a RabbitMQ broker on `localhost` (default port `5672`). Default credentials `guest`/`guest` are used by the samples.
2. In one terminal run the receiver and follow prompts:
   - `dotnet run --project Receive`
   - Enter a queue name (for example `weather_queue`).
   - Enter routing keys (comma separated), for example `us.*.rain,#.snow` or leave blank for binding the empty key.
3. In another terminal run the sender:
   - `dotnet run --project Send`
   - Send messages with routing keys matching the consumer bindings.

Repository code links (branch: `Topic-Exchange-Type-4`)

- `Send` source: https://github.com/HeyHarshdeep/RabbitMq-Playground/blob/Topic-Exchange-Type-4/Send/Send.cs
- `Receive` source: https://github.com/HeyHarshdeep/RabbitMq-Playground/blob/Topic-Exchange-Type-4/Receive/Receive.cs

If you want additional examples of binding keys and which routing keys match them, or Docker commands to start RabbitMQ locally, tell me what to add.
