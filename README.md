# Pitstop - Garage Management System
This repo contains a sample application based on a Garage Management System for PitStop - a fictitious garage. The application targets the employees of PitStop and supports their daily tasks. It should offer the following functionality:

- Vehicle management
- Customer management
- Workshop management
- Inventory management
- Sales
- Invoicing
- Notifications of customers 

>The primary goal of this sample is to demonstrate several Web-Scale Architecture concepts like: Microservices, CQRS, Event Sourcing, Domain Driven Design (DDD), NoSQL.

### Context Map
I've created a context map (DDD) that describes the bounded contexts (domains) within the system and the relationships between them:

![Context map](img/contextmap.png)

As you can see in the context map, Workshop Management is the primary domain. This is where PitStop Garage makes its money. That's why I chose to use a DDD approach (with Aggregates) and event-sourcing for this domain. Customer Management and Vehicle Management are supporting domains for which I've used a standard CRUD approach.

I've decided to leave support for Inventory Management (Products) and Sales out of the sample because this doesn't add enough value. The main goal of the sample is to demonstrate different architectural concepts and not be a full fledged application.

With every relationship I've specified which side of the relationship is Upstream (U) and which side is Downstream (D). Using these indications you can figure out which domain in the system is the system of record (source of truth) for a certain piece of information. We call that side the upstream side. This side dictates the schema of the data. The downstream side has to follow and use some approach to make sure it can use the data from the upstream system. 

So for instance: Vehicles are registered and maintained in the Vehicle Management domain. But within the Workshop Management domain, we need to have some vehicle information to be able to operate - even when the Vehicle service is offline (autonomy is very important in a Microservices architecture). So Vehicle information is cached within the Workshop Management domain. This cache is kept up-to-date by handling events that are being published by the Vehicle Management domain. 

#### Domain objects
The application uses the following domain-objects:

| Name             | Description                                                                                                   |
|------------------|---------------------------------------------------------------------------------------------------------------|
| Vehicle          | Represents a vehicle.                                                                                         |
| Customer         | Represents a customer that own one or more vehicles.                                                          |
| WorkshopPlanning | Represents the planning for the workshop during a single day. A planning contains 0 or more maintenance jobs. |
| MaintenanceJob   | Represents a job to executed for a single vehicle.                                                            |
| Product          | Represents a product (or part) that is used when executing a maintenance job or sold directly to a customer.  |
| Sale             | Represents a direct sale of a product to a customer (not related to a maintenance job.                        |
| Invoice          | Represents an invoice sent to a customer for 1 or more finished maintenance-jobs.                             |

### Solution Architecture
I've created a solution architecture diagram which shows all the moving parts in the application. You will probably recognize how the different bounded contexts in the Context Map are represented by services in this architecture.

![Solution Architecture](img/solution-architecture.png)

#### PitStop Web App
The web application is the front-end for the system. Users can manage customers, vehicles and the planning for the workshop from this front-end. The front-end will only communicate with the different APIs in the system and has no knowledge of the message-broker or any other services.

#### Customer Management Service
This service offers an API that is used to manage Customers in the system. For now, only CREATE and READ functionality (list and single by unique Id) is implemented. 

This service handles the following commands:

- RegisterCustomer

This service publishes the following events:

- CustomerRegistered

#### Vehicle Management Service
This service offers an API that is used to manage Vehicles in the system. For now, only CREATE and READ functionality (list and single by unique Id) is implemented. 

This service handles the following commands:

- RegisterVehicle

This service publishes the following events:

- VehicleRegistered

#### Workshop Management Service
This service contains 2 parts: an API for managing the workshop planning and an event-handler that handles events and builds a read-model that is used by the API. 

##### API
This is an API that is used to manage Maintenance Jobs in the system. For now, only CREATE, UPDATE and READ functionality is implemented. Because we want to be able to keep Workshop Management up and running even when other services are down, the API also offers functionality to retrieve vehicle and customer information from the read-model. This read-model is filled by the event-handler (described below).

This service handles the following commands:

- PlanMaintenanceJob
- FinishMaintenanceJob

This service publishes the following events:

- WorkshopPlanningCreated
- MaintenanceJobPlanned
- MaintenanceJobFinished

Within this domain I've used a DDD approach. The Workshop Planning aggregate handles all commands and yields events that will then be published using the message-broker.

Because this aggregate uses event-sourcing for persisting its state, every command that comes in is first transformed into an event that is handled by the aggregate. This will actually change the internal state of the aggregate. The state is persisted by storing the list of all events that occurred for 1 aggregate instance. When another command comes in for an aggregate instance (identified by its unique Id), all events are replayed and handled by the aggregate to return it to its former state. The aggregate offers a specific constructor that takes a list of events and replays them internally.

##### Event-handler
The event-handler ingests events containing information about Customers and Vehicles coming from the message-broker. It only handles events from the message-broker and offers no API. As stated above, it builds a read-model that is used by the front-end when scheduling maintenance jobs. This ensures that we can always schedule new maintenance jobs and manage existing jobs even though the Customer Service or Vehicle Service is offline.

This service handles the following events:

- CustomerRegistered
- VehicleRegistered
- MaintenanceJobPlanned
- MaintenanceJobFinished

#### Notification Service
The notification service sends a notification to every customer that has a maintenance job planned on the current day. It only handles events from the message-broker and offers no API. 

This service handles the following events:

- CustomerRegistered
- DayHasPassed
- MaintenanceJobPlanned
- MaintenanceJobFinished

#### Invoice Service
The invoice service creates an invoice for all maintenance jobs that have been finished (and are not yet invoiced). It only handles events from the message-broker and offers no API. The invoice is created as an HTML email message which is emailed to a printer. This company will print the invoice and send it to the customer by snail-mail (this last part is out of scope of course). 

This service handles the following events:

- CustomerRegistered
- DayHasPassed
- MaintenanceJobPlanned
- MaintenanceJobFinished

#### Time Service
The Time service is a service that informs other services when a certain time-period has passed. For now only the *DayHasPassed* event is supported.

I chose this approach to make testing of time-related functionality simple without the need for messing with the system-clock on test-machines. So instead of acting upon time-outs based on the system-time, services act upon events.

This service publishes the following events:

- DayHasPassed

#### Auditlog Service
The AuditLog service picks up all events from the message-broker and stores them for later reference. It only handles events from the message-broker and offers no API. 

## Technology
This chapter describes the technology and libraries used to build this application. I'm not going to describe in detail how the different components work. For that I refer you to the documentation of each component on the Internet.

**.NET Core & ASP.NET Core**
The application is build completely using .NET Core and ASP.NET Core. See [https://dot.net](https://dot.net ".NET web-site") for more info.

**Docker**
Every service within the system and all infrastructural components (database, message-broker, mailserver) are run in a Docker container. Docker Compose is used to compose the application and connect all the components. See [https://www.docker.com/](https://www.docker.com/ "Docker web-site") for more info.

**RabbitMQ**
RabbitMQ is used as message-broker. I use a default RabbitMQ Docker image (including management) from Docker Hub (`rabbitmq:3-management`). See [https://www.rabbitmq.com/](https://www.rabbitmq.com/ "RabbitMQ web-site") for more info.

**SQL Server on Linux**
The database server used to host all databases is MS SQL Server running on Linux. I use a default SQL Server for Linux Docker image from Docker Hub (`microsoft/mssql-server-linux`). In this sample application, I chose to use a single SQL Server instance for hosting all databases. In a production environment you could choose for a setup with multiple instances to enable fail-over scenarios in case of an emergency.

**MailDev**
To simulate sending emails, I use MailDev. This test-server acts as both an SMTP Server as a POP3 server and offers a website to see the mails that were sent. No emails are actually sent when using this test-server. I use the default MailDev Docker image from Docker Hub (`djfarrelly/maildev`). See [https://github.com/djfarrelly/MailDev](https://github.com/djfarrelly/MailDev "MailDev Github repo") for more info.

**AutoMapper**
AutoMapper is used (only where it adds value) to map certain POCOs to other POCOs. This is primarily handy when mapping commands to events, events to events or events to models. See [http://automapper.org/](http://automapper.org/ "Automapper web-site") for more info.

**Polly**
Polly is used to make sure the services are resilient to outages of other services. It offers automatic retry or circuit-breaker logic that is used at every interaction with resources that could be down (database, message-broker, other services). See [https://github.com/App-vNext/Polly](https://github.com/App-vNext/Polly "Polly Github repo") for more info.

**Refit**
Refit is used to simplify calling REST APis. See [https://github.com/paulcbetts/refit](https://github.com/paulcbetts/refit "Refit Github repo") for more info. 

**SwashBuckle**
Swashbuckle is used for auto-generating Swagger documentation and a test ui for the ASP.NET Web APIs. See [https://github.com/domaindrivendev/Swashbuckle](https://github.com/domaindrivendev/Swashbuckle "Swashbuckle Github repo") for more info.

**Dapper**
Dapper is used in several services as lightweight ORM layer. See [https://github.com/StackExchange/Dapper](https://github.com/StackExchange/Dapper "Dapper Github repo") for more info.

## Solution Folder structure
The Visual Studio solution contains several folders. Below you find a description of every folder. Most folders correspond to a component in the solution architecture. Look there for an in-depth description of the functionality of a component.

- **Solution items**
	- **ClearDatabases.sql** : a sql script to empty all the SQL databases of the solution.
	- **CopyNuGetFiles.ps1** : copy NuGet packages from the private NuGet folder.
	- **docker-compose.yml** : the docker-compose file for the application.
	- **RebuildAllDockerImages.ps1** : do a docker build of all the projects in the solution.
	- **RemoveUnusedImages.ps1** : removes "dangling" docker images (without a name). They are created during the docker build process and can be safely removed.
	- **StopAndRemoveAllContainers.ps1** : stops and removes all containers.
- **AuditlogService** : the AuditLog service.
- **CustomerManagementAPI** : the Web-API for managing customer data ("CRM").
- **Infrastructure** - an infrastructural component with reusable stuff. This is the only project that other projects have a project-reference to. In a real-world situation, you would build this project and publish it as a NuGet package to a NuGet feed and reference it from there!
- **InvoiceService** - the service that sends invoices for executed maintenance.
- **NotificationService** - the service that sends customers a notification when they have an appointment.
- **TimeService** - the service that lets "the world" know a certain period of time has passed. The current implementation only supports days. 
- **VehicleManagementAPI** - the Web-API for managing vehicle data.
- **WebApp** - the front-end web-application used by the end-users of the system (employees of PitStop garage).
- **WorkshopManagementAPI** - the Web-API for managing workshop data.
- **WorkshopManagementEventHandler** - the event-handler picking up events and creating the read-model for the WorkshopManagement domain.

## Getting started
In order to run the application you need to take several steps. This description assumes you're developing on a Windows machine using Visual Studio 2017 and already forked and pulled the latest version of the source-code from the repo.

> In the `docker-commpose.yml` file in the root of the solution folder there are some credentials specified for components that need them. These are also used by the different services that use these components (specified in config files):
> SQL Server login: sa / 8jkGh47hnDw89Haq8LN2
> Rabbit MQ login: rabbitmquser / DEBmbwkSrzy9D1T9cJfa

- Satisfy prerequisites
   Make sure you have Docker for Windows installed and running smoothly. Also make sure everything is configured correctly in order to pull Docker images from the public Docker hub.

- Create private NuGet source
   To prevent project-references between projects in the solution, I've used a folder on my local file-system as a private NuGet feed. 

   Create a folder somewhere on your local file-system to act as your private NuGet feed (my default is `d:\NuGet\PitStop`). Open Visual Studio and configure the NuGet sources and add the local feed using the folder you just created: 

   ![Add private NuGet feed](img/add-private-nuget-feed.png).

- Open the PitStop solution in Visual Studio.  

- Configure the private NuGet feed
   You only need to do this if you configured a different folder as private feed than `d:\NuGet\PitStop`. Open the Infrastructure project and edit the file *Infrastructure\Properties\PublishProfiles\Local NuGet Folder.pubxml*. Change the *PublishDir* setting to the folder you created. Open the file *CopyNuGetFiles.ps1* in the solution folder and change the folder in the last line of the script to the folder you created.

- Publish Infrastructure package
   In order to reference the Infrastructure package from other projects, we need to publish it. Right click on the Infrastructure project and select the option *Publish*. In the dialog that is shown, click the *Publish* button. Now a PitStop.Infrastructure NuGet package file should appear in your private NuGet feed folder.

- Rebuild solution
   To make sure everything is setup correctly, do a Rebuild All of the solution. This will also restore all the NuGet packages used throughout the solution. If no errors occur, you're good to go.

- Build docker images
   Open up a Powershell window and go to the `Pitstop/src` folder. Make sure you Docker for Windows is started. Then execute the `RebuildAllDockerImages.ps1` script. This will rebuild all the Docker images for all the projects. Watch the output for any errors. After the images are built, you could check whether they are all there using the `docker images` command. This should yield something like this: 

   ![](img/docker-images.png)

- Start the application
   This is it, you're now ready to spin up the system! Open up a Powershell window and go to the `Pitstop/src` folder. Then issue the following command: `docker-compose up`. 

   Because this will start everything in the foreground, you will see all the logging being emitted from the different components. You will probably see a couple of *Unable to connect to bla, retrying in 5 sec.* messages in there. This is expected and not a problem. This is Polly doing its work to make sure that failures that occur when calling a component that is still starting up are handled gracefully. 

   The first time the service are started, the necessary databases are automatically created by the different services. You could check this by connecting to the SQL Server using SSMS (server *localhost*) and looking at the different databases:

   ![](img/ssms-databases.png)

   Upon the registration of the first Maintenance Job, the event-store database *WorkshopManagementEventStore* will be created automatically.

## Testing the app
To test the application you need to open the following web-pages:

- The RabbitMQ management dashboard: [http://localhost:15672](http://localhost:15672). 
   Username: rabbitmquser
   Password: DEBmbwkSrzy9D1T9cJfa

- The MailDev inbox: [http://localhost:4000](http://localhost:4000).

- The PitStop web-application: [http://localhost:6000](http://localhost:6000).

Now you can follow the following scenario (make sure you fill all the fields in the entry-forms):

- Register a new customer on the *Customer Management* screen.
- Register a new Vehicle for this customer on the *Vehicle Management* screen.
- Register a couple of Maintenance Jobs for the vehicle on the *Workshop Management* screen.

Now you've used the basic functionality of the application. We'll test the functionality of the *Notification service* next. 

### Testing notifications
To test the *Notification Service*, make sure you have scheduled a Maintenance Job for today. The *Notification service* reacts to the *DayHasPassed* event that is normally published by the *Time service*. But in order to speed this up, we'll publish such an event using the RabbitMQ Management Dashboard. Open the browser window (or tab) that shows the RabbitMQ Management Dashboard. Go to the *Exchanges* tab and click the *PitStop* exchange. Now expand the *Publish message* part of the screen. To send the correct event, we need to add a header to the message. You do this by adding a header named *MessageType* and value *DayHasPassed*. The body (payload) of the message should contain an empty object (defined in JSON):

![](img/day-has-passed-event.png)

Now you can push the *Publish message* button to send the message. If all goes according to plan, the *Notification Service* will pick up this event, check whether there is a maintenance job scheduled for today (for which no notification has been sent yet) and send a notification email to the customer. This email should be visible in the MailDev in-box:

![](img/notification-email.png)

### Testing invoicing
To test the *Notification Service*, make sure you have scheduled a Maintenance Job for today. Now first complete this Maintenance Job on the *Workshop Management* screen.

The *Invoice service* reacts to the *DayHasPassed* event that is normally published by the *Time service*. But in order to speed this up, we'll publish such an event using the RabbitMQ Management Dashboard. See the description of how to do this under *Testing Notifications* above.

If all goes according to plan, the *Invoice Service* will pick up this event, check whether there are any maintenance jobs completed today (for which no invoice has been sent yet) and send an invoice HTML email to *Presto Print* (a fictitious printing company that will print and snail-mail the invoice to the customer). This email should be visible in the MailDev in-box:

![](img/invoice-email.png)

### Testing APIs
If you want to test the individual APIs in the system, you can use the test UIs that are auto-generated by Swashbuckle. The following URLs can be used:

| API                | URL                                                            |
|--------------------|----------------------------------------------------------------|
| CustomerManagement | [http://localhost:5100/swagger](http://localhost:5100/swagger) |
| VehicleManagement  | [http://localhost:5000/swagger](http://localhost:5000/swagger) |
| WorkshopManagement | [http://localhost:5200/swagger](http://localhost:5200/swagger) |

## Contributing
This sample is a personal R&D project for me to learn. I've tried to document it as thoroughly as possible for people wanting to learn from it. If you have any improvements you want to contribute (to the code or the documentation) or find any bugs that need solving, just create a pull-request!

## Disclaimer
The code in this repo is NOT production grade and lacks any automated testing. It is intentionally kept as simple as possible (KISS). Its primary purpose is demonstrating several architectural concepts and not being a full fledged application that can be put into production as is. 

The author can in no way be held liable for damage caused directly or indirectly by using this code.
