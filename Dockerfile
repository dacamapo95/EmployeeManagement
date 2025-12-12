# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Directory.Build.props", "./"]
COPY ["PropertyManagement.API/EmployeeManagement.API.csproj", "PropertyManagement.API/"]
COPY ["PropertyManagement.Application/EmployeeManagement.Application.csproj", "PropertyManagement.Application/"]
COPY ["PropertyManagement.Domain/EmployeeManagement.Domain.csproj", "PropertyManagement.Domain/"]
COPY ["PropertyManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj", "PropertyManagement.Infrastructure/"]
COPY ["PropertyManagement.Shared/EmployeeManagement.Shared.csproj", "PropertyManagement.Shared/"]

RUN dotnet restore "PropertyManagement.API/EmployeeManagement.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/PropertyManagement.API"
RUN dotnet build "EmployeeManagement.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "EmployeeManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]
