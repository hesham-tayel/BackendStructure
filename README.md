1. Purpose & Responsibilities
This backend service is responsible for:

Exposing HTTP endpoints for all domain operations (CRUD and beyond).

Implementing business logic and validation rules in a centralized, testable layer.

Persisting and retrieving data from MongoDB.

Authenticating and authorizing each request to enforce security policies.

2. Architectural Patterns
Command-Query Responsibility Segregation (CQRS):
We separate writes (commands) from reads (queries) to keep each side focused on its job. Learn more: [CQRS pattern overview.
](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)




MediatR for in-process messaging:
Controllers send commands/queries to MediatR instead of calling services directly. MediatR then invokes the matching handler. Details:[ MediatR Pattern.](https://medium.com/@dnzcnyksl/mediator-pattern-in-c-109b691ff45b)
.


How the Repository Layer is Structured
IAtlasMongoRepository (Generic)
This interface has only one job:
→ Provide a method to get the MongoDB collection for a given model (like Order, User, etc.).

It does not implement operations like Insert, Update, Delete.

It keeps the MongoDB collection handling centralized and reusable.

Feature-Specific Repositories (like IOrderMongoRepository)
Each model (like Order) has its own repository and interface.

Example:

IOrderMongoRepository defines methods like InsertAsync, FindByIdAsync, etc. — but only for orders.

OrderMongoRepository implements these methods.

Inside OrderMongoRepository, when we need to talk to MongoDB:

It first gets the right collection using IAtlasMongoRepository.

Then it performs the required action (insert, update, delete, find) on that collection.

Simple Example Flow (for Orders)
OrderMongoRepository calls GetCollection<Order>() from IAtlasMongoRepository.

It gets the "Orders" collection from MongoDB.

Then it runs InsertOneAsync(order) directly on the collection




Configuration Management
We use appsettings.json and appsettings.Development.json to separate environment-specific settings (like DB connections, URLs, etc.).

Environment Variables are used for sensitive data like passwords and secrets.

Example:
MongoDbSettings__Password
(the double underscore __ maps to nested JSON keys).

This makes our application more secure, more flexible, and easier to deploy across different environments.

Useful links:
[ASP.NET Core - Configuration Overview](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-9.0)

Environment Variables in ASP.NET Core



