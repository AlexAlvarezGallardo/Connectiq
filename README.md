# 🚀 Connectiq Application

![✅ Build Status](https://img.shields.io/github/actions/workflow/status/AlexAlvarezGallardo/connectiq/ci.yml?branch=main)  
![📄 License](https://img.shields.io/github/license/AlexAlvarezGallardo/connectiq)  
![🟣 .NET](https://img.shields.io/badge/.NET-8.0-blue)

---

## 📋 Overview

Connectiq is a modular, scalable microservices application designed to manage customer data efficiently using modern architectural patterns and technologies.

---

## 🏗 Architecture

The system follows a **microservices architecture** with clear boundaries and responsibilities:

- **🖥️ Connectiq.AppHost:**  
  The main entry point that hosts and orchestrates the service configuration.

- **📜 Connectiq.Contracts:**  
  Defines strongly-typed domain contracts and messages using **gRPC** for inter-service communication.

- **👤 CustomerWorker:**  
  Encapsulates customer-related business logic including validation, mapping, and event handling.

- **💾 DatabaseWorker:**  
  Handles database schema migrations and seeding at startup to keep the database up to date during development and deployment.

> 🗑️ `PersistenceWorker` has been removed to avoid cross-service coupling and direct context dependencies. All persistence logic is now managed independently within each domain.

---

## 🔗 Communication and Messaging

- **⚡ gRPC:**  
  Enables fast, contract-first communication between services.

- **🐰 RabbitMQ with MassTransit:**  
  Supports event-driven architecture and asynchronous messaging between microservices, using topic-based routing and configurable bindings.

- **📐 CQRS (Command Query Responsibility Segregation):**  
  Improves maintainability and scalability by splitting commands and queries into distinct paths.

- **🛠 AutoMapper:**  
  Used for mapping between domain entities, DTOs, and event messages.

---

## 🛠 Technologies Used

- **🟣 .NET 8**  
- **🐘 PostgreSQL** – primary relational database.  
- **🐰 RabbitMQ** – event bus with quorum queue configuration.  
- **📩 MassTransit** – abstraction layer for message handling.  
- **⚡ gRPC** – cross-service contract-first communication.  
- **✅ FluentValidation** – input validation and business rule enforcement.  
- **⚙️ GitHub Actions** – CI/CD automation.

---

## 📈 What We've Implemented So Far

- PostgreSQL and RabbitMQ infrastructure with Aspire support.  
- Fully configured CQRS pattern in `CustomerWorker`.  
- gRPC-based domain contracts in `Connectiq.Contracts`.  
- Input validation using FluentValidation for commands and queries.  
- Automated schema migration with `DatabaseWorker`.  
- Dynamic MassTransit configuration using custom extensions and configuration sections.  
- Implemented soft delete and event publishing logic.  
- Enhanced test coverage with unit and integration tests.  
- Removed `PersistenceWorker` to enforce strict service boundaries and decoupling.

---

## 🎯 Next Steps

- Expand test coverage and add performance benchmarks.  
- Improve fault-tolerance with retry policies and circuit breakers.  
- Integrate OpenTelemetry for distributed tracing.  
- Add support for distributed caching.  
- Implement saga patterns for long-running workflows.

---

## 🚀 How to Run

1. Run the application using Aspire (via `dotnet run` in the AppHost project).  
2. All configuration is managed via `appsettings.*.json` and environment variables.  
3. Interact via gRPC clients or expose additional endpoints as needed.
4. Use GraphQL to interact with the application.

---

## 🤝 Contributing

We welcome contributions! To get started:

- Read [CONTRIBUTING.md](./CONTRIBUTING.md)  
- Open issues to report bugs or suggest enhancements  
- Fork the repo, create a feature branch, and open a PR with your changes

---

Feel free to explore the repository and join the development. Let’s build something great with Connectiq!
