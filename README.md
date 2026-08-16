# 🐰 RabbitMQ Playground

A comprehensive .NET reference repository demonstrating key messaging patterns, dispatching modes, exchange types, and integration libraries using **RabbitMQ** and **MassTransit**. Designed to show[...]

---

### 📚 What is RabbitMQ?

[RabbitMQ](https://www.rabbitmq.com/) is an open-source, light-weight message broker that supports multiple messaging protocols. It acts as an intermediary for efficiently routing, storing, and del[...]

> 📖 **Official Documentation:** [RabbitMQ Official Documentation](https://www.rabbitmq.com/docs)

---

### 📂 Repository Structure & Key Topic Branches

This repository is organized into distinct branches, each focusing on a specific RabbitMQ architectural concept or exchange type:

* **[1-getting-started](/HeyHarshdeep/RabbitMq-Playground/tree/master)** — *RabbitMQ & .NET Fundamentals*
  * Covers basic connectivity, producer/consumer setups, and establishing initial channels.
  * 📘 **Docs:** [RabbitMQ .NET Client Tutorial](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet)

* **[2-consumer-acknowledgements](/HeyHarshdeep/RabbitMq-Playground/tree/Consumer-Ackowledge)** — *Reliability & Message Delivery*
  * Demonstrates manual vs. automatic acknowledgements (ACK / NACK / Reject) for message safety.
  * 📘 **Docs:** [Consumer Acknowledgements Guide](https://www.rabbitmq.com/docs/confirms)

* **[3-message-dispatching-modes](/HeyHarshdeep/RabbitMq-Playground/tree/Message-Dispatching-Modes)** — *Work Queues & Load Distribution*
  * Implements round-robin dispatching and fair dispatching (`basicQos` prefetch configurations).
  * 📘 **Docs:** [Work Queues Tutorial](https://www.rabbitmq.com/tutorials/tutorial-two-dotnet)

* **[4-direct-exchange](/HeyHarshdeep/RabbitMq-Playground/tree/Direct-Exchange-Type-3)** — *Direct Routing by Key*
  * Demonstrates unicast message delivery based on exact matching routing keys.
  * 📘 **Docs:** [Direct Exchange Tutorial](https://www.rabbitmq.com/tutorials/tutorial-four-dotnet)

* **[5-fanout-exchange](/HeyHarshdeep/RabbitMq-Playground/tree/Fanout-Exchange-Type-4)** — *Publish/Subscribe (Broadcast)*
  * Demonstrates broadcasting messages to all bound queues simultaneously regardless of routing key.
  * 📘 **Docs:** [Fanout Exchange Tutorial](https://www.rabbitmq.com/tutorials/tutorial-three-dotnet)

* **[6-topic-exchange](/HeyHarshdeep/RabbitMq-Playground/tree/Topic-Exchange-Type-4)** — *Pattern-Based Routing*
  * Demonstrates wildcard routing (`*` and `#`) for selective publish-subscribe filtering.
  * 📘 **Docs:** [Topic Exchange Tutorial](https://www.rabbitmq.com/tutorials/tutorial-five-dotnet)

* **[7-headers-exchange](/HeyHarshdeep/RabbitMq-Playground/tree/Header-Exchange-Type-4)** — *Attribute-Based Routing*
  * Demonstrates message routing using header attributes instead of routing keys (`x-match: all/any`).
  * 📘 **Docs:** [Headers Exchange Guide](https://www.rabbitmq.com/docs/exchanges#headers)

* **[8-masstransit](/HeyHarshdeep/RabbitMq-Playground/tree/master)** — *Distributed Application Framework*
  * Integrates [MassTransit](https://masstransit.io/) abstraction over RabbitMQ for production-grade event-driven microservices.
  * 📘 **Docs:** [MassTransit RabbitMQ Documentation](https://masstransit.io/documentation/transports/rabbitmq)

---

### 💻 Tech Stack & Tools

* **Language/Framework:** .NET 8 / C#
* **Message Broker:** RabbitMQ / Amazon MQ
* **Abstraction Framework:** MassTransit
* **Client SDK:** `RabbitMQ.Client`
