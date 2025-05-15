# Connectiq Application

## Overview

Connectiq is a modular, scalable microservices application designed to manage customer data efficiently with modern architectural patterns and technologies.

---

## Architecture

The system is built following a **microservices architecture** with clear separation of concerns:

- **Connectiq.AppHost:**  
  The main service acting as the application host. It integrates core components and connects to infrastructure services.

- **Connectiq.Contracts:**  
  Defines the domain contracts and messages exchanged between services using **gRPC** for strongly-typed and efficient communication.

- **CustomerWorker:**  
  Handles customer-specific business logic including validation and processing of customer data.

- **DatabaseWorker & PersistenceWorker:**  
  Responsible for database migrations, seeding, and maintaining schema consistency automatically throughout development and deployment.

---

## Communication and Messaging

- **gRPC:**  
  Used within `Connectiq.Contracts` to define service contracts and enable fast, type-safe RPC communication between services.

- **RabbitMQ:**  
  Manages asynchronous messaging and event-driven communication between microservices ensuring loose coupling and scalability.

- **CQRS (Command Query Responsibility Segregation):**  
  Command and query responsibilities are separated, improving scalability and maintainability.

- **AutoMapper:**  
  Used to simplify object-to-object mapping across DTOs, entities, and event messages.

---

## Technologies Used

- **.NET 8:**  
  Modern cross-platform framework powering the services.

- **PostgreSQL:**  
  The relational database management system for persisting customer data.

- **RabbitMQ:**  
  Message broker for event-driven asynchronous communication.

- **gRPC:**  
  Protocol for efficient communication and contract definition between services.

- **AutoMapper:**  
  Simplifies mapping between domain models and DTOs.

- **MassTransit:**  
  Abstraction over RabbitMQ to facilitate message handling.

- **FluentValidation:**  
  For enforcing business rules and input validation on incoming data.

- **GitHub Actions:**  
  For CI/CD workflows and ensuring code quality through tests and coverage (planned).

---

## What We've Implemented So Far

- Setup of PostgreSQL and RabbitMQ for persistence and messaging.
- Defined domain contracts and messages with gRPC.
- Implemented customer management microservice (`CustomerWorker`) with validation and event handling.
- Automated database migrations and seeding with `DatabaseWorker` and `PersistenceWorker`.
- Applied CQRS pattern separating commands and queries.
- Configured object mapping using AutoMapper for clean DTO conversions.
- Established a foundation for testing and code coverage to ensure quality and performance.

---

## Next Steps

- Implement comprehensive unit and integration tests with coverage.
- Add benchmark tests to measure performance.
- Configure branch protections and CI/CD pipelines.
- Extend microservices and communication contracts as needed.

---

## How to Run

1. Make sure you have PostgreSQL and RabbitMQ services running and accessible.
2. The application uses **Aspire** for automatic configuration, so no manual `appsettings.json` setup is needed.
3. Build and run the services using `dotnet run` or via Docker containers.
4. Interact with the services through the provided gRPC clients or API endpoints.

---

Feel free to explore the repository and reach out for any questions or collaborations!

