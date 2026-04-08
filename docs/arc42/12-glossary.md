# 12. Glossary

| Term | Definition |
|------|-----------|
| **Aggregate** | A DDD pattern. A cluster of domain objects (entities and value objects) treated as a single unit for data changes. In Pitstop, `WorkshopPlanning` is the aggregate root in the Workshop Management bounded context. |
| **Bounded Context** | A DDD concept. A logical boundary within which a particular domain model applies. Pitstop has bounded contexts for Customer Management, Vehicle Management, Workshop Management, Notification, Invoice, and Auditlog. |
| **Command** | A message that requests a state change (e.g., `RegisterCustomer`, `PlanMaintenanceJob`). Commands are sent synchronously via HTTP to the responsible API service. |
| **CQRS** | Command Query Responsibility Segregation. A pattern that separates read and write models. In Pitstop, write operations go through the event-sourced aggregate; read operations use a separate read-model built by the event handler. |
| **Domain Event** | A message representing something that happened in the domain (e.g., `CustomerRegistered`, `MaintenanceJobPlanned`). Events are published to RabbitMQ and consumed by interested services. |
| **Event Sourcing** | A persistence pattern where state is stored as a sequence of immutable events rather than current-state snapshots. The `WorkshopPlanning` aggregate is rebuilt by replaying all its events. |
| **Event Store** | The database that persists domain events. In Pitstop, this is the `WorkshopManagementEventStore` SQL Server database, used exclusively by the Workshop Management API. |
| **Eventual Consistency** | A consistency model where, after a change, the system will become consistent over time (rather than immediately). Services in Pitstop react to events asynchronously, so there is a brief period where read-models may not reflect the latest state. |
| **Fanout Exchange** | A RabbitMQ exchange type that broadcasts every message to all bound queues. Pitstop uses fanout exchanges so every subscribing service receives every event. |
| **Infrastructure.Messaging** | A shared .NET library (NuGet package) that provides the `IMessagePublisher` and `IMessageHandler` abstractions, decoupling services from the concrete message broker implementation. |
| **Maintenance Job** | A unit of work to be performed on a single vehicle. Part of a `WorkshopPlanning` (the day's schedule). |
| **Read-Model** | A denormalized data store optimized for queries. In Pitstop, the WorkshopManagementEventHandler builds a read-model with cached Customer and Vehicle data so the Workshop Management bounded context can operate autonomously. |
| **Refit** | A .NET library that generates typed REST API clients from interface definitions. Used by the WebApp to call backend services. |
| **Service Mesh** | An infrastructure layer (Istio or Linkerd) that handles service-to-service communication, providing traffic management, observability, and security features without changing application code. |
| **Workshop Planning** | The aggregate root in the Workshop Management bounded context. Represents the maintenance job schedule for a single day. |

---
[← Back to arc42 index](arc42.md)
