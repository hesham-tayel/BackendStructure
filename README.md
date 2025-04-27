1. Purpose & Responsibilities
This backend service is responsible for:

Exposing HTTP endpoints for all domain operations (CRUD and beyond).

Implementing business logic and validation rules in a centralized, testable layer.

Persisting and retrieving data from the configured data store (MongoDB).

Authenticating and authorizing each request to enforce security policies.


To better organize how the service handles reading and writing operations, we adopted the CQRS (Command Query Responsibility Segregation) pattern."

Why We Use CQRS

Separation of Reads vs. Writes
By splitting “commands” (which change state) from “queries” (which read state), each side can be optimized independently. You avoid entangling validation, authorization or transaction logic in your read paths, and you keep your write paths focused on business rules.

Clarity of Intent
A CreateOrderCommand clearly expresses “I want to create an order,” while a GetUserByIdQuery says “I want this user’s data.” There’s no confusion about side-effects.

Scalability
Reads and writes often have different performance and availability requirements. For example, you might put a fast in-memory cache in front of your read operations to serve frequent lookups, while your write pathway focuses on durability and validation without worrying about cache coherency.

Testability
Each handler is a small, focused piece of logic you can unit-test in isolation, mocking only the pieces it depends on.

Evolving Requirements
When new business rules arrive, you add or modify the corresponding command handler without risking regressions in your read logic (and vice versa).

To apply CQRS, we used MediatR to connect our controllers to the business logic.
Instead of the controller calling services or repositories directly, it sends a command or a query using MediatR. MediatR then finds the right handler to process the request.

This helped us:

Keep controllers clean and simple.

Move all important logic into handlers.

Make testing easier because each handler does only one thing.

In our project, we organized it like this:

All commands (for actions like create, update, delete) are placed in the Commands/ folder, each with its own handler.

All queries (for getting data) are placed in the Queries/ folder, each with its own handler.

In controllers, we just call _mediator.Send(...) and wait for the result.

Handlers are the ones that do the real work, like saving to the database or getting data


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
ASP.NET Core - Configuration Overview

Environment Variables in ASP.NET Core



