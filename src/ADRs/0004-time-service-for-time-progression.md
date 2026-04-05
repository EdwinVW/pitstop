# ADR-0004: Dedicated Time Service for Time Progression

## Status
Accepted

## Date
2026-04-06

## Context
Several services in the solution need to perform time-based processing: the `NotificationService` sends daily maintenance reminders, and the `InvoiceService` generates invoices at the end of a working day. These behaviours must be triggered at a logical "day boundary".

The conventional approach would be to embed a scheduled job (cron-style background task) inside each service that reacts to the passage of real time. However, this ties business logic to the system clock, makes testing harder, and risks services reacting to different logical days if their internal schedules drift.

## Decision
A dedicated **TimeService** is introduced. It is the sole component responsible for publishing `DayHasPassed` events to the message bus. All services that need to react to the passage of time subscribe to this event rather than managing their own internal timers.

## Rationale
Three concerns motivated this decision:

1. **Deterministic testing**: By externalising time progression as an explicit event, tests can fast-forward time by publishing a `DayHasPassed` event manually, without waiting for real time to elapse or patching system clocks. This makes time-dependent behaviour fully controllable in tests.

2. **Single logical day for all consumers**: All subscribing services react to the same published event, guaranteeing they operate on the same logical day. There is no risk of clock skew or scheduling drift causing one service to process a different day than another.

3. **Externalising infrastructure concerns**: Time is an external dependency, not internal business logic. Treating it as an infrastructure concern — something that is injected into the system via an event — is consistent with the broader event-driven design and makes the boundary between domain logic and infrastructure explicit.

## Consequences

### Positive
- Time-dependent behaviour is fully testable without relying on real-time delays or clock manipulation.
- All consumers are guaranteed to process the same logical day simultaneously.
- Adding a new time-triggered service only requires subscribing to the existing `DayHasPassed` event — no new scheduling infrastructure needed.
- Demonstrates the pattern of treating infrastructure concerns (time, external triggers) as first-class domain events.

### Negative
- Introduces an additional service that must be running for time-dependent processing to occur; if the TimeService is down, no daily processing happens.
- The abstraction may be surprising to developers unfamiliar with the pattern — the connection between "sending invoices" and "a separate time service" is non-obvious.

## Alternatives Considered
- **Per-service internal schedulers**: Each service manages its own cron-style background task. Simpler to set up, but ties logic to the real system clock, complicates testing, and risks services processing different logical days if schedules drift. Rejected.
