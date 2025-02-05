# IPROX TV Show Data Sync and Web API Development Report

## By: Sanjith Gunaratne

## Running the Project Locally

### 1. Setting Up the Database
- Use the provided sample database located in the `Meta/Database` folder.
- Restore the database and update the connection string in `appsettings.json` accordingly.
- Alternatively, run database migrations using the following steps:
  - Set the startup project as `Iprox.Presentation.TvShows.Api`.
  - In Package Manager Console, select `Iprox.Infrastructure.Persistence` as the default project.
  - Run the command:
    ```sh
    Update-Database
    ```

### 2. Configuring Connection Strings and App Settings

#### For Web API and Minimal API:
Modify the `appsettings.json` file with the appropriate database connection string:
```json
{
  "ConnectionStrings": {
    "ApplicationDbContext": "Server=.;Database=IproxDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### For Azure Function:
Create/Modify the `local.settings.json` file with the correct storage connection string:
```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=iproxtvshowstorage;AccountKey=7Dd0TIzb+YDQyqIZorIxRyUCvLzHre2U7DK9rZt0ioM+CPu5+cyQ1+3br75XRJGEKTDcIXVC1LAw+AStLNLTVw==;EndpointSuffix=core.windows.net",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
    },
    "ConnectionStrings": {
        "ApplicationDbContext": "Server=.;Database=IproxDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
    }
}
```

### 3. Running the APIs and Azure Function Locally

#### To Run Web API (Controller-Based)
- Navigate to `Iprox.Presentation.TvShows.Api`.
- Run the project using Visual Studio or the following command in the terminal:
  ```sh
  dotnet run
  ```

#### To Run Minimal API
- Navigate to `Iprox.Presentation.TvShows.Minimal.Api`.
- Run the project using:
  ```sh
  dotnet run
  ```

#### To Run Azure Function
- Navigate to `Iprox.Presentation.Functions.TvShowFunctions`.
- Start the function using:
  ```sh
  func start
  ```

---

## Completed Work

### .NET 8 Web API, Minimal API & Azure Function

- **Clean Architecture:** The project follows clean architecture principles to ensure scalability and maintainability.
- **SOLID Principles:** Applied SOLID principles for better maintainability and extensibility.
- **Repository Pattern & Unit of Work:** Implemented to improve separation of concerns and transaction management.
- **DRY Principle:** Eliminated code duplication by creating reusable methods and shared components.
- **API Versioning (v1.0):** Added versioning in the Minimal API project while maintaining default settings in the controller-based API.
- **Customized API Documentation (Swagger):** Configured for easy testing and documentation (available in Minimal API with default Swagger settings in controller-based API).
- **Enums:** Utilized for managing categorical values for better code readability.
- **Entity & LINQ:** Used for efficient database interactions and queries.
- **Seeding & Migrations:** Implemented database seeding and migrations with schema management.
- **Caching:** MemoryCache added for performance improvements on `GET api/show` endpoint.
- **Pagination:** Implemented for better data handling and performance.
- **Unit Tests:** Started writing unit tests, including `CreateAsync_ShouldLogError_WhenExceptionIsThrown()`.
- **Search for a Show by Name:** Implemented a search feature for TV shows by name.
- **Display Shows in Descending Order of Premiere Date:** Ensured proper sorting.
- **Database Foreign Keys for Genres:** Created a separate genre master table and mapping table for proper relationships.
- **Controller and Minimal API:** Both approaches are implemented for CRUD operations.
- **Async Programming:** Applied throughout for non-blocking performance.
- **Azure Function for TV Show Data Sync:**
  - Uses a timer trigger to sync data from the TV Maze API every minute.
  - Fetches only the latest sync ID to avoid hitting API rate limits.
  - Retrieves ~250 shows per call and updates the database accordingly.

---

## Pending Work

### 1. Implement Configurable Parameters
- Currently, only connection strings are configurable.
- Other parameters (e.g., `pageCount`) are still hardcoded.
- Moving these parameters to configuration settings (e.g., `appsettings.json`) will enhance flexibility.
- Implementing the IOptions pattern for reading configuration settings will improve separation of concerns.

### 2. Stored Procedures
- Stored procedures have been implemented for complex queries, improving performance and maintainability.

### 3. Timeouts
- Implement timeout management for external API calls and database queries to prevent long-running operations.

---

This report summarizes the work completed, the current state of the project, and next steps for improvements.

