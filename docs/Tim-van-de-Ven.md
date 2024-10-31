# Eigen bijdrage Tim van de Ven

```markdown
Je begin hier onder het hoofdkopje met een samenvatting van je bijdrage zoals je die hieronder uitwerkt. Best aan het einde schrijven. Zorg voor een soft landing van de beoordelaar, maar dat deze ook direct een beeld krijgt. Je hoeft geen heel verslag te schrijven. De kopjes kunnen dan wat korter met wat bullet lijst met links voor 2 tot 4 zaken en 1 of 2 inleidende zinnen erboven. Een iets uitgebreidere eind conclusie schrijf je onder het laatste kopje.
```

...

## 1. Code/platform bijdrage

In dit gedeelte bespreek ik mijn bijdragen aan de code en het platform. Mijn werk omvat het opzetten van een CI/CD pipeline, het integreren van monitoring tools in het Kubernetes platform, het ontwikkelen van een nieuwe MicroService en het toevoegen van een CustomerSupport pagina:

1. [CI/CD Pipeline](https://github.com/hanaim-devops/pitstop-team-luna/pull/31): Ik heb een nieuwe CI/CD pipeline opgezet. Deze volgt een standaard Build > Test > Deploy flow en is geïntegreerd in GitHub om de kwaliteit van de code te waarborgen. De Deploy stap wordt uitsluitend uitgevoerd op de Main branch.
2. [Prometheus AlertManager](https://github.com/hanaim-devops/pitstop-team-luna/pull/40): Ik heb Prometheus en Alertmanager toegevoegd aan het Kubernetes platform, waardoor monitoring en alertering mogelijk zijn.
3. [Nieuwe MicroService](https://github.com/hanaim-devops/pitstop-team-luna/pull/42): Voor mijn functionele taak was een nieuwe MicroService nodig. Om mijn branches kort te houden, heb ik eerst deze MicroService gecreëerd en geconfigureerd voor Docker en Kubernetes.
4. [CustomerSupport](https://github.com/hanaim-devops/pitstop-team-luna/pull/45): In de nieuwe MicroService heb ik een CustomerSupport pagina toegevoegd. Hier is een overzicht te zien van alle geweigerde reparaties. De data wordt ontvangen via een Event en vanwege het principe 'autonomy over authority' wordt de data door deze MicroService zelf beheerd.

## 2. Bijdrage app configuratie/containers/kubernetes

In dit gedeelte bespreek ik mijn bijdragen aan de configuratie van de applicatie, het gebruik van containers en de integratie met Kubernetes. Mijn werk omvat onder andere het opzetten van de configuratie voor monitoring tools, het schrijven van automatiseringscripts en het ontwikkelen van een nieuwe MicroService met correcte configuraties:

1. [Prometheus en Alertmanager configuratie](https://github.com/hanaim-devops/pitstop-team-luna/pull/40/commits/449e06ecd89f7480e7cce37d6a27a77c98098d82): Ik heb de configuratie voor Prometheus en Alertmanager opgezet en toegevoegd aan het Kubernetes platform.
2. [Kubernetes scripts](https://github.com/hanaim-devops/pitstop-team-luna/pull/40/commits/c508c51e517052262350056003d878daafb660f3): Ik heb scripts geschreven voor het automatisch deployen van Prometheus en Alertmanager naar het Kubernetes platform.
3. [Nieuwe MicroService](https://github.com/hanaim-devops/pitstop-team-luna/pull/42): Bij het ontwikkelen van mijn nieuwe MicroService heb ik direct gezorgd voor een correcte configuratie met zowel Docker als Kubernetes.

## 3. Bijdrage versiebeheer, CI/CD pipeline en/of monitoring

Ik heb een belangrijke rol gespeeld in het opzetten en automatiseren van de delivery pipeline, GitOps en monitoring. Mijn bijdragen omvatten de volgende aspecten:

1. [CI/CD Pipeline](https://github.com/hanaim-devops/pitstop-team-luna/pull/31): Ik heb een nieuwe CI/CD pipeline opgezet die een standaard Build > Test > Deploy flow volgt en geïntegreerd is in GitHub om de kwaliteit van de code te waarborgen. De Deploy stap wordt uitsluitend uitgevoerd op de Main branch. Ik heb ervoor gezorgd dat de pipeline naadloos aansluit op de bestaande infrastructuur en workflows.

2. GitOps: Ik heb bijgedragen aan het implementeren van GitOps-principes, zoals het gebruik van een pull model en het automatiseren van deployment processen. Dit heeft geleid tot een betere samenwerking binnen het team en meer controle over de gehele ontwikkelingscyclus.

3. [Prometheus en Alertmanager](https://github.com/hanaim-devops/pitstop-team-luna/pull/40): Ik heb monitoring en alertering geïmplementeerd met behulp van Prometheus en Alertmanager in het Kubernetes platform. Hierdoor is het mogelijk geworden om de prestaties van de applicatie te monitoren en tijdig te reageren op eventuele problemen. Ik heb ook gewerkt aan het toevoegen van zowel standaard als aangepaste metrics en het opzetten van rapportages om inzicht te krijgen in de prestaties van de applicatie en het systeem.

## 4. Onderzoek

In tegenstelling tot een vooraf uitgevoerd onderzoek, heb ik tijdens dit project het onderwerp van Prometheus en Alertmanager toegewezen gekregen en ben ik begonnen met het onderzoeken en implementeren van deze technologieën. Dit heeft geleid tot de succesvolle demo integratie van deze monitoring tools in ons project, zoals beschreven in mijn bijdragen aan de app configuratie en Kubernetes scripts.

Gedurende het project heb ik mezelf ondergedompeld in de documentatie, tutorials en best practices rondom Prometheus, Alertmanager, en hun integratie met Kubernetes. Dit heeft me in staat gesteld om een solide basis te leggen voor het monitoren en alerteren van onze applicatie. Op dit moment is er echter alleen een demo-casus geïntegreerd, voornamelijk vanwege tijdsgebrek. Desondanks dient deze demo als een eerste stap in het volledig benutten van deze technologieën binnen ons project.

## 5. Bijdrage code review/kwaliteit anderen en security

In dit gedeelte bespreek ik mijn bijdrage aan de code review, kwaliteitsverbetering en security van het werk van mijn collega's. Ik heb verschillende Pull Requests gereviewed en feedback gegeven om de algehele kwaliteit van het project te waarborgen en mogelijke beveiligingsproblemen te identificeren:

1. Ik heb de [PullRequest](https://github.com/hanaim-devops/pitstop-team-luna/pull/38) van Hugo gereviewed, waarin hij Slack Notifications heeft geïmplementeerd. Tijdens de review heb ik een kleine security issue opgemerkt.
2. Ik heb de [PullRequest](https://github.com/hanaim-devops/pitstop-team-luna/pull/46) van Harutjun gereviewed, waarin hij zijn functionele taak heeft geïmplementeerd. Tijdens de review zijn er redelijk wat Code Style issues aan het licht gekomen, en ik heb enkele vragen over de keuzes die hij heeft gemaakt.
3. Ik heb de [PullRequest](https://github.com/hanaim-devops/pitstop-team-luna/pull/44) van Osama gereviewed, waarin hij zijn onderzoeksproject van Keda samenvoegt. Tijdens de review heb ik opgemerkt dat hij vergeten was om de bestaande Kubernetes scripts in het project te updaten met zijn nieuwe Kubernetes bestanden.

## 6. Bijdrage documentatie

Competenties: *DevOps-6 Onderzoek*

```markdown
Zet hier een links naar en geef beschrijving van je C4 diagram of diagrammen, README of andere markdown bestanden, ADR's of andere documentatie. Bij andere markdown bestanden of doumentatie kun je denken aan eigen proces documentatie, zoals code standaarden, commit- of branchingconventies. Tot slot ook user stories en acceptatiecriteria (hopelijk verwerkt in gitlab issues en vertaalt naar `.feature` files) en evt. noemen en verwijzen naar handmatige test scripts/documenten.
```

TODO: C4 documentatie bijdragen

## 7. Bijdrage Agile werken, groepsproces, communicatie opdrachtgever en soft skills

Tijdens het project heb ik actief deelgenomen aan Scrum ceremonies en bijgedragen aan het groepsproces en de communicatie met de opdrachtgever. Enkele voorbeelden van mijn inbreng zijn:

1. Tijdens de sprint planning heb ik geholpen bij het inschatten van de benodigde tijd, prioritering van taken en inplannen van de sprintdoelen.
2. Tijdens de technische discussie met de opdrachtgever heb ik het proces [genotuleerd](https://github.com/hanaim-devops/pitstop-team-luna/commit/c9f2829c4debd22ce8c2707c887216650d54e0b8).
3. Om het groepsproces te verbeteren heb ik een Daily Standup meeting aangemaakt, dagelijks om 10.00.
4. Tijdens de Daily Standup meetings heb ik regelmatig de rol van gespreksleider op me genomen om een gestroomlijnde en productieve vergadering te waarborgen. Dit heeft bijgedragen aan een soepel verloop van het project en een effectieve communicatie binnen het team.
  
## 8. Leerervaringen

Gedurende dit project heb ik waardevolle leerervaringen opgedaan die me zullen helpen in mijn verdere loopbaan. Hieronder deel ik enkele tips en tops die ik uit deze ervaringen heb gehaald:

Tips:

1. Beter tijdsmanagement: Plan tijd voor zowel onderzoek als implementatie om ervoor te zorgen dat er voldoende tijd is om een onderwerp grondig te verkennen en te integreren in het project. Wees je ervan bewust dat je niet te lang vasthoudt aan problemen die mogelijk niet relevant zijn voor het project, zoals het probleem met Selenium drivers in de CI/CD pipeline waar te veel tijd aan is besteed.
2. Proactieve communicatie: Communiceer vroegtijdig met teamgenoten en de opdrachtgever over eventuele problemen of obstakels, zodat er samen naar oplossingen kan worden gezocht.

Tops:

1. Teamwork: Goede samenwerking en communicatie met teamgenoten, wat heeft geleid tot een betere kwaliteit van het eindproduct en een prettige werkomgeving.
2. Feedback: Het geven van constructieve feedback op het werk van mijn teamgenoten, met name op basis van mijn ervaring in C#, heeft bijgedragen aan de groei en ontwikkeling van zowel mijzelf als mijn teamgenoten. Door het delen van mijn kennis en inzichten, heb ik hen geholpen om hun vaardigheden verder te ontwikkelen en de kwaliteit van het project te verbeteren.

## 9. Conclusie & feedback

Competenties: *DevOps-7 - Attitude*

```markdown
Schrijf een conclusie van al bovenstaande punten. En beschrijf dan ook wat algemener hoe je terugkijkt op het project. Geef wat constructieve feedback, tips aan docenten/beoordelaars e.d. En beschrijf wat je aan devops kennis, vaardigheden of andere zaken meeneemt naar je afstudeeropdracht of verdere loopbaan.
```

...