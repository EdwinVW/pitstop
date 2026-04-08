# ADR-0006: No API Gateway

## Status
Accepted

## Date
2026-04-06

## Context
The `WebApp` (ASP.NET Core MVC frontend) communicates with three backend services — CustomerManagementAPI, VehicleManagementAPI, and WorkshopManagementAPI — using Refit typed HTTP clients. In a production microservices system, an API gateway or Backend for Frontend (BFF) is commonly placed between the frontend and the backend services to centralise cross-cutting concerns such as authentication, SSL termination, rate limiting, request aggregation, and routing.

The Kubernetes deployment does include Istio VirtualService and DestinationRule resources, which provide infrastructure-level traffic routing, but these do not constitute an application-level API gateway.

## Decision
No API gateway or BFF layer is introduced. The `WebApp` calls backend service APIs directly.

## Rationale
The decision was deliberate to keep the solution **focused on its core teaching objectives**: microservices decomposition, event-driven communication, DDD, and container-based deployment. Introducing an API gateway would add another moving part — another service to configure, deploy, and understand — without contributing to these core goals.

The solution already demonstrates a significant number of architectural patterns and technologies. Adding an API gateway would broaden the scope without deepening the understanding of the patterns already covered.

## Consequences

### Positive
- The solution remains focused and approachable; learners are not distracted by gateway configuration.
- Fewer services to run, configure, and understand locally.

### Negative
- Cross-cutting concerns (authentication, rate limiting, SSL termination) are not demonstrated in a centralised way.
- The `WebApp` holds direct references to all backend service base URLs, which in a production system would be a concern for security and maintainability.
- Request aggregation (combining multiple backend calls into one frontend call) is handled ad-hoc in the WebApp rather than in a dedicated layer.

## Alternatives Considered
- **API Gateway (e.g., YARP, Ocelot, Kong, Nginx, Istio Ingress Gateway)**: Would centralise routing, authentication, and cross-cutting concerns. Appropriate for production systems. Rejected here to keep the solution focused on its primary educational goals.
- **Backend for Frontend (BFF)**: A tailored API layer for the WebApp that aggregates backend calls. Useful for reducing chatty frontend-to-backend communication. Rejected for the same reason — out of scope for this reference implementation.
