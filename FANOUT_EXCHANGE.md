RabbitMQ — Fanout Exchange (Compact Reference)

Overview
- RabbitMQ is a message broker that routes messages from producers to consumers via exchanges and queues.
- A fanout exchange broadcasts messages to all queues bound to it, ignoring routing keys.

Fanout exchange (quick summary)
- Exchange type: `fanout`.
- Routing behaviour: messages published to the exchange are delivered to every bound queue.
- Useful for: broadcast scenarios, e.g., logs, notifications to multiple consumers.

Project: RabbitMq-Playground (Fanout exchange example)
This branch demonstrates the fanout exchange. The `Receive` consumer below is updated to support fanout:
- The code declares the `weather_fanout` exchange (type `fanout`).
- A named queue is declared and bound to the exchange using an empty routing key (routing key is ignored for fanout).
- The consumer processes messages with explicit ack/reject and uses prefetch to process one message at a time.

Key implementation details (from `Receive/Receive.cs`)
- Connection config uses default RabbitMQ host/port and credentials (`localhost:5672`, `guest/guest`).
- `channel.ExchangeDeclare("weather_fanout", ExchangeType.Fanout, ...)` ensures the exchange exists.
- The consumer asks for a queue name.
- `channel.QueueDeclare(...)` creates the named queue (non-durable, non-exclusive, no auto-delete).
- `channel.QueueBind(queueName, "weather_fanout", string.Empty)` binds the queue. For fanout exchanges the routing key is ignored.
- Prefetch set with `channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false)` to process one message at a time.
- Consumer is `EventingBasicConsumer`:
  - On `Received` it decodes message body with `UTF-8`.
  - If the message contains the text `exception` it writes an error, calls `channel.BasicReject(ea.DeliveryTag, false)` (reject without requeue) and throws an exception.
  - If the message is a parsable integer the consumer sleeps that many milliseconds (simulates work): `Thread.Sleep(delayTime)`.
  - On successful processing the consumer sends `channel.BasicAck(ea.DeliveryTag, false)`.

How to run & test (concise)
1. Ensure RabbitMQ is running locally (default management UI on 15672, broker on 5672).
2. Open multiple terminals to simulate different consumers (fanout broadcasts to all bound queues).
3. In each consumer terminal:
   - `cd Receive` then `dotnet run`.
   - Enter a unique queue name (e.g., `q1`, `q2`).
4. In a producer terminal:
   - `cd Send` then `dotnet run`.
   - Enter message(s) when prompted; routing key is ignored by fanout exchange.
5. Expected behaviour:
   - All active consumers with queues bound to `weather_fanout` receive every message the producer publishes.
   - If a consumer's handler throws (message contains `exception`) the message will be rejected and not requeued.
   - Integer messages cause the consumer to sleep that many milliseconds (simulate work) before acknowledging.

Notes / tips
- Exchange name in code is `weather_fanout` — the producer must publish to the same exchange.
- For durable/production usage set durable queues/exchanges and persistent messages.
- Consider using non-throwing error handling inside the `Received` handler and implement dead-lettering or retries if needed.

File references
- `Receive/Receive.cs` — consumer implementation for fanout exchange.
- `Send/Send.cs` — producer; publish to `weather_fanout` (routing key ignored).

Place this file at the repo root for quick reference on the fanout branch.
