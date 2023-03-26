# .NET 6 Web API Example Project
This is a .NET 6 Web API project built using `Onion Architecture`. It consists of the following layers:

- **Core Layer**: Contains the domain model classes and repository interfaces.
- **Application Layer**: Contains the DTO to model conversion and business logic.
- **Data Access Layer**: Contains the repository implementations using `EF Core`.
- **Web API Layer**: Contains the controllers and `JWT authorization`.

There is only one controller and each method is documented to give you a broad idea what each endpoint is meant to do.

## Requirements

- .NET 6 SDK

## Installation

1. Clone the repository or download the code.
2. Open the solution in Visual Studio or any other .NET 6 IDE.
3. Run `dotnet restore` to restore the required packages.
6. Run the project using `dotnet run` or by pressing F5 in Visual Studio.

## Layers

### Core Layer

The Core Layer contains the domain model classes and repository interfaces. 
It is responsible for defining the entities and their relationships, as well as the contracts for interacting with the data storage layer.

### Application Layer

The Application Layer contains the DTO-model conversion and business logic. It is responsible for converting the domain models into DTOs that are used by the Web API Layer, a
s well as implementing the business rules and logic that govern the behavior of the application.

### Data Access Layer

The Data Access Layer contains the Entity Framework and repository implementations. 
It is responsible for handling the storage and retrieval of data from the database, as well as implementing the repository interfaces defined in the Core Layer.

### Web API Layer

The Web API Layer contains the controllers and JWT authorization. 
It is responsible for exposing the application's functionality to the outside world through RESTful APIs. 
JWT authentication is used to secure the APIs and prevent unauthorized access.

## Notable Considerations

### Global Exception Handler
The project has a global exception handler implemented using an error handling filter attribute which returns `ProblemDetails` objects. This ensures that any unhandled exceptions thrown by the API are caught and handled in a consistent way (`ErrorHandlingFilterAttribute` class).

### Singleton Pattern
The project has implemented the Singleton pattern for the `SensorSettings` class, which is responsible for holding the data from `appsettings.json` for future use. The appsettings data is bound to SensorSettings through `Builder extension method`. We could have also used DI with `AddSingleton` method instead of implementing the Singleton pattern manually.

### Factory Pattern
The project has implemented the Factory pattern for the `AlertHandlerFactory`, which is responsible for creating instances of the `AlertHandler` class and its derivatives based on the type of alert received. However, in this special case, the factory is returning a dictionary of all handlers at once.

### Template Method Pattern
The project has implemented the Template Method pattern for the `AlertHandler` class and its derivatives, which provides a common interface for handling alerts while allowing each specific type of alert to customize the implementation as needed through polymorphism. This solution enables easy addition of new types of alerts by simply adding a new handler without any changes to the existing code, adhering to the open-closed principle.
