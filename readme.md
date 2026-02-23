# TvTracker

TvTracker is a full-stack, server-side rendered web application built with ASP.NET Core Razor Pages.
It allows users to track and rate movies and TV shows across user profiles.

This project was designed as a hands-on learning and portfolio exercise to familiarize myself with the .NET development, emphasizing clean architecture, domain-driven design, and database modeling.


## Tech Stack

Backend: ASP.NET Core, Razor Pages

Frontend: Razor Views + JavaScript for dynamic ui updates. 

Database: EF Core, SQLite 

Architecture: Pages, Service layer pattern + ViewModels

## Key concepts used

- Domain-Driven Design & Table Hierarchies - a strong emphasis was placed on DDD principles. Therefore, data model use hierarchical structures implemented via **TPC (tables-per-concrete type)** and **TPH (table-per-hierarchy)** to enable better reusability,domain representation wile maintaining clear separation. 

- Fluent API - entity relationships, navigation and inheritance were explicitly configured via FluentAPI for better control over schema generation and documentation. Data annotation weren't used as i found the alternative to be cleaner as it allows to separate mapping logic from the entity model.

- Razor pages - since this project is meant to be a pure .NET/C# learning project i decided to implement this app as an SSR to minimize frontend code, however, as some ui elements still require dynamic behavior JS was used in those places.

## setup

First apply the migrations to setup db tables:
```
dotnet ef database update
```
Then run the app via:
```
dotnet run
```