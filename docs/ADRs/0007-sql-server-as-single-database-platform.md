# ADR-0007: SQL Server as the Single Database Platform

## Status
Accepted

## Date
2026-04-06

## Context
Each microservice that requires persistence owns its own database schema, in line with the database-per-service principle. The services have varying persistence needs:

- **CustomerManagementAPI / VehicleManagementAPI**: Simple relational CRUD data.
- **WorkshopManagementAPI**: An event store — a sequence of domain events used to reconstruct aggregate state.
- **WorkshopManagementEventHandler**: A relational read model built from incoming events.
- **InvoiceService / NotificationService**: Relational reference data (customers, vehicles, maintenance jobs) consumed from events.
- **AuditlogService**: An append-only log of all system events.

Polyglot persistence — choosing the storage technology best suited to each service — could be justified: a document store for the audit log, a dedicated event database (e.g., EventStoreDB) for the event store, or a lightweight embedded database for simple read models.

## Decision
**SQL Server** is used as the database platform for all services that require persistence.

## Rationale
The primary motivation is pragmatism: using a single database technology across all services avoids introducing multiple database engines into the infrastructure, which would increase the operational footprint and the number of technologies a developer must understand to work with the solution.

From a learning perspective, the focus of this solution is on microservices architecture patterns — not on demonstrating polyglot persistence. Introducing multiple database technologies would broaden the scope without contributing to the core educational goals.

SQL Server is a well-understood, widely available relational database that can adequately serve all the persistence needs present in this solution, even where a specialised store might be a more natural fit in a production context.

## Consequences

### Positive
- Single database technology to install, configure, and operate locally (one Docker container).
- Developers only need familiarity with one database platform to work across all services.
- Keeps the infrastructure footprint and cognitive overhead of the solution to a minimum.

### Negative
- Not representative of polyglot persistence, which is a common and legitimate approach in real-world microservices systems.
- Some persistence patterns are a less natural fit for a relational database — notably the append-only audit log and the event store, which would map more naturally to a document store or a dedicated event database such as EventStoreDB.
- All services share a dependency on a single running SQL Server instance, which in production would be a scalability and isolation concern.

## Alternatives Considered
- **Polyglot persistence**: Selecting the optimal storage technology per service (e.g., EventStoreDB for the event store, a document store for the audit log). More representative of real-world microservices, but out of scope for a solution focused on other architectural patterns. Rejected to keep the infrastructure footprint small and the solution focused.
