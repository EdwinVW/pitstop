# ADR-0003: Event Sourcing Scoped to Workshop Management

## Status
Accepted

## Date
2026-04-06

## Context
The solution contains three core domain services: CustomerManagement, VehicleManagement, and WorkshopManagement. Each service owns its data and encapsulates its domain logic, but the complexity and nature of that logic differs significantly across these services.

A uniform approach to data management and domain modelling could have been applied across all services for consistency. Alternatively, each service could be designed fit-for-purpose based on the complexity and characteristics of its domain.

## Decision
**Event Sourcing and Domain-Driven Design (DDD)** are applied exclusively to `WorkshopManagementAPI`. `CustomerManagementAPI` and `VehicleManagementAPI` use a straightforward CRUD approach with Entity Framework Core.

## Rationale
Workshop Management is the core bounded context of the domain — it is where the primary business activity (planning and executing vehicle maintenance) takes place and where the most business rules live. This justifies the investment in a richer design:

- Multiple business rules govern when and how maintenance jobs can be planned (`WorkshopPlanningRules`, `MaintenanceJobRules`).
- The state of a `WorkshopPlanning` aggregate evolves through a meaningful sequence of events (`MaintenanceJobPlanned`, `MaintenanceJobFinished`, etc.) that are worth preserving as first-class facts.
- DDD patterns (aggregates, value objects, domain exceptions) provide the right tools to model and protect this complexity.

Customer and Vehicle Management, by contrast, are supporting contexts with simple lifecycle semantics (register, update). There are no complex business rules, no meaningful state transitions, and no value in storing a history of changes. A CRUD approach with EF Core is the right fit for these services.

This contrast is also intentional from a pedagogical standpoint: the solution demonstrates that within a single microservices system, **different services warrant different design approaches**. There is no one-size-fits-all answer — the design should be fit-for-purpose per service.

## Consequences

### Positive
- Each service is designed at the level of complexity its domain actually requires, avoiding both under-engineering (missing business rule protection) and over-engineering (unnecessary event sourcing in CRUD services).
- WorkshopManagementAPI serves as a concrete example of how to implement DDD, CQRS, and event sourcing in .NET.
- The contrast between services illustrates to developers that architectural patterns are tools to be applied selectively, not universally.

### Negative
- The solution is less uniform: developers need to understand two distinct design styles when working across services.
- The WorkshopManagementAPI is significantly more complex to understand and extend than the CRUD services, which may steepen the learning curve for that specific service.

## Alternatives Considered
- **Event sourcing across all services**: Would impose unnecessary complexity on CustomerManagement and VehicleManagement, which have no need for event history or complex state transitions. Rejected as over-engineering for those contexts.
- **CRUD across all services**: Would fail to adequately model the business rules and state transitions in WorkshopManagement, and would miss the opportunity to demonstrate advanced DDD/event sourcing patterns. Rejected as under-engineering for that context.
