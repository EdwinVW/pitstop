# Pitstop.Infrastructure.Messaging library
This library contains helper-classes for working with messaging within the Pitstop sample solution. It contains the following items:

- The base-classes for Commands and Events.
- Interfaces that abstract functionality to publish and consume messages using a message-broker. 
- Implementations for the interfaces that use RabbitMQ as message-broker.
- A helper class (_MessageSerializer_) for serializing and deserializing commands and events to and from JSON.

## Release notes
### Version 2.0.0
- [**Breaking**] Removed the use of the MessageTypes enum that contained all the available message-types. Throughout the solution, message-type is now a simple string.
- [**Breaking**] Removed the MessageType enum from all the interfaces and implementations.
- [**Breaking**] Removed the MessageTypes enum.

### Version 1.1.0
- Upgrade to .NET Core 2.2.
- Upgraded all dependencies to the latest version.

### Version 1.0.0
Initial version. 