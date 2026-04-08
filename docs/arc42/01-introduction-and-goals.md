# 1. Introduction and Goals

## 1.1 Requirements Overview

Pitstop is a sample application based on a **Garage Management System** for a fictitious garage / car repair shop. It targets the garage employees and supports their daily tasks:

- **Manage customers** — Register and look up customers.
- **Manage vehicles** — Register vehicles and associate them with customers.
- **Manage workshop planning** — Plan and track maintenance jobs for vehicles on specific days.
- **Send notifications** — Automatically notify customers when their vehicle has a maintenance job scheduled for the current day.
- **Send invoices** — Generate and email invoices for finished maintenance jobs.
- **Audit logging** — Record all domain events for later reference.

> The functional scope is intentionally limited (only create and read — no update or delete). The primary goal is to demonstrate architectural concepts, not to build a full-fledged business application.

## 1.2 Quality Goals

| Priority | Quality Goal        | Description |
|----------|---------------------|-------------|
| 1        | **Learnability**    | The solution must be easy to understand for .NET developers who want to learn about microservices architecture, event-driven systems, DDD, CQRS, and event sourcing. |
| 2        | **Demonstrability** | The system must be easy to run locally (Docker Compose) and on Kubernetes, so it can be used in conference talks and workshops. |
| 3        | **Autonomy**        | Each service must be independently deployable and able to continue operating even when other services are temporarily unavailable. |
| 4        | **Resilience**      | The system must handle transient failures gracefully using retry and circuit-breaker patterns. |

## 1.3 Stakeholders

| Role/Name             | Expectations |
|-----------------------|--------------|
| **Conference audience**      | Understand microservices, DDD, CQRS, event sourcing, and container technologies from a working reference implementation. |
| **Workshop participants**    | Be able to run the system locally, walk through the code, and experiment with changes. |
| **.NET developers**          | Learn best-practice patterns for building event-driven microservice systems with .NET and ASP.NET Core. |
| **DevOps engineers**         | Understand containerisation with Docker, orchestration with Kubernetes, and service-mesh capabilities (Istio, Linkerd). |

---
[← Back to arc42 index](arc42.md)
