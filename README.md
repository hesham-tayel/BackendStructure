# BackendStructure README

## 1. Purpose & Responsibilities

This backend service is responsible for:

- Exposing HTTP endpoints for all domain operations (CRUD and beyond).  
- Implementing business logic and validation rules in a centralized, testable layer.  
- Persisting and retrieving data from MongoDB.  
- Authenticating and authorizing each request to enforce security policies.  

---

## 2. Architectural Patterns

### 2.1 Command–Query Responsibility Segregation (CQRS)  
We separate writes (commands) from reads (queries) to keep each side focused on its job.  
Learn more: [CQRS pattern overview](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)

### 2.2 MediatR for In-Process Messaging  
Controllers send commands/queries to MediatR instead of calling services directly; MediatR then invokes the matching handler.  
Details: [MediatR on GitHub](https://github.com/jbogard/MediatR)

---

## 3. Configuration Management

- **`appsettings.json`** & **`appsettings.Development.json`** hold non-sensitive settings (connection strings, URLs, collection names).  
- **Environment variables** override secrets at runtime. For example:  
  - `MongoDbSettings__ConnectionString`  
  - `MongoDbSettings__Password`  
  The double-underscore (`__`) maps to the nested JSON key `MongoDbSettings:Password`.  

Learn more: [ASP.NET Core Configuration Overview](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-9.0)

---

## 4. Program.cs & Dependency Injection

`Program.cs` wires up everything in one place:

1. **Bind settings**  
   - Uses `Configure<MongoDbSettings>()` to read JSON + environment variables into a typed options class.  
2. **Register singletons**  
   - `IMongoClient` — one shared connection pool  
   - `IMongoDatabase` — points to the configured database  
   - `IAtlasMongoRepository` —generic “key-ring” for collections  
3. **Register scoped/transient**  
   - Feature repositories (`IOrderMongoRepository`, etc.)  
4. **Register infrastructure**  
   - `AddAutoMapper(...)`  
   - `AddMediatR(...)`  
   - `AddControllers()`  

Learn more: [ASP.NET Core Dependency Injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)

---

## 5. MongoDB Setup & Lifecycle

1. **Connection details**  
   - Defined under `"MongoDbSettings"` in `appsettings.json`:  
     ```json
     {
       "MongoDbSettings": {
         "ConnectionString": "mongodb+srv://user:<password>@cluster...",
         "DatabaseName": "Production"
       }
     }
     ```
2. **Environment overrides**  
   - Use **environment variables** to inject secrets at runtime:  
     - `MongoDbSettings__ConnectionString`  
     - `MongoDbSettings__Password`  
   - Ensures no passwords live in source control.  
3. **MongoClient**  
   - Acts as the gateway to the MongoDB cluster.  
   - Manages a pool of network connections for the application’s lifetime.  
4. **MongoDatabase**  
   - Represents the specific database (e.g. `Production`).  
   - All collections are retrieved from here.  
5. **IAtlasMongoRepository**  
   - A “key-ring” service with one method:  
     ```csharp
     IMongoCollection<T> GetCollection<T>(string name);
     ```  
   - Hides client/database details from feature code.  
6. **Singleton lifetimes**  
   - We register `IMongoClient`, `IMongoDatabase`, and `IAtlasMongoRepository` as singletons:  
     - **Performance:** one connection pool, no repeated handshakes  
     - **Thread-safety:** the driver is safe for concurrent use  
     - **Simplicity:** every component shares the same configured instance  

---

## 6. Repository Layer Structure

### 6.1 IAtlasMongoRepository (Generic)  
- Single responsibility: provide a method to retrieve any MongoDB collection by name.  
- Keeps collection-access logic centralized and reusable.  

### 6.2 Feature-Specific Repositories  
- Each domain model (e.g. `Order`, `User`) has its own interface, such as `IOrderMongoRepository`, defining operations like `InsertAsync`, `FindByIdAsync`.  
- Implementations (e.g. `OrderMongoRepository`) obtain the correct collection via `IAtlasMongoRepository` and perform the actual database calls.  
