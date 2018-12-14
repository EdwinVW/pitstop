# Pitstop.Infrastructure.Messaging library
This library contains helper-classes for working with messaging within the Pitstop sample solution. It contains the following items:

- The base-classes for Commands and Events.
- Interfaces that abstract functionality to publish and consume messages using a message-broker. 
- Implementations for the interfaces that use RabbitMQ as message-broker.
- A helper class (_MessageSerializer_) for serializing and deserializing commands and events to and from JSON.

## Release notes
### Version 2.0.0
- [**Breaking**] Removed the use of the _MessageTypes_ enum that contained all the available message-types. Throughout the solution, message-type is now a simple string.
- [**Breaking**] Removed the _MessageTypes_ enum from all the interfaces and implementations.
- [**Breaking**] Removed the _MessageTypes_ enum.
- [**Breaking**] Removed the _MessageType_ parameter from the Message base-class constructor. MessageType is now automatically filled with the name of the class.
- Added parameterless constructor to the _Command_ and _Event_ classes that initialize the _MessageId_ parameter of the base-class constructor with a new Guid.

### Version 1.1.0
- Upgrade to .NET Core 2.2.
- Upgraded all dependencies to the latest version.

### Version 1.0.0
Initial version. 