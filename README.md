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



