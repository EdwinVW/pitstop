# 4. Solution Strategy

## 4.1 Technology Decisions

| Decision | Rationale |
|----------|-----------|
| **Microservices architecture** | Demonstrates decomposition, independent deployability, per-service data ownership, and eventual consistency. See [ADR-0001](../ADRs/0001-microservices-architecture.md). |
| **.NET 9 / ASP.NET Core** | Author's expertise; single-stack keeps the solution accessible to .NET developers. See [ADR-0002](../ADRs/0002-dotnet-as-implementation-platform.md). |
| **RabbitMQ** | Push-based event notification with queue semantics; simpler than Kafka for the educational scope. See [ADR-0003](../ADRs/0003-rabbitmq-as-message-broker.md). |
| **SQL Server** | Single database platform avoids polyglot-persistence complexity. See [ADR-0007](../ADRs/0007-sql-server-as-single-database-platform.md). |
| **Docker / Kubernetes** | Containerisation is the standard deployment model. Kubernetes manifests enable orchestration demos. See [ADR-0010](../ADRs/0010-kubernetes-with-service-mesh.md). |

## 4.2 Architectural Patterns & Approaches

| Pattern | Where Applied | Purpose |
|---------|---------------|---------|
| **Event-Driven Architecture** | All services | Services communicate asynchronously through domain events published to RabbitMQ. This decouples services and enables eventual consistency. |
| **Domain-Driven Design (DDD)** | WorkshopManagementAPI | The core bounded context uses Aggregates (`WorkshopPlanning`), value objects, and domain events to model complex business rules. |
| **Event Sourcing** | WorkshopManagementAPI | The `WorkshopPlanning` aggregate persists its state as a sequence of domain events in a dedicated event store, rather than as current-state snapshots. See [ADR-0004](../ADRs/0004-event-sourcing-scoped-to-workshop-management.md). |
| **CQRS** | WorkshopManagementAPI | Commands are transformed into events that modify the aggregate; a separate read-model is built by the WorkshopManagementEventHandler for query purposes. |
| **CRUD** | CustomerManagementAPI, VehicleManagementAPI | Supporting bounded contexts with simple lifecycle; Entity Framework Core with code-first migrations. |
| **Messaging Abstraction** | Infrastructure.Messaging library | `IMessagePublisher` and `IMessageHandler` interfaces decouple all services from the concrete broker implementation. See [ADR-0009](../ADRs/0009-infrastructure-messaging-abstraction.md). |

## 4.3 Key Design Decisions

1. **Different design approaches per bounded context** — Workshop Management (core domain) uses DDD + event sourcing; Customer and Vehicle Management (supporting domains) use simple CRUD. This demonstrates that different services warrant different levels of design complexity.

2. **Dedicated Time Service** — Rather than using internal timers or cron jobs, a dedicated service publishes `DayHasPassed` events. This makes time-dependent behaviour fully deterministic and testable. See [ADR-0005](../ADRs/0005-time-service-for-time-progression.md).

3. **No API Gateway** — The WebApp calls backend APIs directly using Refit typed HTTP clients. This keeps the solution focused on core microservices patterns. See [ADR-0006](../ADRs/0006-no-api-gateway.md).

4. **Centralized logging with Seq** — All services use Serilog for structured logging and send log events to a central Seq server. This provides a single pane of glass for observability. See [ADR-0008](../ADRs/0008-seq-for-centralized-logging.md).

5. **Read-model in Workshop Management** — The WorkshopManagementEventHandler builds a read-model with cached Customer and Vehicle data. This ensures Workshop Management can operate autonomously even when other services are offline.
