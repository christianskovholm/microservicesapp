# Organization Service

* [Overview](#Overview)
* [Design](#Design)
    *  [CQRS](#CQRS)
    *  [3-Tier Architecture](#3-Tier-Architecture)
    *  [Domain Driven Design](#Domain-Driven-Design)
    *  [Docker](#Docker)
    *  [Testing](#Testing)

## Overview

Organization service is an [ASP.NET Core 3.1](https://dotnet.microsoft.com/learn/aspnet/what-is-aspnet-core) application. It serves the purpose of demonstrating best practices and patterns for developing and designing ASP.NET Core and [.NET Core](https://dotnet.microsoft.com/) microservices. Recall that microservicesapp is developed as a small knowledge sharing platform to demonstrate the practices and patterns in a real world context. In this domain, organization service is responsible for managing departments, members and roles within an organization.

## Design

This section highlights the central design methods applied in the design of organization service. Please refer to the code to learn how to replicate the methods in your own projects.

### CQRS

[CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) is implemented with [commands](src/OrganizationService.Application/Commands) for data manipulation and [queries](src/OrganizationService.Infrastructure/Queries/Queries.cs) for data retrieval. The commands are sent to their respective command handlers using [MediatR](https://github.com/jbogard/MediatR) as a command bus. The queries are implemented as raw SQL queries executed by [dapper](https://github.com/StackExchange/Dapper) for fast performance.

### 3-Tier Architecture

Organization service follows the 3-Tier architecture pattern. The tiers, or layers, are implemented as separate projects. [OrganizationService.Application](src/OrganizationService.Application) contains the API and application configuration logic, [OrganizationService.Domain](src/OrganizationService.Domain) contains the business logic and [OrganizationService.Infrastructure](src/OrganizationService.Infrastructure) contains the persistence logic. Separating the codebase into layers like this also adheres to the [Separation of Concerns](https://en.wikipedia.org/wiki/Separation_of_concerns) (SoC) principle, which makes the application easier to maintain.

### Domain Driven Design

Organization service follows a subset of the [Domain Driven Design](https://en.wikipedia.org/wiki/Domain-driven_design) (DDD) principles. It implements the [aggregate root pattern](https://en.wikipedia.org/wiki/Aggregate) where the root of the domain objects, [Organization](src/OrganizationService.Domain/Aggregates/Organization/Organization.cs), contains all the other domain objects as either children og grandchildren.

All operations in the aggregate create [domain events](src/OrganizationService.Domain/DomainEvents) as side effects. These are stored in the domain object where they occurred. The domain events are [persisted to the database](src/OrganizationService.Application/Behaviors/TransactionPipelineBehavior.cs) when the aggregate root is updated. To manage organizations in the database, the [repository pattern](https://deviq.com/repository-pattern/) is implemented with a [repository](src/OrganizationService.Infrastructure/OrganizationRepository.cs) as a tier between the application logic and the [Entity Framework data access layer](src/OrganizationService.Infrastructure/OrganizationDbContext.cs).

### Docker

The application is run in a [Docker](https://www.docker.com/) container. The [dockerfile](Dockerfile) utilizes [multi-stage builds](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/#use-multi-stage-builds). The stages are ordered so that system and package dependencies are cached in the base and restore stage and used in subsequent stages test and deploy. By ordering the stages like this, the cached dependencies can easily be reused for faster testing and deployment during CI/CD. This also has the added benefit that dependency caching is delegated to docker, making caching CI/CD vendor agnostic.

### Testing

Organization service uses [xunit](https://xunit.net/) for testing and follows the [Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development) (TDD) approach. The tests span [unit tests](https://en.wikipedia.org/wiki/Unit_testing), [integration tests](https://en.wikipedia.org/wiki/Integration_testing) and [functional tests](https://en.wikipedia.org/wiki/Functional_testing). The tests are separated into projects per test type. The projects are located in the [test folder](test).