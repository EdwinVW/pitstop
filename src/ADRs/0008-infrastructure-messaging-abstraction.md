# ADR-0008: Custom Infrastructure.Messaging Abstraction Library

## Status
Accepted

## Date
2026-04-06

## Context
All services in the solution publish or consume messages via RabbitMQ. Rather than having each service take a direct dependency on the `RabbitMQ.Client` library, a custom abstraction library — `Infrastructure.Messaging` — is introduced. It is also published as the `Pitstop.Infrastructure.Messaging` NuGet package for independent versioning and distribution.

The library defines two interfaces:
- `IMessagePublisher` — for publishing messages to the broker
- `IMessageHandler` — for consuming messages from the broker

A concrete `RabbitMQMessagePublisher` and `RabbitMQMessageHandler` implement these interfaces against the RabbitMQ AMQP client.

## Decision
All inter-service messaging is accessed through the `IMessagePublisher` and `IMessageHandler` interfaces provided by the `Infrastructure.Messaging` library. No service takes a direct dependency on `RabbitMQ.Client`.

## Rationale
Two concerns drove this decision:

1. **Broker portability**: By depending on an interface rather than a concrete RabbitMQ implementation, the underlying message broker can be swapped (e.g., to Azure Service Bus, NATS, or another broker) by providing a new implementation of `IMessagePublisher` / `IMessageHandler`. No changes are required in the services themselves.

2. **Testability**: The interfaces can be mocked in unit and integration tests, allowing message publishing and consumption behaviour to be verified without a running RabbitMQ instance. This is essential for writing fast, isolated tests for service logic that involves messaging.

## Consequences

### Positive
- Services are decoupled from RabbitMQ-specific AMQP concepts — they depend only on a simple, domain-agnostic messaging interface.
- Switching the broker requires only a new interface implementation; all service code remains unchanged.
- Messaging behaviour is fully mockable in tests, enabling isolated unit testing of services that publish or consume events.
- Distributed as a NuGet package, allowing the abstraction to be versioned and shared independently of the services that consume it.

### Negative
- Introduces an additional layer of indirection; developers need to look at the library to understand what actually happens when a message is published or consumed.
- The abstraction may not map cleanly to all brokers (different brokers have different delivery guarantees and routing models), so portability in practice may require more than just swapping the implementation.

## Alternatives Considered
- **Direct use of `RabbitMQ.Client` in each service**: Simpler, but couples all services to RabbitMQ and makes messaging behaviour hard to mock in tests. Rejected.
- **Using MassTransit or NServiceBus**: Mature messaging frameworks that provide similar abstraction and broker portability. Would have reduced the need for a custom library, but would also introduce a significant third-party dependency and additional concepts for developers to learn. Rejected in favour of a thin, purpose-built abstraction.
