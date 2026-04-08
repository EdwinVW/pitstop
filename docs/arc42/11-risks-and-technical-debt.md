# 11. Risks and Technical Debt

## 11.1 Risks

| ID | Risk | Probability | Impact | Mitigation |
|----|------|-------------|--------|------------|
| R1 | **Single SQL Server instance** — All services share one SQL Server container. A failure takes down all services simultaneously. | Medium (development) / High (if used as production blueprint) | High | Acceptable for development/demo. A production deployment would use separate SQL Server instances or a managed database service. See [ADR-0007](../ADRs/0007-sql-server-as-single-database-platform.md). |
| R2 | **No authentication or authorization** — The system has no security measures. APIs are open, and credentials are in configuration files. | N/A (by design) | High (if deployed publicly) | Intentional omission for a demo application. Must be addressed before any production use. |
| R3 | **TimeService as single point of failure** — If the TimeService is down, no `DayHasPassed` events are published, and daily processing (notifications, invoices) does not occur. | Low | Medium | The TimeService is a very simple service with few failure modes. In production, a persistent scheduler or managed cron service would be more appropriate. |
| R4 | **RabbitMQ as single point of failure** — A RabbitMQ outage blocks all asynchronous communication. | Medium | High | Acceptable for a demo. Production would use RabbitMQ clustering or a managed message service. |
| R5 | **Eventual consistency confusion** — Conference audience or developers unfamiliar with eventual consistency may see stale data and assume it is a bug. | Medium | Low | Covered by documentation and live explanation during talks. Seq logging helps trace event propagation. |

## 11.2 Technical Debt

| ID | Item | Description | Severity |
|----|------|-------------|----------|
| TD1 | **No update or delete operations** | Only CREATE and READ are implemented. This limits the usefulness as a reference for full CRUD patterns with event sourcing. | Low (intentional) |
| TD2 | **No API gateway** | Cross-cutting concerns like rate limiting, authentication, and SSL termination are not centralized. The WebApp holds direct service URLs. See [ADR-0006](../ADRs/0006-no-api-gateway.md). | Low (intentional) |
| TD3 | **Hardcoded configuration** | Connection strings and credentials are stored in `appsettings.json` files and `docker-compose.yml`. No secret management in place. | Medium |
| TD4 | **Limited test coverage** | Unit tests exist only for WorkshopManagementAPI (core domain). No integration tests for event propagation. UI tests exist but cover basic flows only. | Medium |
| TD5 | **Seq licensing** | Seq free tier is limited. Production use requires a commercial licence. The community might need an alternative (e.g., OpenTelemetry + Grafana). | Low |
| TD6 | **Dual service mesh maintenance** | Supporting both Istio and Linkerd configurations increases maintenance burden (34+ YAML files). See [ADR-0010](../ADRs/0010-kubernetes-with-service-mesh.md). | Low |

---
[← Back to arc42 index](arc42.md)
