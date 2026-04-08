# ADR-0008: Seq for Centralized Logging

## Status
Accepted

## Date
2026-04-06

## Context
The solution runs as multiple independent services across several containers. Diagnosing issues and observing system behaviour requires log output from all services to be collectable, searchable, and viewable in one place. Each service uses Serilog for structured logging, which produces rich, queryable log events rather than plain text lines.

Several centralized logging solutions are available:
- **Seq** — a structured log server with a web UI, designed for .NET/Serilog ecosystems
- **ELK stack** (Elasticsearch + Logstash + Kibana) — widely used, powerful, but operationally heavy
- **Grafana + Loki** — lightweight log aggregation with Grafana dashboards
- **Cloud-native solutions** (Azure Monitor, Application Insights, AWS CloudWatch) — managed, but introduce cloud-provider dependency

## Decision
**Seq** is used as the centralized log aggregation server, configured as a Serilog sink in all services.

## Rationale
Three factors drove this choice:

1. **Simplicity**: Seq runs as a single Docker container with no additional components. The ELK stack requires at minimum three containers (Elasticsearch, Logstash, Kibana) with significant configuration overhead. Seq requires none of that.

2. **First-class .NET and Serilog support**: Seq is purpose-built for structured .NET logging. The `Serilog.Sinks.Seq` package integrates seamlessly — structured log properties (service name, machine name, log level) are captured and queryable without any transformation pipeline.

3. **Demo-friendliness**: The solution is used in conference talks and presentations. Seq's clean, intuitive web UI at `http://localhost:5341` allows live log streaming and filtering to be demonstrated clearly to an audience without requiring explanation of complex tooling.

## Consequences

### Positive
- Single container to add to Docker Compose — minimal operational overhead.
- Structured log properties from all services are immediately searchable via Seq's query language.
- The web UI is accessible and visually clear, making it effective for both development debugging and live demonstrations.
- No cloud-provider dependency; fully self-contained in the local and Kubernetes environments.

### Negative
- Seq is not as widely adopted in production environments as the ELK stack or cloud-native solutions, so the choice may not directly transfer to what developers encounter in their workplace.
- Seq's free tier has limitations (single user); production use of Seq requires a commercial licence.

## Alternatives Considered
- **ELK Stack (Elasticsearch + Logstash + Kibana)**: Industry-standard but requires multiple containers, significant configuration, and is operationally heavy relative to the needs of this solution. Rejected in favour of simplicity.
- **Grafana + Loki**: Lightweight alternative but less optimised for structured .NET log events and requires more setup than Seq. Rejected.
- **Cloud-native logging (Azure Monitor, Application Insights)**: Would introduce a cloud-provider dependency and require external accounts, making local development and offline demos impractical. Rejected.
