# Activity service

* [Overview](#Overview)
* [Design](#Design)
    * [uWSGI](#uWSGI)
    * [Psycopg2](#Psycopg2)
    * [Pipenv](#Pipenv)
    * [Docker](#Docker)
    * [Testing](#Testing)

## Overview

Activity service is a [Python](https://www.python.org/) application developed with [Flask](https://flask.palletsprojects.com/en/1.1.x/). It serves the purpose of demonstrating best practices and patterns for developing and designing Python and Flask microservices. Recall that microservicesapp is developed as a small knowledge sharing platform to demonstrate the practices and patterns in a real world context. In this domain, activity service listens to the events produced by [organization service](../organization-service) and [idea service](../idea-service), and saves them as activities in it's own database. Organizations then have a single source of truth for the activity created by operations in the other services.

## Design

### uWSGI

Activity service is served by [uWSGI](https://uwsgi-docs.readthedocs.io/en/latest/). uWSGI starts two processes on application startup. The master worker process, which runs an instance of the [Flask app object](activityservice/app.py#L42), and a [mule](https://uwsgi-docs.readthedocs.io/en/latest/Mules.html) process which starts a [kafka consumer](activityservice/mule.py#L10). It is considered a good practice to separate the worker process that serves web requests from the process that handles incoming messages from kafka. By having these processes run separately, the app is free from having to start a background thread for consuming messages during instantiation of the Flask app object.

### Psycopg2

Both the worker process and the mule process use the [psycopg2](https://github.com/psycopg/psycopg2) database adapter for database access. The adapter is fast at making raw SQL queries. The reason for choosing to do raw SQL queries instead of an [ORM](https://en.wikipedia.org/wiki/Object%E2%80%93relational_mapping) is because activity service has a single domain model, [Activity](activityservice/db.py#L11). I believe that ORMs are better suited for applications with more domain models. The psycopg2 adapter is also significantly faster than an ORM such as [SQLAlchemy](https://www.sqlalchemy.org/) in terms of performance.

### Pipenv

Activity service utilizes [Pipenv](https://docs.pipenv.org/) for management of packages and virtual environments. The [Pipfile](Pipfile) declares packages and virtual environments, which can then be installed by the [Pipenv CLI](https://pipenv.pypa.io/en/latest/cli/). Without Pipenv, developers have to manage their packages and venvs separately, which may leed to inconsistencies between these. Pipenv ensures these are in sync, and this frees up time for developers to be more productive in other areas.

Activity service has two virtualenvs. A base environment with packages used in production and in dev and test environments, and a dev environment with packages used in development and testing. By installing the dev environment, the packages of the base environment also gets installed.

### Docker

The application is run in a [Docker](https://www.docker.com/) container. The [dockerfile](Dockerfile) utilizes [multi-stage builds](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/#use-multi-stage-builds). The stages are ordered so that system and package dependencies are cached in the base stage and used in subsequent stages dev, test and deploy. By ordering the stages like this, the cached dependencies can easily be reused for faster testing and deployment during CI/CD. This also has the added benefit that dependency caching is delegated to docker, making caching CI/CD vendor agnostic. 

### Testing

Activity service uses [pytest](https://docs.pytest.org/en/stable/) for testing and follows the [Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development) (TDD) approach. The application also follows the conventional Python approach of having tests in a [tests folder](tests) next to the [package folder](activityservice) at the root project folder level. Writing tests is considered a best practice, as it helps to ensure that the internal components, their integration, and the overall application work as expected. The tests cover [unit tests](https://en.wikipedia.org/wiki/Unit_testing), [integration tests](https://en.wikipedia.org/wiki/Integration_testing), and [functional tests](https://en.wikipedia.org/wiki/Functional_testing).