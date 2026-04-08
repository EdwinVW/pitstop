# 8. Cross-cutting Concepts

## 8.1 Messaging and Event-Driven Communication

All inter-service communication is asynchronous and event-driven via RabbitMQ:

- **Publishing**: Services use `IMessagePublisher.PublishMessageAsync(messageType, message, routingKey)` to publish domain events.
- **Consuming**: Background services implement `IMessageHandlerCallback.HandleMessageAsync(messageType, body)` and register with `IMessageHandler.Start(callback)`.
- **Exchange type**: Fanout — every subscribed queue receives every message.
- **Reliability**: Manual message acknowledgement (no auto-ack). Messages are only acknowledged after successful processing.
- **Retry**: Exponential backoff retry policy (9 retries, starting at 5 seconds) for both publishing and consuming.
- **Abstraction**: The `Infrastructure.Messaging` library decouples services from the concrete broker. See [ADR-0009](../ADRs/0009-infrastructure-messaging-abstraction.md).

### Domain Events

| Event | Published by | Consumed by |
|-------|-------------|-------------|
| `CustomerRegistered` | CustomerManagementAPI | WorkshopMgmtEventHandler, NotificationService, InvoiceService, AuditlogService |
| `VehicleRegistered` | VehicleManagementAPI | WorkshopMgmtEventHandler, AuditlogService |
| `WorkshopPlanningCreated` | WorkshopManagementAPI | WorkshopMgmtEventHandler, AuditlogService |
| `MaintenanceJobPlanned` | WorkshopManagementAPI | WorkshopMgmtEventHandler, NotificationService, InvoiceService, AuditlogService |
| `MaintenanceJobFinished` | WorkshopManagementAPI | WorkshopMgmtEventHandler, NotificationService, InvoiceService, AuditlogService |
| `DayHasPassed` | TimeService | NotificationService, InvoiceService, AuditlogService |

## 8.2 Persistence

### Database-per-service

Each service has its own logical database on the shared SQL Server instance:

| Service | Database | ORM / Data Access |
|---------|----------|-------------------|
| CustomerManagementAPI | `CustomerManagement` | Entity Framework Core (code-first, auto-migration) |
| VehicleManagementAPI | `VehicleManagement` | Entity Framework Core (code-first, auto-migration) |
| WorkshopManagementAPI | `WorkshopManagementEventStore` | Dapper (event store) |
| WorkshopManagementAPI / EventHandler | `WorkshopManagement` | Dapper (read-model / reference data) |
| NotificationService | `Notification` | Dapper |
| InvoiceService | `Invoice` | Dapper |
| AuditlogService | `Auditlog` | Dapper |

### Automatic Database Creation

All databases are automatically created on first startup. The event-store database (`WorkshopManagementEventStore`) is created upon the first maintenance job registration.

## 8.3 Logging and Observability

- **Framework**: [Serilog](https://serilog.net/) with structured/semantic logging.
- **Sinks**: Console and Seq server.
- **Enrichment**: Machine name is added to all log events for correlation in multi-container environments.
- **Central server**: [Seq](https://getseq.net/) at `http://localhost:5341` — provides searching, filtering, and dashboard capabilities over structured log data.
- See [ADR-0008](../ADRs/0008-seq-for-centralized-logging.md).

## 8.4 Health Monitoring

- API services and the WebApp expose a health-check endpoint at `/hc`.
- Implemented using `Microsoft.AspNetCore.HealthChecks`.
- Services with SQL Server dependencies also check database connectivity.
- Docker `HEALTHCHECK` statements call the `/hc` endpoint every 30 seconds:
  ```
  HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1
  ```

## 8.5 Resilience

- **Library**: [Polly](https://github.com/App-vNext/Polly) for retry and circuit-breaker patterns.
- **Where applied**:
  - Database connections (SQL Server startup can be slow).
  - Message broker connections (RabbitMQ might not be ready at startup).
  - HTTP calls from WebApp to backend APIs (with fallback to offline page after max retries).
- **Pattern**: Exponential backoff retry. Services log `Unable to connect to RabbitMQ/SQL Server, retrying in 5 sec.` during startup until dependencies are available.

## 8.6 Mapping

- **Library**: [AutoMapper](https://automapper.org/) — used where it adds value to map between POCOs (commands → events, events → models).
- Not used universally; only where mapping complexity warrants it.

## 8.7 API Documentation

- **Library**: [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) — auto-generates Swagger/OpenAPI documentation and interactive test UI for all ASP.NET Core Web APIs.

## 8.8 HTTP Client Abstraction

- **Library**: [Refit](https://github.com/paulcbetts/refit) — generates typed HTTP clients from interface definitions. Used by the WebApp to call backend APIs.

## 8.9 Security

This is a demo application and does **not** implement:
- Authentication or authorization
- HTTPS/TLS between services
- Secret management (connection strings are in configuration files)

In a production environment, these would be essential additions.
