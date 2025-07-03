# Gmail Client Application

A full-featured Gmail client application built with Clean Architecture, enabling users to authenticate via Google OAuth, read and send emails efficiently.

## Features

- Google OAuth authentication for secure access to Gmail data.
- Read and send emails directly from the application.
- Implemented pagination for fast and efficient email browsing.
- Separation of concerns using Mediator pattern and CQRS for clear responsibility distribution.

## Technology Stack

- **Frontend:** JavaScript, HTML, CSS
- **Backend:** ASP.NET Core 8, C#, Entity Framework Core
- **Database:** Microsoft SQL Server (MSSQL)
- **Architecture:** Clean Architecture with layered structure (API, Domain, Application, Persistence, Infrastructure, UI)

## Architecture Layers

- **API Layer:** Handles HTTP requests and exposes endpoints.
- **Domain Layer:** Contains core entities and business models.
- **Application Layer:** Implements CQRS with MediatR handlers for queries and commands.
- **Persistence Layer:** Manages database access and data storage via Entity Framework.
- **Infrastructure Layer:** Handles integration with Google OAuth and external services.
- **UI Layer:** Frontend components built with JS, HTML, and CSS.

## How to Run the Project Locally

### Getting Started

1. Clone the repository to your local machine.

```bash
git clone https://github.com/Mkrager/Gmail-Client.git
```

2. Set up your development environment. Make sure you have the necessary tools and packages installed.

3. Configure the project settings and dependencies. You may need to create configuration files for sensitive information like API keys and database connection strings.

4. Install the required packages using your package manager of choice (e.g., npm, yarn, NuGet).

5. Run the application locally for development and testing.

```bash
dotnet run
```

6.Access the application in your web browser at http://localhost:port.
