# DomeGym

**DomeGym** is a Gym Session Scheduling & Booking System designed as a proof of concept for implementing **Domain-Driven Design (DDD)** principles and building blocks.

While modern DDD implementations avoid Repositories, Unit of Work, Aggregates, and Aggregate Roots simply because EF Core is already built on top of these patterns, this project intentionally models them to focus on demonstrating core DDD principles.

## What is Domain-Driven Design?

Domain-Driven Design (DDD) is a software development philosophy that emphasizes the importance of understanding and modeling the business domain. It is a strategy aimed at improving the quality of software by aligning it more closely with the business needs it serves.

Originated by Eric Evans in his book *Domain-Driven Design: Tackling Complexity in the Heart of Software* (2003), DDD helps manage complex domains by aligning the software model with the business needs.

By managing complex domains through this alignment, DDD promotes better structure, maintainability, and consistency in the codebase, reducing divergence between the system and the real-world domain it represents. Ultimately, this makes the system easier and more enjoyable to work with over time.

## The Two Parts of DDD

Domain-Driven Design (DDD) is divided into two main aspects: **Strategic Design** and **Tactical Design**. Both are essential but serve different purposes within software development.

### Strategic Design

**Definition**: Strategic Design focuses on performing a high-level analysis of the domain and its constraints, staying away from technical details. It's not about writing a code or thinking of any specific tech stack, or architecture.

**Key Practices:**
1. Identifying and defining the business **Core Domains** and **Subdomains**.
2. Establishing clear boundaries between different parts of the system (**Bounded Contexts**).
3. Developing a shared language (**Ubiquitous Language**) within each **Bounded Context**.
4. Mapping relationships between different **Bounded Contexts** and **Teams** (**Context Mapping**).
5. Aligning the software architecture with the business strategy and domain structure.

In DDD, a _bounded context_ is typically a microservice. Microservices should be designed around business capabilities, not horizontal layers such as data access or messaging. In addition, they should have loose coupling and high functional cohesion. Microservices are *loosely coupled* if you can change one service without requiring other services to be updated at the same time. A microservice is *cohesive* if it has a single, well-defined purpose, such as managing user accounts or tracking gym session participation. A service should encapsulate domain knowledge and abstract that knowledge from clients. For example, a client should be able to schedule a gym session without knowing the details of the scheduling algorithm or how the trainer availability is managed.

**Example**: **DomeGym** uses Strategic Design to define distinct bounded contexts for user management, session reservation, and gym management. This separation allows independent development and deployment, reducing interdependencies.

### Tactical Design

**Definition**: Tactical Design deals with the implementation details within each bounded context. It focuses on how to design and build the individual components, such as entities, value objects, aggregates, repositories, and services.

**Tactical DDD patterns:**
- **Entities**: Domain objects identified by unique IDs; equality is based on ID alone, not attributes.
- **Value Objects**: Immutable domain objects defined by attributes rather than by a unique ID. To update a value object, you always create a new instance to replace the old one. Value objects can have methods that encapsulate domain logic, but those methods should have no side-effects on the object's state. Typical examples of value objects include _colors_, _dates and times_, and _currency values_.
- **Aggregates and Aggregate Roots**: An aggregate is a cluster of entities and value objects that function as a single unit. The aggregate root is the main entry point for interacting with the aggregate, enforcing consistency rules.
- **Repositories**: Repositories provide access to aggregates. They encapsulate the logic for retrieving and storing aggregates and provide a convenient API for interacting with them.
- **Domain and Application Services**: A service is an object that implements some logic without holding any state. Evans distinguishes between _domain services_, which encapsulate domain logic, and _application services_, which provide technical functionality, such as user authentication or sending an SMS message. Domain services are often used to model behavior that spans multiple entities.
- **Domain Events**: Domain events represent something that has happened in the domain, signaling a change or action. They are immutable, capturing facts from the past. These events help decouple different parts of the system by notifying other components about changes, triggering necessary side effects. Domain events promote a cleaner separation of concerns and enable eventual consistency, which is a simple pattern in which processing is divided. An elegant solution for publishing domain events in EF Core is by overriding `SaveChangesAsync`. This ensures events are published after database changes, though it introduces tradeoffs: eventual consistency (because messages are processed after the original transactions) and a risk of database inconsistency (because handling domain events can fail). Eventual consistency is something we can live with, however, introducing a risk of database inconsistency is a big concern. This can be mitigated with the Outbox pattern, where domain events are saved in a transaction, ensuring atomicity and asynchronous processing.

There are a few other DDD patterns not listed here, including _factories_, and _modules_. Modules are essentially namespaces. They define how we organize our code into different logical modules, each with its own distinct purpose in the system.

**Example**: In the **DomeGym** context, Tactical Design is used to implement the user management bounded context. Users are modeled as entities, addresses as value objects, and use repositories to access user records. This granular approach ensures that the user management system is robust and easy to extend.

### Domain Events vs Integration Events

Semantically, they're the same thing: a representation of something that occurred in the past. However, their intent is different and this is important to understand.

**Domain Events:**
- Published and consumed within a single domain
- Sent using an in-memory message bus
- Can be processed synchronously or asynchronously

**Integration Events:**
- Consumed by other subsystems (microservices, Bounded Contexts)
- Sent with a message broker over a queue
- Processed completely asynchronously

### Comparing Strategic and Tactical Design
- **Scope**: Strategic Design is macro, focusing on the big picture; Tactical Design is micro, focusing on implementation.
- **Focus**: Strategic Design deals with understanding and organizing the business domain; Tactical Design is concerned with how to build within those boundaries.
- **Output**: Strategic Design produces bounded contexts and context maps; Tactical Design produces domain models and code structures.

## Example: Gym Session Scheduling & Booking System

### 1. Ubiquitous Language

We start by defining the Ubiquitous Language and marking domain objects and verbs:

- A `user` can `create` a `participant` `profile`
- `Participants` can `reserve` a `spot` in a session
- A `session` takes place in a `room`
- A `session` has a single `trainer` and a maximum number of participants
- A `gym` can have multiple `rooms`
- A `user` can `create` an `admin` `profile`
- An `admin` can have an `active subscription`
- A `subscription` can have multiple gyms
- An `active subscription` can be of type `Free`, `Starter`, or `Pro`.
- A `user` can `create` a `trainer` `profile`
- A `trainer` can `teach` `sessions` across `gyms` and `subscriptions`

**Consistency**: Once terms like "create" vs "open" are chosen, they should be used consistently throughout the system.

### 2. Domain Objects

Based on the Ubiquitous Language, the following Domain Objects are identified:

- **User**
- **Participant**
- **Admin**
- **Trainer**
- **Subscription**
- **Gym**
- **Room**
- **Session**

### 3. Domain Invariants

Invariants are simply business rules. Things in our system that must always be true.

**Session Invariants**
- A session cannot contain more than the maximum number of participants
- A reservation cannot be canceled for free less than 24 hours before the session starts

**Gym Invariants**
- A gym cannot have more rooms than the subscription allows

**Room Invariants**
- A room cannot have more sessions than the subscription allows
- A room cannot have two or more overlapping sessions

**Subscription Invariants**
- A subscription cannot have more gyms than the subscription allows

**Trainer Invariants**
- A trainer cannot teach two or more overlapping sessions

**Participant Invariants**
- A participant cannot reserve overlapping sessions

## Anemic Domain Model vs. Rich Domain Model

In this project, the entities follow the **Rich Domain Model** concept, which comes from Domain-Driven Design.

This concept allows us to implement business rules within the entities, in one place. This avoids spreading business rules throughout the codebase in different classes, making the code more manageable.

You can follow the **Domain-Driven Design** principle for your domain entities or use **anemic entities** with plain `get` and `set` properties.

### Anemic Domain Model

```csharp
public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime Date { get; set; }
    public List<OrderItem> Items { get; set; } = [];
}
```

### Rich Domain Model

```csharp
public class Order
{
    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public DateTime Date { get; private set; }
    public ShipmentStatus OrderStatus { get; private set; }

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    private readonly List<OrderItem> _items = new();

    private Order() { }

    public static Order Create(string orderNumber, Customer customer, List<OrderItem> items)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = orderNumber,
            Customer = customer,
            CustomerId = customer.Id,
            Date = DateTime.UtcNow
        }.AddItems(items);
    }

    private Order AddItems(List<OrderItem> items)
    {
        _items.AddRange(items);
        return this;
    }

    public ErrorOr<Success> Process()
    {
        if (Status is not OrderStatus.Created)
        {
            return Error.Validation("Can only update to Processing from Created status");
        }

        Status = OrderStatus.Processing;
        Date = DateTime.UtcNow;

        return Result.Success;
    }
}
```

## References

- [Strategic Design](https://learn.microsoft.com/en-us/azure/architecture/microservices/model/domain-analysis#introduction)
- [Tactical Design](https://learn.microsoft.com/en-us/azure/architecture/microservices/model/tactical-ddd#drone-delivery-applying-the-patterns)
- [Domain Events vs Integration Events](https://www.milanjovanovic.tech/blog/how-to-use-domain-events-to-build-loosely-coupled-systems)
- [Outbox Pattern](https://www.milanjovanovic.tech/blog/outbox-pattern-for-reliable-microservices-messaging)

## Generating Migrations and Applying Them to the Database

EF Core is used to manage migrations and database updates in **DomeGym**. Below are the steps and commands for generating and applying migrations for the various bounded contexts in the system.

### General Steps
1. Ensure the `.NET CLI` tools are installed on your system.
2. Navigate to the root directory of the project.
3. Use the following commands to generate and apply migrations for the respective contexts.

### Commands for Each Bounded Context

#### GymManagement

1. **Generate the initial migration**:
   ```bash
   dotnet ef migrations add Init \
       --project GymManagement\src\GymManagement.Infrastructure \
       --startup-project GymManagement\src\GymManagement.Api \
       --context GymManagementDbContext \
       --output-dir Persistence\Migrations
   ```

2. **Apply the migration to the database**:
   ```bash
   dotnet ef database update \
       --project GymManagement\src\GymManagement.Infrastructure \
       --startup-project GymManagement\src\GymManagement.Api \
       --context GymManagementDbContext
   ```

#### SessionReservation

1. **Generate the initial migration**:
   ```bash
   dotnet ef migrations add Init \
       --project SessionReservation\src\SessionReservation.Infrastructure \
       --startup-project SessionReservation\src\SessionReservation.Api \
       --context SessionReservationDbContext \
       --output-dir Persistence\Migrations
   ```

2. **Apply the migration to the database**:
   ```bash
   dotnet ef database update \
       --project SessionReservation\src\SessionReservation.Infrastructure \
       --startup-project SessionReservation\src\SessionReservation.Api \
       --context SessionReservationDbContext
   ```

#### UserManagement

1. **Generate the initial migration**:
   ```bash
   dotnet ef migrations add Init \
       --project UserManagement\src\UserManagement.Infrastructure \
       --startup-project UserManagement\src\UserManagement.Api \
       --context UserManagementDbContext \
       --output-dir Persistence\Migrations
   ```

2. **Apply the migration to the database**:
   ```bash
   dotnet ef database update \
       --project UserManagement\src\UserManagement.Infrastructure \
       --startup-project UserManagement\src\UserManagement.Api \
       --context UserManagementDbContext
   ```
