# .NET 8 Sidecar Pattern Implementation

This project demonstrates the **Sidecar Pattern** using two .NET 8 Web APIs. The Sidecar pattern is an architectural pattern where a helper process is deployed alongside a primary application to handle cross-cutting concerns.

## Architecture

1.  **MainApp**: A primary Web API that performs business logic. It offloads logging/telemetry tasks to the sidecar via HTTP calls.
2.  **SidecarApp**: A secondary service that acts as a logging agent. In a real scenario, this service would aggregate logs, add infrastructure metadata, and forward them to a centralized store (e.g., ELK stack).

In a production Kubernetes environment, these two containers would reside in the same **Pod** and share the same network stack (`localhost`). In this demo, they communicate over a Docker bridge network using service names.

## Prerequisites

-   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Getting Started

1.  **Clone or Download** the project files into a single directory.
2.  Open a terminal in the root directory (where `docker-compose.yml` is located).
3.  **Run the application**:
    `docker-compose up --build`

## Usage

Once the containers are running:

1.  **Main Application Swagger**: Access `http://localhost:8080/swagger`
2.  **Sidecar Application Swagger**: Access `http://localhost:5001/swagger`

### Testing the Pattern

1.  Open the MainApp Swagger UI.
2.  Locate the `POST /api/Business/process` endpoint.
3.  Execute the request with a JSON body like:
    