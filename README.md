# Employee Management API

API para gestión de empleados con autenticación OAuth2/OpenID Connect usando Keycloak. Construida con .NET 8, Clean Architecture, CQRS.

## 🚀 Quick Start

### Prerrequisitos
- Docker 
- Git
- .NET 8 Runtime

### Levantar el Proyecto

```bash
# Clonar el repositorio
git clone https://github.com/dacamapo95/EmployeeManagement
cd EmployeeManagement

# Iniciar todos los servicios
docker-compose up --build
```

Espera a que todos los servicios estén listos.

### URLs del Sistema

| Servicio | URL | Credenciales |
|----------|-----|--------------|
| **API** | http://localhost:5000 | - |
| **Swagger** | http://localhost:5000/swagger | - |
| **Keycloak Admin** | http://localhost:8080 | admin / admin |
| **SQL Server** | localhost:1433 | sa / YourStrong@Passw0rd |

## 👥 Usuarios de Prueba

| Usuario | Contraseña | Rol | 
|---------|-----------|-----|
| testuser | test123 | Employee | 
| manager | manager123 | Manager |
| admin | admin123 | Admin |

## 🔐 Autenticación

### 1. Obtener Token (Postman)

**Request:**
```
POST http://localhost:8080/realms/employeemanagement/protocol/openid-connect/token
Content-Type: application/x-www-form-urlencoded

grant_type: password
client_id: postman-client
username: testuser
password: test123
```

**Response:**
```json
{
  "access_token": "eyJhbGci...",
  "expires_in": 3600,
  "refresh_token": "..."
}
```

### 2. Usar el Token

En Postman:
- Authorization → Type: **Bearer Token**
- Token: `<pega_tu_access_token>`

En curl:
```bash
curl -X GET 'http://localhost:5000/api/redarbor' \
  -H "Authorization: Bearer <tu_access_token>"
```

## 📋 Endpoints Principales

### Employees (`/api/redarbor`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/redarbor` | Listar empleados (paginado) |
| GET | `/api/redarbor/{id}` |  Obtener empleado por ID |
| POST | `/api/redarbor` | Crear empleado |
| PUT | `/api/redarbor/{id}` | Actualizar empleado |
| DELETE | `/api/redarbor/{id}` |  Eliminar empleado |

### Authentication (`/api/auth`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/auth/info` | Info de autenticación y usuarios de prueba |

### Companies (`/api/companies`)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/companies` | Listar compañías |
| GET | `/api/companies/{id}/portals` | Listar portales de una compañía |



## 🏗️ Arquitectura

```
EmployeeManagement/
├── PropertyManagement.API/          # Presentación - Carter endpoints
├── PropertyManagement.Application/  # Lógica de negocio - CQRS con MediatR
├── PropertyManagement.Domain/       # Entidades y reglas de negocio
├── PropertyManagement.Infrastructure/ # Datos - EF Core, Dapper , Repositorios
├── PropertyManagement.Shared/       # Primitivas compartidas, Result pattern
└── PropertyManagement.Tests/        # Tests unitarios - NUnit
```

### Tecnologías

- **.NET 8.0** - Framework
- **Entity Framework Core 8** - ORM
- **SQL Server 2022** - Base de datos
- **Keycloak 23** - Autenticación OAuth2/OIDC
- **MediatR** - CQRS pattern
- **Carter** - Minimal APIs
- **FluentValidation** - Validación
- **Serilog** - Logging
- **Docker & Docker Compose** - Contenedores

### Patrones Implementados

- ✅ **Clean Architecture** - Separación de capas
- ✅ **CQRS** - Command Query Responsibility Segregation
- ✅ **Repository Pattern** - Abstracción de datos
- ✅ **Result Pattern** - Manejo de errores sin excepciones
- ✅ **Unit of Work** - Transacciones
- ✅ **OAuth2 + OIDC** - Autenticación y autorización

## 📊 Base de Datos

La base de datos se inicializa automáticamente con:
- 5 Compañías
- 8 Portales
- Schema: `EMY`

### Seguridad
- ✅ Autenticación JWT con Keycloak

### API
- ✅ OpenAPI/Swagger documentation
- ✅ Paginación en listados
- ✅ Filtros de búsqueda
- ✅ Validación con FluentValidation
- ✅ Result pattern para manejo de errores

### Auditoría
- ✅ Campos de auditoría automáticos (CreatedBy, LastModifiedBy)
- ✅ Timestamps UTC
- ✅ Logging estructurado con Serilog

## 📝 Notas

- Los tokens JWT expiran en **1 hora**
- Las contraseñas en este proyecto son **solo para demostración**
- El proyecto incluye datos de prueba pre-cargados
- Keycloak realm se importa automáticamente en el primer inicio

