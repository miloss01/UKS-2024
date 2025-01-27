#!/bin/bash
# Apply migrations
dotnet ef database update --project DockerHubBackend/DockerHubBackend.csproj --startup-project DockerHubBackend/DockerHubBackend.csproj

# Start the application
dotnet DockerHubBackend.dll