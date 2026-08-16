RabbitMQ  Direct Exchange (Compact Reference)

Overview
- RabbitMQ is a message broker that routes messages from producers to consumers via exchanges and queues.
- A direct exchange routes messages to queues based on exact matching of a routing key.

Direct exchange (quick summary)
- Exchange type: `direct`.
- Routing behaviour: messages published with routing key `k` are delivered to all queues bound to the exchange with the same routing key `k`.
- Useful for: targeted delivery, multiple queues listening for specific keys (e.g., `sydney`, `brisbane`).

Project: RabbitMq-Playground (Direct exchange example)
This repository contains two small console apps demonstrating direct-exchange usage:
- `Send`  a producer that publishes messages with a routing key.
- `Receive`  a consumer that declares a queue, binds it to the `weather_direct` exchange with one or more routing keys, and processes messages.

Key implementation details (from `Receive/Receive.cs`)
- Connection config uses default RabbitMQ host/port and credentials (`localhost:5672`, `guest/guest`).
- The consumer asks for a queue name and routing keys (comma-separated).
- `channel.QueueDeclare(...)` creates the named queue (non-durable, non-exclusive, no auto-delete).
- For each routing key provided the code does: `channel.QueueBind(queueName, "weather_direct", key)`.
- If no routing keys are provided the queue is bound using an empty string key.
- Prefetch set with `channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false)` to process one message at a time.
- Consumer is `EventingBasicConsumer`:
  - On `Received` it decodes message body with `UTF-8`.
  - If the message contains the text `exception` it writes an error, calls `channel.BasicReject(ea.DeliveryTag, false)` (reject without requeue) and throws an exception.
  - If the message is a parsable integer the consumer sleeps that many milliseconds (simulates work): `Thread.Sleep(delayTime)`.
  - On successful processing the consumer sends `channel.BasicAck(ea.DeliveryTag, false)`.

Why important
- `BasicAck` and `BasicReject(..., false)` ensure explicit acknowledgement and control over requeueing.
- PrefetchCount = 1 ensures fair dispatch: a busy consumer won't receive more messages than it can process.
- Binding by routing key demonstrates exact-match routing in direct exchanges.

How to run & test (concise)
1. Ensure RabbitMQ is running locally (default management UI on 15672, broker on 5672).
2. Open multiple terminals to simulate different consumers.
3. In each consumer terminal:
   - `cd Receive` then `dotnet run`.
   - Enter a unique queue name (e.g., `qld`, `nsw`).
   - Enter routing keys (comma-separated) � e.g., `sydney` or `brisbane`.
4. In a producer terminal:
   - `cd Send` then `dotnet run`.
   - Enter routing key when prompted and then the message text.
5. Expected behaviour:
   - Only consumers whose queues are bound with the exact routing key receive the message.
   - If a consumer sends a message `exception` the message will be rejected (not requeued) and the consumer throws an exception.
   - Integer messages cause the consumer to sleep that many milliseconds (simulate work) before acknowledging.

Notes / tips
- Exchange name in code is `weather_direct`. The producer must publish to the same exchange with appropriate routing keys.
- For durable/production usage set durable queues/exchanges and persistent messages.
- Consider handling consumer exceptions gracefully (avoid throwing from the `Received` handler) and implement dead-lettering or retry if needed.

Minimal file references
- `Receive/Receive.cs` � consumer implementation described above.
- `Send/Send.cs` � producer; publishes messages to `weather_direct` with a specified routing key.

Links & short snippets
- File links (relative, work in forks/branches):
  - [Receive/Receive.cs](Receive/Receive.cs)
  - [Send/Send.cs](Send/Send.cs)

- Quick handler excerpt (from `Receive/Receive.cs`):
  ```csharp
  // Receive/Receive.cs - message handler excerpt
  var body = ea.Body.ToArray();
  var message = Encoding.UTF8.GetString(body);
  Console.WriteLine($" [x] Received {message}");
  if (message.Contains("exception"))
  {
      channel.BasicReject(ea.DeliveryTag, false);
      // handle error / notify / dead-letter
  }
  channel.BasicAck(ea.DeliveryTag, false);
  ```

- Permalinks on GitHub (stable):
  - Use `https://github.com/<owner>/<repo>/blob/<commit-sha>/Receive/Receive.cs#L10-L30` for permanent references.
  - For convenience during development use branch links like `blob/<branch>`; prefer relative links inside the repo for README/docs.

This document is intentionally concise � keep it at the repo root for quick reference.
