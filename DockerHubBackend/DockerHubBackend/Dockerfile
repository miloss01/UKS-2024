# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Install EF Core CLI tools to run migrations
RUN dotnet tool install --global dotnet-ef

# Copy the rest of the app's source code and build it
COPY . ./
RUN dotnet publish -c Release -o /out

# Use the runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /out .

# Create the Logs directory
RUN mkdir -p /app/../../../Logs

# Expose the port on which the app runs (adjust based on your app's settings)
EXPOSE 5156

# Start the application
ENTRYPOINT ["dotnet", "DockerHubBackend.dll"]

CMD ["dotnet", "ef", "database", "update", "--no-build", "--project", "DockerHubBackend/DockerHubBackend.csproj", "--startup-project", "DockerHubBackend/DockerHubBackend.csproj"]


