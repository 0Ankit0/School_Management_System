# Workflow: School Management System (SMS)

**Document Version:** 2.8
**Date:** 2025-08-13
**Author:** Gemini CLI Agent

---

## 1. Introduction

This document provides a comprehensive and detailed overview of the typical workflows and user interactions within the School Management System (SMS). It describes the step-by-step processes for various tasks, considering different user roles, system responses, and potential edge cases. The aim is to make this document a self-contained reference for understanding system behavior without needing to consult other sources.

## 2. User Roles

The SMS supports a role-based access control (RBAC) system, defining specific permissions and functionalities for each user type:

- **Administrator:** Possesses full control over the system. Can manage users, students, teachers, courses, enrollments, system settings, and generate comprehensive reports.
- **Teacher:** Manages their assigned courses, views enrolled students, records grades, tracks attendance, posts assignments, and communicates with students and parents.
- **Student:** Accesses their academic records, attendance, assignments, school announcements, and communicates with their teachers.
- **Parent:** Monitors their child(ren)'s academic progress, attendance, school announcements, and communicates with their child's teachers.

## 3. Core Workflows

### 3.1. UI Development Workflow

This workflow outlines the process for creating and maintaining UI elements in the `SMS.UI` project.

1.  **Developer Action:** Identify a UI element to be created or modified.
2.  **Developer Action:** In the `SMS.UI` project, create or modify the corresponding Razor component.
3.  **Developer Action:** Implement the component's markup, styling, and C# logic.

### 3.2. API Development Workflow

This workflow describes how new API endpoints are added to the microservices.

1.  **Developer Action:** Define the required Data Transfer Objects (DTOs) and their validation rules in the `SMS.Contracts` project. Refer to [API Contracts and Data Transfer Objects (DTOs)](./API_Contracts.md) for detailed specifications.
2.  **Developer Action:** Create a new Minimal API endpoint class within the relevant microservice project. This class should implement the `IEndpoint` interface (defined in `SMS.ServiceDefaults`).
3.  **Developer Action:** Implement the endpoint logic within the `MapEndpoints` method of the `IEndpoint` implementation. This includes input validation (using validators from `SMS.Contracts`), business logic execution, and data access.
    -   For simple CRUD operations, Entity Framework Core will be used via the microservice's dedicated `DbContext`.
    -   For complex queries, reporting, or performance-critical reads, Dapper will be used directly to execute raw SQL queries.
4.  **Developer Action:** Add unit and integration tests for the new endpoint in the `SMS.Tests` project.

### 3.7. Assignment & Submission Workflows

#### 3.7.1. Creating and Posting Assignments (Teacher)

1.  **Teacher Action:** Navigates to the "Assignments" section in the `SMS.UI` application.
2.  **System Response:** The `SMS.UI` application renders the appropriate form, using UI components and potentially DTOs from `SMS.Contracts` for data binding.
3.  **Teacher Action:** Fills out the form and clicks "Submit".
4.  **`SMS.UI`:** Sends a request to the API Gateway using a DTO (e.g., `CreateAssignmentRequest`) from `SMS.Contracts`.
5.  **API Gateway:** Routes the request to the appropriate microservice (e.g., School Core Service).
6.  **Microservice (e.g., School Core Service):** Validates the request (using validators from `SMS.Contracts`), uses its `DbContext` (via EF Core) to create the assignment, and returns a success response (e.g., `AssignmentResponse`) from `SMS.Contracts`.
7.  **`SMS.UI`:** Displays a success message.

### 3.8. Messaging, Announcements, and Notifications

- All messaging, announcement, and notification logic is handled by the respective microservices. The `SMS.UI` application is responsible for displaying the data, using DTOs from `SMS.Contracts` for data transfer. Refer to [API Contracts and Data Transfer Objects (DTOs)](./API_Contracts.md) for detailed DTO specifications.

## 4. Error Handling & Validation

- **Client-Side Validation:** Basic validation is performed in the `SMS.UI` application to provide immediate feedback to the user.
- **Server-Side Validation:** Each microservice performs comprehensive validation on all incoming requests, using FluentValidation rules defined on DTOs in `SMS.Contracts`.

## 5. Microservices Transition Workflow

This section outlines the step-by-step process for transitioning the SMS from its current layered architecture to a microservices-based architecture, focusing on the four defined services: User, Notification & Messaging, File Management, and School Core.

### 5.1. Phase 1: Preparation and Planning

1.  **Define Service Boundaries:** Clearly define the responsibilities, entities, and APIs for each of the four microservices. This has been conceptually outlined in the System Design Document.
2.  **Identify Shared Components:** Determine what code (e.g., common utilities, cross-cutting concerns) can be shared or needs to be duplicated/adapted for each service.
3.  **Set Up Infrastructure:** Plan for API Gateway, Service Discovery, Centralized Logging, Distributed Tracing, and Monitoring solutions.
4.  **Choose Communication Patterns:** Decide on synchronous (REST) and asynchronous (message broker) communication strategies.
5.  **Data Migration Strategy:** Plan how data will be migrated from the monolithic database to individual service databases.

### 5.2. Phase 2: Incremental Decomposition

This phase involves gradually extracting functionalities from the `SMS.ApiService` into new microservices.

1.  **Start with a Low-Risk Service:** Begin by extracting a service that has minimal dependencies on others (e.g., File Management Service or Notification & Messaging Service).
2.  **Create New Microservice Project:** For each new microservice (e.g., `SMS.Microservices.User`), create a new ASP.NET Core Web API project.
    -   **Required NuGet Packages (General):**
        -   `Microsoft.AspNetCore.Mvc.NewtonsoftJson` (for JSON handling)
        -   `Microsoft.EntityFrameworkCore` (for data access)
        -   `Npgsql.EntityFrameworkCore.PostgreSQL` (for PostgreSQL database)
        -   `FluentValidation.AspNetCore` (for DTO validation)
        -   `Serilog.AspNetCore` (for logging)
        -   `OpenTelemetry.Exporter.Jaeger` (for distributed tracing)
        -   `OpenTelemetry.Extensions.Hosting`
        -   `Dapper` (for complex queries in School Core Service)
        -   `MassTransit.RabbitMQ` (if using RabbitMQ for async communication)

3.  **Migrate Code:** Move relevant controllers, business logic, and data access code from `SMS.ApiService` and `SMS.Data` to the new microservice project.
4.  **Define Service-Specific Contracts:** Create or adapt DTOs and validators in `SMS.Contracts` (or a new service-specific contracts project) for the new microservice.
5.  **Implement Data Access:** Configure the `DbContext` and migrations for the microservice's dedicated database.
6.  **Implement API Endpoints:** Expose the microservice's functionalities via Minimal API endpoints, implementing the `IEndpoint` interface for dynamic registration.
7.  **Update `SMS.UI` and `SMS.ApiService`:** Modify the existing `SMS.UI` to call the new microservice via the API Gateway. If `SMS.ApiService` still needs to interact with the extracted service, update its code to use the new microservice's API.
8.  **Implement API Gateway Routing:** Configure the API Gateway to route requests to the new microservice.
9.  **Deploy and Test:** Deploy the new microservice and thoroughly test its functionality in isolation and as part of the integrated system.
10. **Repeat:** Continue this process for each microservice until the monolithic API is fully decomposed.

### 5.3. Phase 3: Cross-Cutting Concerns and Advanced Features

1.  **Implement Centralized Logging:** Configure Serilog in each microservice to send logs to a central log management system.
2.  **Implement Distributed Tracing:** Integrate OpenTelemetry into each microservice to enable end-to-end request tracing.
3.  **Implement Service Discovery:** Configure services to register themselves with a service discovery mechanism.
4.  **Implement API Gateway Advanced Features:** Add rate limiting, caching, and other features to the API Gateway.
5.  **Implement Secure Communication (mTLS):** Configure mTLS for internal service-to-service communication.
6.  **Implement Event-Driven Architecture (EDA):** For complex workflows, use the message broker for asynchronous communication and eventual consistency.

### 5.4. Security Considerations during Transition

-   **Authentication:** Ensure seamless authentication flow from `SMS.UI` through the API Gateway to microservices.
-   **Authorization:** Implement granular authorization checks within each microservice based on roles and claims.
-   **Secrets Management:** Securely manage API keys, database connection strings, and other sensitive information for each microservice.

---

This detailed workflow provides a practical guide for evolving the SMS into a robust and scalable microservices architecture.
