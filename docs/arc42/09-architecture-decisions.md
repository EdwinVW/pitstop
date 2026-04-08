# 9. Architecture Decisions

Architecture decisions are recorded as Architecture Decision Records (ADRs) in the [ADRs](../ADRs/) folder.

| ADR | Title | Status | Summary |
|-----|-------|--------|---------|
| [0001](../ADRs/0001-microservices-architecture.md) | Microservices Architecture | Accepted | System is structured as microservices with independent services, per-service databases, and event-driven communication. Intentionally designed as a reference implementation despite the operational overhead being disproportionate to the functional scope. |
| [0002](../ADRs/0002-dotnet-as-implementation-platform.md) | .NET as Implementation Platform | Accepted | All services implemented in .NET/C#. Single technology stack keeps the solution accessible to target audience. |
| [0003](../ADRs/0003-rabbitmq-as-message-broker.md) | RabbitMQ as Message Broker | Accepted | RabbitMQ is the asynchronous message broker. Push-based event notification with queue semantics; Kafka's strengths (long-term replay, persistent log) aren't needed since replay is handled at application level via the event store. |
| [0004](../ADRs/0004-event-sourcing-scoped-to-workshop-management.md) | Event Sourcing Scoped to Workshop Management | Accepted | Event Sourcing and DDD applied exclusively to WorkshopManagementAPI (core domain). Customer and Vehicle Management use simple CRUD. Different services warrant different design approaches. |
| [0005](../ADRs/0005-time-service-for-time-progression.md) | Dedicated Time Service for Time Progression | Accepted | A dedicated TimeService publishes `DayHasPassed` events. All services subscribe to this rather than managing internal timers. Enables deterministic and testable time-dependent behaviour. |
| [0006](../ADRs/0006-no-api-gateway.md) | No API Gateway | Accepted | No API gateway or BFF layer. WebApp calls backend APIs directly via Refit. Keeps solution focused on core microservices patterns. |
| [0007](../ADRs/0007-sql-server-as-single-database-platform.md) | SQL Server as Single Database Platform | Accepted | All services use SQL Server. Avoids polyglot persistence complexity; keeps operational footprint and cognitive overhead minimal. |
| [0008](../ADRs/0008-seq-for-centralized-logging.md) | Seq for Centralized Logging | Accepted | Seq as centralized log aggregation server with Serilog structured logging. Single-container deployment; first-class .NET/Serilog support; demo-friendly web UI. |
| [0009](../ADRs/0009-infrastructure-messaging-abstraction.md) | Infrastructure Messaging Abstraction | Accepted | All messaging through `IMessagePublisher`/`IMessageHandler` interfaces. No direct RabbitMQ.Client dependency in services. Enables broker portability and testability. |
| [0010](../ADRs/0010-kubernetes-with-service-mesh.md) | Kubernetes with Service Mesh Support | Accepted | Kubernetes manifests for plain deployment and optional Istio/Linkerd service mesh. Supports conference talks about service meshes. |

---
[← Back to arc42 index](arc42.md)
