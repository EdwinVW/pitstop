# 2. Architecture Constraints

## 2.1 Technical Constraints

| Constraint | Description |
|------------|-------------|
| **.NET / C#** | All services are implemented in .NET 9 and C#. This keeps the solution accessible to the target audience (.NET developers) and enables shared libraries via NuGet. See [ADR-0002](../ADRs/0002-dotnet-as-implementation-platform.md). |
| **Docker** | Every service and all infrastructure components run as Linux Docker containers. Docker Compose is the primary local orchestration tool. |
| **SQL Server** | A single SQL Server instance is used as the database platform for all services. This is a deliberate simplification; production deployments would use separate instances. See [ADR-0007](../ADRs/0007-sql-server-as-single-database-platform.md). |
| **RabbitMQ** | RabbitMQ is the sole message broker for all asynchronous inter-service communication. See [ADR-0003](../ADRs/0003-rabbitmq-as-message-broker.md). |

## 2.2 Organisational Constraints

| Constraint | Description |
|------------|-------------|
| **Educational purpose** | The solution is a reference implementation for conference talks and workshops. Architectural decisions prioritise demonstrability and clarity over production readiness. |
| **Single developer** | The project is maintained by a single developer, which limits the breadth of technology choices and favours simplicity. |
| **Open source** | The code is publicly available on GitHub. No proprietary dependencies or licences are used (except Seq, which has a free tier for development). |

## 2.3 Conventions

| Convention | Description |
|------------|-------------|
| **Per-service database** | Each service owns its data and accesses it through its own database schema. No shared databases between services (logical separation on a single SQL Server instance). |
| **Asynchronous communication** | Services communicate exclusively through events via RabbitMQ. There are no synchronous service-to-service calls (only the WebApp calls APIs synchronously). |
| **Infrastructure.Messaging abstraction** | All message-broker interaction goes through the `IMessagePublisher` / `IMessageHandler` interfaces. No service directly depends on `RabbitMQ.Client`. See [ADR-0009](../ADRs/0009-infrastructure-messaging-abstraction.md). |
