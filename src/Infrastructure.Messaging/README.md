# Pitstop.Infrastructure.Messaging library
This library contains helper-classes for working with messaging within the Pitstop sample solution. It contains the following items:

- The base-classes for Commands and Events.
- Interfaces that abstract functionality to publish and consume messages using a message-broker. 
- Implementations for the interfaces that use RabbitMQ as message-broker.
- A helper class (_MessageSerializer_) for serializing and deserializing commands and events to and from JSON.

## Release notes
### Version 2.5.0
- Refactored RabbitMQMessagePublisher so it keeps the connection to RabbitMQ open (in stead of recreating it with every publish action).

### Version 2.4.0
- Upgraded all dependencies to the latest version.
 
### Version 2.3.0
- Added overload for connecting to RabbitMQ clusters.

### Version 2.2.0
- Upgraded project to target netstandard2.1.
- Upgraded all dependencies to the latest version.

### Version 2.1.0
- Upgraded all dependencies to the latest version.

### Version 2.0.0
- [**Breaking**] Removed the use of the _MessageTypes_ enum that contained all the available message-types. Throughout the solution, message-type is now a simple string.
- [**Breaking**] Removed the _MessageTypes_ enum from all the interfaces and implementations.
- [**Breaking**] Removed the _MessageTypes_ enum.
- [**Breaking**] Removed the _MessageType_ parameter from the Message base-class constructor. 
- Added some additional convenience-constructors to the _Message_, _Command_ and _Event_ classes that enable a developer to use them without explicitly specifying the _MessageId_ and _MessageType_ parameters. If omitted, _MessageId_ is automatically filled with a new Guid and _MessageType_ is automatically filled with name of the class.

### Version 1.1.0
- Upgrade to .NET Core 2.2.
- Upgraded all dependencies to the latest version.

### Version 1.0.0
Initial version. 