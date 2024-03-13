# Bca.CodeChallange
As requested by the document, the system was made using the CQRS architecture approach, so there aren’t controllers, just commands and queries following the best practices of REST API. 
The choice to use CQRS is to have a decoupled system, small and meaningful commands, easier to maintain, to add new features, write unit and integration tests, and implement all SOLID principles. This approach allows, in the future, to implement or integrate with other services like message brokers, internal APIs, segregate CRUD operations in different database types, notification services like SMS, Push Notifications, etc., making it possible to have a distributed system.
Regarding tests, the pattern chosen was AAA (Arrange, Act, and Assert), using mock classes to build dependencies of each tested class, and for this first release, only the most important classes were tested, as well as the main commands.

## Assumptions that I thought of:
- Allowed to create a bid even if it’s smaller than the highest of an auction because this information could be used in the future by ETL and BI analysis.

- It’s not possible to create a vehicle without the manufacturer being registered in the system.

- It’s not possible to update or delete a vehicle in this first release.
