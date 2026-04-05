# ADR-0010: .NET / C# as the Implementation Platform

## Status
Accepted

## Date
2026-04-06

## Context
In a microservices architecture, each service is independently deployable and could in principle be implemented using any programming language or runtime — this polyglot capability is often cited as a benefit of the style. However, mixing languages and runtimes across services adds significant complexity in terms of tooling, build pipelines, developer onboarding, and shared library distribution.

This solution implements all services — REST APIs, background workers, and the web frontend — using .NET and C#. No service is implemented in a different language or runtime.

## Decision
**.NET / C#** is used exclusively as the implementation platform across all services.

## Rationale
The primary reason is the author's professional background in the Microsoft / .NET / C# ecosystem. Using a platform with deep familiarity allows the solution to demonstrate architectural patterns at a high level of quality and idiomatic correctness, rather than spreading effort across multiple technology stacks where depth would be shallower.

As a secondary benefit, a uniform technology stack keeps the solution accessible to its primary target audience: .NET developers learning microservices architecture. Developers can focus on the architectural patterns and design decisions without needing to context-switch between languages or runtimes.

## Consequences

### Positive
- All services are implemented with consistent quality and idiomatic .NET patterns.
- A single SDK, toolchain, and build system applies across the entire solution.
- Shared code (e.g., `Infrastructure.Messaging`, `TestUtils`) can be distributed as NuGet packages and consumed by all services without cross-language concerns.
- The solution is immediately accessible to .NET developers without requiring knowledge of additional runtimes.

### Negative
- Does not demonstrate polyglot service implementation, which is a genuine option in real-world microservices systems.
- May give the impression that a uniform technology stack is a requirement of microservices, rather than a pragmatic choice.

## Alternatives Considered
- **Polyglot implementation**: Implementing individual services in Node.js, Python, or Go to demonstrate technology diversity. Would showcase a broader range of runtime options but would dilute the depth of implementation quality and complicate the solution without contributing to its core architectural teaching goals. Rejected.
