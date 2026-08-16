RabbitMQ Direct Exchange (Compact Reference)

Overview
- RabbitMQ is a message broker that routes messages from producers to consumers via exchanges and queues.
- A direct exchange routes messages to queues based on exact matching of a routing key.

Direct exchange (quick summary)
- Exchange type: `direct`.
- Routing behaviour: messages published with routing key `k` are delivered to all queues bound to the exchange with the same routing key `k`.
- Useful for: targeted delivery, multiple queues listening for specific keys (e.g., `sydney`, `brisbane`).

Project: RabbitMq-Playground (Direct exchange example)
This repository contains a set of small console apps and examples demonstrating RabbitMQ usage across different exchange types and dispatch modes:
- `Send` — a producer that publishes messages with a routing key.
- `Receive` — a consumer that declares a queue, binds it to the `weather_direct` exchange with one or more routing keys, and processes messages.

Repository structure & topic → branch mapping
Each topic/example lives on its own branch. Use the links below to jump directly to the branch that contains the example code and README for that topic.

- 1-getting-started — master
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/master
  - Covers basic connectivity, producer/consumer setups and initial examples.

- 2-consumer-acknowledgements — Consumer-Ackowledge
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/Consumer-Ackowledge
  - Demonstrates manual vs automatic acknowledgements (ACK / NACK / Reject).

- 3-message-dispatching-modes — Message-Dispatching-Modes
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/Message-Dispatching-Modes
  - Implements work queues, fair dispatch and prefetch settings.

- 4-direct-exchange — Direct-Exchange-Type-3
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/Direct-Exchange-Type-3
  - Demonstrates direct routing by exact routing key (the direct-exchange example described here).

- 5-fanout-exchange — Fanout-Exchange-Type-4
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/Fanout-Exchange-Type-4
  - Demonstrates publish/subscribe broadcast behavior.

- 6-topic-exchange — Topic-Exchange-Type-4
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/Topic-Exchange-Type-4
  - Demonstrates wildcard/pattern-based routing.

- 7-headers-exchange — Header-Exchange-Type-4
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/Header-Exchange-Type-4
  - Demonstrates attribute-based routing using headers.

- 8-masstransit — master
  - Link: https://github.com/HeyHarshdeep/RabbitMq-Playground/tree/master
  - Integrations and examples using MassTransit on top of RabbitMQ.

How to run & test (concise)
1. Ensure RabbitMQ is running locally (management UI on 15672, broker on 5672).
2. Open multiple terminals to simulate different consumers.
3. In each consumer terminal:
   - cd Receive
   - dotnet run
   - Enter a unique queue name and comma-separated routing keys.
4. In a producer terminal:
   - cd Send
   - dotnet run
   - Enter routing key and message text.

Notes / tips
- Exchange name in code: `weather_direct`. Producer must publish to the same exchange.
- For production set durable queues/exchanges and persistent messages; consider dead-lettering and retries.
- Consumer prefetch (BasicQos) and manual ACK/Reject handling are demonstrated in the consumer sample.

Minimal file references
- Receive/Receive.cs — consumer implementation.
- Send/Send.cs — producer implementation.

This document is intentionally concise — keep it at the repo root for quick reference.
