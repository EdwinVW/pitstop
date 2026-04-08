# ADR-0001: Microservices Architecture

## Status
Accepted

## Date
2026-04-06

## Context
Pitstop is a garage management system of modest size. The functional scope — managing customers, vehicles, workshop planning, invoicing, notifications, and audit logging — could reasonably be implemented as a single deployable unit. A modular monolith would reduce operational complexity while still allowing internal separation of concerns.

However, the primary purpose of this solution is to serve as a **reference implementation** that teaches developers how to design and build microservices systems. This educational goal shapes the architecture independently of what the business size alone would justify.

## Decision
The application is structured as a **microservices architecture**, with each functional domain (Customer Management, Vehicle Management, Workshop Management, Invoice, Notification, Audit Log, Time) implemented as an independent service with its own database and deployment unit.

## Rationale
The solution is explicitly designed to demonstrate microservices patterns in practice, including:
- Event-driven communication via a message broker
- Independent deployability per service
- Per-service data ownership and isolation
- Eventual consistency between services
- Container-based deployment with Docker and Kubernetes

The architectural complexity (multiple databases, distributed tracing, eventual consistency) is intentional: it provides realistic learning material that a trivially simple example would not.

## Consequences

### Positive
- Serves its educational purpose by demonstrating real-world microservices patterns and trade-offs.
- Each service can be studied, extended, or replaced independently.
- Provides working examples of Docker, Kubernetes, service mesh (Istio/Linkerd), and centralized logging (Seq).

### Negative
- The operational overhead (10+ containers locally, distributed consistency concerns, complex startup dependencies) is disproportionate to the functional scope of the application.
- Not a suitable architectural template for teams building small-to-medium applications where a modular monolith would be more appropriate.

## Alternatives Considered
- **Modular monolith**: Would be the right choice for a production system of this size, offering clear module boundaries without the operational cost of distribution. Rejected here because it would not demonstrate the microservices patterns this solution aims to teach.
