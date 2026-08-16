Header Exchange demo (RabbitMQ)

Overview

This repository contains a minimal demo showing how RabbitMQ "headers" exchanges work using two small .NET console applications:

- `Send` — publishes messages to a headers exchange named `weather_headers`. It attaches two headers to each message: `location` and `temperature` (both encoded as UTF-8 byte arrays).
- `Receive` — declares a queue, binds it to the `weather_headers` exchange with a set of binding headers and an `x-match` rule (`any` or `all`), and consumes messages using manual acknowledgements.

What is a headers exchange?

A headers exchange routes messages based on message headers instead of the routing key. When binding a queue to a headers exchange you provide a map of header keys and values plus an `x-match` policy:

- `x-match = all` — route the message only when all provided header keys match the values on the message.
- `x-match = any` — route the message when any one of the provided header key/value pairs matches.

Important: header matching is performed against the value representation stored on the message. In this sample both sender and receiver use UTF-8 byte arrays for header values to ensure consistent matching.

How the code works (brief)

- `Send` prompts for `Message`, `Location`, `Temperature`. It creates message properties and, when headers are provided, adds `location` and `temperature` headers as `byte[]` (UTF-8) and publishes to the `weather_headers` exchange.
- `Receive` prompts for a queue name, `x-match` (Any/All), `Location`, and `Temperature`. It declares the queue, ensures the `weather_headers` exchange exists, builds a binding header map (encoding values as UTF-8 byte arrays to match the sender), and calls `QueueBind(...)` with that header map.
- The consumer processes messages with manual acks. If message body contains the literal `exception`, the message is rejected (and an exception is thrown) to demonstrate error handling. If the message body parses as an integer the consumer sleeps for that many milliseconds to simulate work.

How to run

1. Ensure RabbitMQ is running on `localhost:5672` with guest/guest credentials (or edit the connection settings in source files).
2. From the solution root run the apps separately:
   - `dotnet run --project Receive` (follow prompts to create a queue and binding)
   - `dotnet run --project Send` (publish messages and headers)

Examples

- Receiver: `x-match = all`, `location = nyc`, `temperature = 30` -> only messages containing both headers with those exact values will be routed to the queue.
- Receiver: `x-match = any`, `location = london` -> messages that include `location = london` OR the other configured header will be routed.

Notes and caveats

- Header values must match exactly (same encoding and byte representation). This demo encodes header values as UTF-8 byte arrays on both sides to make matching predictable.
- This is educational sample code, not production code. It uses console input and simple error handling.
- For more advanced scenarios consider explicit header typing or a consistent serialization strategy.

If you want, I can add examples demonstrating failing/succeeding matches or a small helper to centralize header encoding and parsing.
