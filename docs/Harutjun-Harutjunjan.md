# Eigen bijdrage Harutjun Harutjunjan

Voor het project heb ik het meeste gewerkt aan het implementeren van mijn user story, jaeger en het fouten oplossen van docker. Helaas heb ik veel hardware problemen meegemaakt met docker waardoor ik veel moeite ben opgelopen. Ik heb mijn user story gemaakt met code en al. Verder heb ik voor de pipeline een tag toegevoegd voor vewrsiebeheer met semver. Zelf heb ik 2 pull requests gereviewed om zo de code kwaliteit te bewaren. Ik heb alle branches gemaakt uit de taken zelf in github en heb ik ook alle commits duidelijk proberen te houden. Verder heb ik zelf veel taken gemaakt in github en daar ook de acceptatiecriteria van. Ook heb ik in de readme een beschrijving van de casus en wat daarbij hoort. Voor scrum heb ik 1 keer gehandeld als een voorzitter van het gesprek en de andere keer als notulist. Vervolgens ben ik altijd goed aanwezig geweest bij de scrum ceremonies en vooral ook met de DSU(Daily Standup). Helaas heb ik veel moeite gehad met het implementeren van mijn technologie en is het mij niet gelukt, bekijk hiervoor hoofdtuk 4.

## 1. Code/platform bijdrage

Competenties: *DevOps-1 Continuous Delivery*

Ik heb aan 2 microservice gewerkt, namelijk RepairManagementAPI en NotificationService. Ik heb de voor mij belangrijkste stukken in de 2 bovenste links gezet per microservice. Daaronder heb ik de link naar mijn pull request waar alles bekeken kan worden.

- [RepairManagementAPI, zie regel 205-245](https://github.com/hanaim-devops/pitstop-team-luna/blob/3-als-monteur-wil-ik-automatisch-een-notificatie-krijgen-zodra-de-klant-goedkeuring-geeft-zodat-ik-de-reparatie-efficint-in-kan-plannen/src/RepairManagementAPI/Controllers/RepairManagementController.cs)
- [NotificationService, zie regel 113-141](https://github.com/hanaim-devops/pitstop-team-luna/blob/3-als-monteur-wil-ik-automatisch-een-notificatie-krijgen-zodra-de-klant-goedkeuring-geeft-zodat-ik-de-reparatie-efficint-in-kan-plannen/src/NotificationService/NotificationWorker.cs)
- [Hele pull request van mijn bijdrage](https://github.com/hanaim-devops/pitstop-team-luna/pull/46)

## 2. Bijdrage app configuratie/containers/kubernetes

Competenties: *DevOps-2 Orchestration, Containerization*

Hoewel ik niet direct verantwoordelijk was voor het opzetten van de Docker- en Kubernetes-configuraties, heb ik wel gebruik gemaakt van de bestaande Dockerfiles en configuraties om de applicatie draaiende te krijgen. De applicatie was al geconfigureerd met Docker, wat het proces vereenvoudigde, en ik kon de containers snel opstarten met docker compose up.

- Gebruik van bestaande Dockerfiles: Ik heb de Dockerfiles bestudeerd en begrijp nu hoe ze zijn opgebouwd, wat me heeft geholpen bij het begrijpen van de containerisatie van de applicatie.
- Kubernetes configuratie: Tijdens het project heb ik documentatie over de Kubernetes-configuraties bestudeerd, wat mijn begrip van container orchestratie wel heeft vergroot.
- Samenwerking met team: Ik heb nauw samengewerkt met teamleden die verantwoordelijk waren voor de Kubernetes-configuratie en heb hun inzichten en feedback geïntegreerd in mijn eigen werkprocessen.

Hoewel mijn bijdrage niet direct zichtbaar is, heeft de opgedane kennis een blijvende impact op mijn ontwikkeling.

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Competenties: *DevOps-1 - Continuous Delivery*, *DevOps-3 GitOps*, *DevOps-5 - SlackOps*

Bij de pipeline heb ik zelf voor versiebeheer een stuk toegevoegd waar steeds een nieuwe tag wordt gecreate. 

- [Pipeline changes](https://github.com/hanaim-devops/pitstop-team-luna/commit/05f430b5cb7e30fd2950e00218df9c22e8117988)

Daarnaast heb ik geprobeerd om Jaeger toe te passen voor monitoring, waarbij ik verschillende bronnen heb bestudeerd om de mogelijkheden te begrijpen. Dit is mij echter helaas niet gelukt hierover spreek ik meer in hoofdstuk 4.

## 4. Onderzoek

IK GA HIER BESCHRIJVEN WAAROM HET NIET IS GELUKT

Competenties: *Nieuwsgierige houding*

Beschrijf hier voor het Course BP kort je onderzochte technologie met een link naar je blog post, of het toepassen ervan gelukt is en hoe, of waarom niet. Beschrijf evt. kort extra leerervaringen met andere technologieen of verdieping sinds het blog.

Tijdens het grote project beschrijf je hier onderzoek naar het domein en nieuwe onderzochte/gebruikte DevOps technologieën. Wellicht heb je nogmaals de voor blog onderzochte technologie kunnen toepassen in een andere context. Verder heb je nu een complex domein waar je in moet verdiepen en uitvragen bij de opdrachtgever. Link bijvoorbeeld naar repo's met POC's of, domein modellen of beschrijf andere onderwerpen en link naar gebruikte bronnen.

Als de tijdens course onderzochte technologie wel toepasbaar is kun je dit uiteraard onder dit punt noemen. Of wellicht was door een teamgenoot onderzochte technologie relevant, waar jij je nu verder in verdiept hebt en mee gewerkt hebt, dus hier kunt beschrijven. Tot slot kun je hier ook juist een korte uitleg geef over WAAROM  jouw eerder onderzochte technologie dan precies niet relevant of inpasbaar was. Dit is voor een naieve buitenstaander niet altijd meteen duidelijk, maar kan ook heel interessant zijn. Bijvoorbeeld dat [gebruik van Ansible in combi met Kubernetes](https://www.ansible.com/blog/how-useful-is-ansible-in-a-cloud-native-kubernetes-environment) niet handig blijkt. Ook als je geen uitgebreid onderzoek hebt gedaan of ADR hebt waar je naar kunt linken, dan kun je onder dit kopje wel alsnog kort conceptuele kennis duidelijk maken.

## 5. Bijdrage code review/kwaliteit anderen en security

Competenties: *DevOps-7 - Attitude*, *DevOps-4 DevSecOps*

Ik heb wat code gereviewed van groepsgenoten en zeker ook comments van mijn groepsgenoten op mijn pr doorgevoerd.

- [Code review pr 1](https://github.com/hanaim-devops/pitstop-team-luna/pull/45)
- [Code review pr 2 (klap bijvoorbeeld de eerste comment open)](https://github.com/hanaim-devops/pitstop-team-luna/pull/38/files)
- [Referentie nogmaals naar mijn pr waar ik alle comments fix](https://github.com/hanaim-devops/pitstop-team-luna/pull/46/files)

## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*

Voor dit project heb ik ons casus voor verduidelijking in de README geschreven met ook de uitbreidingen die wij zelf hadden besloten. Ik heb deze helaas wel laat gepushed maar al welk vroeg gemaakt zie ik nu. Bij de tasks link die ik heb toegevoegd kan je een snelle ctrl + f doen met mijn github naam (HHarutjun) en dan kan je alle taken zien die ik heb gemaakt met de daarbij behorende acceptatiecriteria. Hoewel ik zeker ben van het fijt dat ik ook een paar van de top niveau user stories taken heb aangemaakt is dat helaas niet terug te vinden op github. De subtaken echter wel dus ik hoop dat dat genoeg is. Ook heb ik mijn branch gemaakt op basis van de gegeven taak en zijn de commits ook gedaan op de taak zelf.

- [Bijdrage in README.md](https://github.com/hanaim-devops/pitstop-team-luna/commit/7fe275e5d1450b48c63a224464508df5be499e63)
- [Bijdrage aanmaken tasks en de daarbij behorende acceptatiecriteria](https://github.com/hanaim-devops/pitstop-team-luna/issues?q=is%3Aissue&page=1)
- [Bijdrage branch en commit conventies](https://github.com/hanaim-devops/pitstop-team-luna/commits/3-als-monteur-wil-ik-automatisch-een-notificatie-krijgen-zodra-de-klant-goedkeuring-geeft-zodat-ik-de-reparatie-efficint-in-kan-plannen/)

## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Competenties: *DevOps-1 - Continuous Delivery*, *Agile*

Bij de sprint retrospective van sprint 1 heb ik gedient als scrum master en op 28-10-2024 bij de sprint 2 planning heb ik gedient als notulist. Hieronder een link naar mijn notulen. Ook was ik bij elke DSU aanwezig. Daarnaast heb ik ook gewoon heel goed bijgedragen bij de communicatie door altijd bereikbaar te zijn. Dit was ook een "Continue doing" punt bij ons bij de retrospective.

- [Geschreven notulen](../Notities/28-10-2024.md)
- Scrum master in retrospective
- Elke DSU aanwezig
- [Miro bord retrospective](https://miro.com/app/board/uXjVLMAHh4U=/)
  
## 8. Leerervaringen

Competenties: *DevOps-7 - Attitude*

Hier zijn 3 tops en 4 tips die ik voor mijzelf heb opgesteld die ik kan meenemen naar mijn verdere loopbaan. Hier valt de feedback van mijzelf en teamgenoten ook onder.

Tops:

- Ik vind wel mooi dat ik zo heb gewerkt in een teamverband, ik hebben nog nooit een beroepsproduct van 2 weken gemaakt met zoveel man. Dus ik vond wel goed hoe ik me in de hele omgeving kon integreren.
- Ik vind ook van mezelf dat ik heel hard heb zitten werken en onderzoeken op alle errors die ik kreeg om geen obstakel te zijn voor mijn team.
- Ik vind best goed dat ik heb kunnen in een onbekend project met een taal waar ik ook niet veel ervaring mee heb (c#)

Tips:

- Ik vind dat ik moet proberen om wat verder te kijken dan mijn neus lang is. Ik had helaas niet zulke grote obstakels verwacht bij het implementeren van mijn taken (tech en userstory)
- Beter vooruitzicht houden waarop mijn taken dependent zijn als ik ze aanneem of maak. Ik begon namelijk pas na te denken waar mijn taak op afhankelijk was nadat ik was begonnen. Hierdoor kon ik niet heel goed doorwerken aan mijn taak in de vakantie.
- Beter onderzoek doen naar het hele structuur en hoe alles samenhangt. Ik heb het gevoel dat ik meer onderzoek had gedaan in eerste instantie hoe elke microservice (vooral die ik nodig heb) intern werken, maar ik heb niet genoeg onderzoek gedaan hoe ze samen werken.
- Neem alles stap voor stap door is iets wat ik ook tegen mezelf zou zeggen. Soms kijk ik naar het hele plaatje en dan lijkt het allemaal zo groot en moeilijk voor me, maar als ik een voor een rustig iets doorlees dan wordt het langzamerhand duidelijker. Ik neem dit mee voor de volgende keer.

## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

Tijdens dit project heb ik waardevolle ervaring opgedaan, ondanks de uitdagingen die we tegenkwamen. Zo had ik mijn eerste ervaring in het werken met kubernetes en microservice architectuur in een best aanzienlijk project. Het was wel vooral op het begin een wat onoverzichtelijkere periode waarin de beperkte tijd van twee weken een struikelblok vormde om alles zo perfect mogelijk af te ronden. Desondanks ben ik wel tevreden met wat we als team hebben bereikt.

In mijn bijdrage heb ik me gefocust op het implementeren van mijn user story, het werken met Jaeger voor tracing, en het oplossen van problemen met Docker. De hardwareproblemen die ik ondervond, hebben een aanzienlijke impact gehad op mijn voortgang. Dit heeft me echter ook geleerd hoe belangrijk het is om goed voorbereid te zijn en problemen vroegtijdig te signaleren.

De ervaring in het team heeft me doen inzien dat effectieve communicatie en samenwerking cruciaal zijn, vooral in een Agile omgeving. Mijn rol als scrum master en notulist heeft me geholpen om beter inzicht te krijgen in groepsdynamiek en om het proces van Agile werken te begrijpen.

Constructieve feedback voor de organisatie zou zijn om de informatie te verbeteren en mogelijk een meer gestructureerde aanpak te hanteren voor toekomstige projecten. Dit zou kunnen helpen om de chaos te verminderen en de efficiëntie te verhogen. Wat ik bijvoorbeeld wat moeite mee had is dat we zoveel "sources of truths" hadden op bijvoorbeeld slack, de lessen etc. Dus je moest op heel veel verschillende plekken kijken als het ware om te kijken wat er nou precies wordt verwacht.  

Ik neem verschillende vaardigheden mee naar mijn afstudeeropdracht en verdere loopbaan, zoals de ontwikkeling van CI/CD-pipelines, ervaring met Kubernetes en Docker, en het vermogen om in teamverband te werken onder druk. Deze ervaringen hebben mijn nieuwsgierigheid naar DevOps-technologieën verder bevorderd en ik kijk ernaar uit om deze kennis in de toekomst verder uit te breiden.
