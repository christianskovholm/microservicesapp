# Idea service

* [Overview](#Overview)
* [Design](#Design)
  * [REST](#REST)
  * [CRUD](#CRUD)
  * [CQRS](#CQRS)
  * [Package dependency inversion](#Package-dependency-inversion)
  * [Hot reloading](#Hot-reloading)
  * [Docker](#Docker)
  * [Testing](#Testing)

## Overview

Idea service is a [Go](https://golang.org/) application. It serves the purpose of demonstrating best practices and patterns for developing and designing Go microservices. Recall that microservicesapp is developed as a small knowledge sharing platform, to demonstrate the practices and patterns in a real world context. In this domain, idea service is responsible for creating, updating and retrieving ideas created by members of an organization.

## Design

This section highlights the central design methods applied in the design of idea service. Please refer to the code to learn how to replicate the methods in your own projects.

### REST

Idea service follows the [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) architectural style. The resources that idea service expose are available at logically named endpoints. For example, to create an idea, a `POST` request is made against the `/organizations/{id}/ideas` endpoint. Because ideas belong to an organization, the URL for ideas is 'nested' within the organizations endpoint. A benefit to this approach is that the create operation `/organizations/{id}/ideas` requires the organization id. The id parameter can easily be retrieved from the request URL in the code.

### CRUD

Idea service follows the [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) pattern. There are endpoints for each of the create (`POST`), read (`GET`), update (`PUT`) and delete (`DELETE`) operations. These can be viewed in the [route configuration for Server](http/server.go#L47). 

### CQRS

[CQRS](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs) is implemented with [commands](commands/commands.go) for data manipulation and [queries](queries/queries.go) for data retrieval. The commands are processed by their handle methods through the [command dispatcher](commands/dispatcher.go#L25). The queries are implemented as raw SQL queries for fast performance.

### Package dependency inversion

The code is separated into [packages](https://www.golang-book.com/books/intro/11). Following the [dependency inversion principle](https://en.wikipedia.org/wiki/Dependency_inversion_principle) of the [SOLID](https://en.wikipedia.org/wiki/SOLID) design principles, code in the packages do not depend directly on implementations defined in other packages in the code base. Code in packages instead depend on interfaces defined in their own package, and implementations for these are then provided in the [entrypoint of the application](cmd/ideaservice/main.go#L12).

### Hot reloading

Idea service utilizes [inotifywait](https://linux.die.net/man/1/inotifywait) in a [bash script](scripts/dev.sh) for hot reloading. When the application is started in the development container, it will exit and rebuild upon code changes. This improves the development workflow, as the developer does not have to manually restart the application everytime a code change is made.

### Docker

The application is run in a [Docker](https://www.docker.com/) container. The [dockerfile](Dockerfile) utilizes [multi-stage builds](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/#use-multi-stage-builds). The stages are ordered so that system and package dependencies are cached in the base and restore stage and used in subsequent stages test and build. By ordering the stages like this, the cached dependencies can easily be reused for faster testing and deployment during CI/CD. This also has the added benefit that dependency caching is delegated to docker, making caching CI/CD vendor agnostic.

With Go, it is possible to build an application as an executable binary that targets a specific type of operating system. Because the binary can be built to not have any system dependencies other than the target Linux OS, which is the case with idea service, it can be built as a docker image from the [scratch](https://hub.docker.com/_/scratch) image. The build stage copies the binary to the deploy stage, which derives from the scratch image. This results in a filesize of just around 7MB, making startup time of the app very fast.

### Testing

Idea service follows the [Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development) (TDD) approach. The tests span [unit tests](https://en.wikipedia.org/wiki/Unit_testing), [integration tests](https://en.wikipedia.org/wiki/Integration_testing) and [functional tests](https://en.wikipedia.org/wiki/Functional_testing). Following the best practice advised by the go team, the tests are located in the same folder as the file being tested, and are suffixed with test. For example, the tests for [server.go](http/server.go) are named [server_test.go](http/server_test.go).