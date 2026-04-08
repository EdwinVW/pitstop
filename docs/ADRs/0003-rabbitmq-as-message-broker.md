# ADR-0003: RabbitMQ as Message Broker

## Status
Accepted

## Date
2026-04-06

## Context
The microservices in this solution communicate asynchronously via a message broker. Events published by one service (e.g., `CustomerRegistered`, `MaintenanceJobPlanned`) must be delivered to one or more consuming services (e.g., WorkshopManagementEventHandler, NotificationService, AuditlogService).

Several messaging technologies were considered:
- **RabbitMQ** — traditional message broker, push-based, queue and exchange model (AMQP)
- **Apache Kafka** — distributed commit log, pull-based, persistent event stream with replay capability
- **Cloud-native brokers** — Azure Service Bus, AWS SQS/SNS, Google Pub/Sub

## Decision
**RabbitMQ** is used as the message broker for all asynchronous inter-service communication.

## Rationale
The communication pattern in this solution is **event notification**: services publish facts (events) that other services react to. This maps naturally to the traditional broker model:

- **Push-based delivery** suits consumers that need to react to events as they arrive (e.g., send a notification, update a read model).
- **Queue semantics** provide straightforward competing-consumer load balancing and per-consumer message acknowledgement.
- **Exchange and routing** (fanout/topic) allow a single published event to be delivered to multiple independent queues, one per consuming service — without producers needing to know their consumers.

Kafka's strengths — long-term event log retention and consumer-controlled replay — are not required here. Event replay and historical state reconstruction are handled at the application level via the custom event store in WorkshopManagementAPI (SQL Server). Using Kafka for that purpose would introduce overlap and unnecessary complexity.

RabbitMQ also has a lower operational footprint and is simpler to run locally in Docker, which aligns with the educational purpose of the solution.

## Consequences

### Positive
- Clean pub/sub semantics that match the event notification use case.
- Simple local setup via the official RabbitMQ Docker image with management UI.
- Messages are not retained after acknowledgement, keeping the broker lightweight.
- Well-understood operational model (queues, exchanges, bindings).

### Negative
- No built-in event replay from the broker; consumers that are offline will miss events published during their downtime unless durable queues are configured correctly.
- Not suited for use cases requiring long-term event stream storage or event sourcing at the infrastructure level.

## Alternatives Considered
- **Apache Kafka**: Better fit for event streaming and replay, but overlaps with the application-level event store already present in WorkshopManagementAPI, and introduces higher operational complexity. Rejected because traditional broker semantics are the right fit here.
- **Cloud-native brokers (Azure Service Bus, AWS SQS/SNS)**: Would introduce a cloud-provider dependency, making the solution harder to run locally and less portable. Rejected to keep the setup self-contained.
