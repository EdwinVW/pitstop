# ADR-0009: Kubernetes Deployment with Istio and Linkerd Service Mesh Support

## Status
Accepted

## Date
2026-04-06

## Context
The solution includes Kubernetes manifests for deploying all services to a cluster. Beyond basic Kubernetes resources (Deployments, Services, ConfigMaps), the repository also contains Istio and Linkerd service mesh configurations, including:

- Canary deployments (v1/v2 variants for CustomerManagementAPI and VehicleManagementAPI)
- Traffic mirroring
- Fault injection
- AuthorizationPolicies and DestinationRules (Istio)
- Linkerd manifests as an alternative to Istio

A plain Kubernetes deployment without a service mesh would already demonstrate container orchestration for a microservices system. The service mesh layer adds significant additional concepts: sidecar proxy injection, mTLS between services, traffic shaping, and mesh-level observability.

## Decision
Kubernetes manifests are provided for both **plain deployment** and **service mesh deployment** using Istio (primary) and Linkerd (alternative). The service mesh configuration is optional and additive — the solution runs without it.

## Rationale
The service mesh support was added primarily to support **conference talks about service-meshes**. Pitstop serves as a realistic, working microservices application that can be used as a live demo substrate for demonstrating service mesh features in practice — features that are difficult to illustrate without a multi-service application already in place.

The two service mesh options (Istio and Linkerd) allow talks to compare the approaches: Istio as the feature-rich, widely adopted option, and Linkerd as the lightweight, simpler alternative.

## Consequences

### Positive
- Provides a ready-made demo environment for conference talks on service meshes, avoiding the need to build a separate demo application.
- Demonstrates advanced traffic management scenarios (canary releases, fault injection, traffic mirroring) that are relevant to production microservices operations.
- Having both Istio and Linkerd configurations allows direct comparison between the two leading service mesh implementations.
- The service mesh configuration is optional — developers learning the core microservices patterns are not required to engage with it.

### Negative
- Significantly increases the Kubernetes configuration surface area (34 YAML files), which can be overwhelming for developers whose primary interest is in the application-level architecture.
- Service mesh setup requires additional tooling (Istio/Linkerd CLI, cluster-level installation) that goes beyond standard Kubernetes knowledge.
- The Istio and Linkerd manifests require maintenance as both projects evolve their APIs and configuration formats.

## Alternatives Considered
- **Plain Kubernetes only**: Would be sufficient to demonstrate container orchestration and is simpler to understand and maintain. Rejected because it would not support the service mesh demonstration use case.
- **Single service mesh (Istio only)**: Would reduce maintenance burden. Linkerd was retained to enable direct comparison between the two approaches in talks.
