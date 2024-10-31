# Pitstop - Garage Management System
This repo contains a sample application based on a Garage Management System for Pitstop - a fictitious garage / car repair shop. The primary goal of this sample is to demonstrate several software-architecture concepts like:  
* Microservices  
* CQRS  
* Event driven Architecture  
* Event sourcing  
* Domain Driven Design (DDD)  
* Eventual Consistency  

and how to use container-technologies like:

* Docker
* Kubernetes
* Istio (service-mesh)
* Linkerd (service-mesh)

See [the Wiki for this repository](https://github.com/EdwinVW/pitstop/wiki "Pitstop Wiki") for more information about the solution and instructions on how to build, run and test the application using Docker-compose and Kubernetes.

![](pitstop-garage.png)

> This is an actual garage somewhere in Dresden Germany. Thanks to Thomas Moerkerken for the picture!

slack invite: https://join.slack.com/t/pitstop-luna/shared_invite/zt-2spmmbuc5-0NxJT2TPwBu3YBC9zX6BDQ

## Casus: Notificatiesysteem voor Snellere Reactie en Efficiënter Gebruik van Werkplaatsen

Het doel van dit project is om het bestaande notificatiesysteem van Pitstop uit te breiden met real-time notificaties voor klanten. Deze notificaties zorgen ervoor dat klanten direct op de hoogte worden gebracht van de status van hun reparatie en dat monteurs efficiënter kunnen werken. Dit is een verbetering ten opzichte van het huidige systeem, dat enkel notificaties verstuurt over aankomende onderhoudsbeurten.

### Overzicht van het Nieuwe Notificatiesysteem

Het nieuwe systeem stuurt klanten notificaties bij de volgende gebeurtenissen:

1. **Reparatie Gestart** – De klant ontvangt een melding wanneer de monteur de reparatie heeft aangemeld en de klant informeert over de status.
2. **Klant Goedkeuring Vereist** – Indien extra kosten of werkzaamheden nodig zijn, ontvangt de klant een notificatie met een overzicht van de prijs en werkzaamheden. De klant kan de reparatie hier goedkeuren of afwijzen.
3. **Reparatie Voltooid** – Zodra de reparatie klaar is, ontvangt de klant een bevestiging dat het voertuig kan worden opgehaald.

### Proces en Rollen

Het systeem introduceert drie gebruikersrollen om de communicatie en het werkproces te optimaliseren:

**Monteur**: kan voertuigen aanmelden voor reparatie en specificeren wat er gerepareerd moet worden. De monteur ontvangt een bevestiging als de klant akkoord gaat of ziet een afwijzing als de klant niet akkoord gaat met de prijs.
**Klantenservice**: heeft een overzicht van alle berichtenuitwisselingen tussen monteurs en klanten en kan helpen bij vragen of problemen tijdens de communicatie.
**Werkplaatsmanager**: heeft toegang tot statistieken over reactietijden en goedkeuringstijden, wat helpt bij het verbeteren van de doorlooptijd en het efficiënter inzetten van werkplaatsen.

### Doel

Dit uitgebreide notificatiesysteem vermindert de wachttijden doordat klanten sneller reageren op statusupdates. Door de tijdige goedkeuring en afwijzing van reparaties kunnen werkplaatsen optimaal worden benut, wat uiteindelijk zorgt voor een betere klanttevredenheid en een efficiëntere workflow in de garage.
