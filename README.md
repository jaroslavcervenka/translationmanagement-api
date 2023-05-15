# Translation management API

This is an example of ASP.NET core 7 backend application for translation management.
It provides some useful concepts and solutions.
You can find [client implementation](https://github.com/jaroslavcervenka/translationmanagement-api) in my repository as well.

Patterns
- Clean architecture
- CQRS - commands and queries
- Repository
- Unit of work
- Failing fast
- Validation
- Transient fault handling
- Testing automation - unit, integration and system tests
- Producer/consumer - channels
- Avoid using the exceptions for operation flow
- Separate domain and DTO objects

Libraries
- Ardalis.GuardClauses
- AutoMapper
- BetterHostedServices
- FluentResults
- MediatR
- Polly

## TODO
There are a few things I need to improve and implement. Sorry for that.
I hope it will be ASAP.

- Architecture diagram
- Use minimal API`s instead of controllers
- Validation - DI registration and response feedback
- Unit, integration and system tests
- Entity framework core - use immutable approach