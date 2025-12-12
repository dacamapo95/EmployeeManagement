# Employee Management - Docker Setup

Esta guía explica cómo ejecutar la aplicación Employee Management usando Docker y Docker Compose.

## Requisitos previos

- Docker Desktop instalado y ejecutándose
- Docker Compose (incluido en Docker Desktop)

## Arquitectura

El stack de Docker incluye:
- **API**: Aplicación ASP.NET Core 8.0 (puerto 5000)
- **SQL Server**: Microsoft SQL Server 2022 Developer Edition (puerto 1433)

## Configuración inicial

### 1. Configurar variables de entorno (Opcional)

Copia el archivo de ejemplo `.env.example` a `.env` y modifica los valores si es necesario:

```bash
cp .env.example .env
```

Variables disponibles:
- `DB_SA_PASSWORD`: Contraseña del usuario SA de SQL Server (por defecto: `YourStrong@Passw0rd`)
- `ASPNETCORE_ENVIRONMENT`: Entorno de ejecución (por defecto: `Development`)

**Nota de seguridad**: La contraseña debe cumplir con la política de SQL Server:
- Mínimo 8 caracteres
- Debe contener mayúsculas, minúsculas, números y caracteres especiales

### 2. Construir y ejecutar los contenedores

```bash
docker-compose up --build
```

Este comando:
1. Construye la imagen de la API
2. Descarga la imagen de SQL Server
3. Crea una red para la comunicación entre servicios
4. Inicia SQL Server y espera a que esté disponible (healthcheck)
5. Inicia la API y ejecuta las migraciones de base de datos
6. Siembra datos iniciales (Companies y Portals)

### 3. Acceder a la aplicación

Una vez que los contenedores estén ejecutándose:

- **Swagger UI**: http://localhost:5000/swagger
- **API**: http://localhost:5000
- **SQL Server**: localhost:1433
  - Usuario: `sa`
  - Contraseña: La configurada en `.env` o `YourStrong@Passw0rd`

## Comandos útiles

### Detener los contenedores
```bash
docker-compose down
```

### Detener y eliminar volúmenes (elimina la base de datos)
```bash
docker-compose down -v
```

### Ver logs
```bash
# Todos los servicios
docker-compose logs -f

# Solo la API
docker-compose logs -f api

# Solo SQL Server
docker-compose logs -f sqlserver
```

### Reconstruir la API sin caché
```bash
docker-compose build --no-cache api
docker-compose up api
```

### Ejecutar en segundo plano (detached mode)
```bash
docker-compose up -d
```

### Ver estado de los contenedores
```bash
docker-compose ps
```

## Solución de problemas

### La API no se conecta a SQL Server

1. Verifica que SQL Server esté saludable:
   ```bash
   docker-compose ps
   ```
   El estado de `sqlserver` debe ser "healthy"

2. Verifica los logs de SQL Server:
   ```bash
   docker-compose logs sqlserver
   ```

3. Verifica la cadena de conexión en los logs de la API:
   ```bash
   docker-compose logs api
   ```

### Error de migraciones de base de datos

Si las migraciones fallan, puedes ejecutarlas manualmente:

```bash
# Entrar al contenedor de la API
docker exec -it employeemanagement-api bash

# Ejecutar las migraciones (si tienes dotnet-ef instalado)
dotnet ef database update
```

### Cambiar la contraseña de SQL Server

1. Detén los contenedores:
   ```bash
   docker-compose down -v
   ```

2. Edita el archivo `.env` y cambia `DB_SA_PASSWORD`

3. Inicia nuevamente:
   ```bash
   docker-compose up --build
   ```

### Puerto 1433 ya en uso

Si tienes SQL Server instalado localmente, puede haber conflicto de puertos. Opciones:

1. Detén tu SQL Server local
2. Cambia el puerto en `docker-compose.yml`:
   ```yaml
   ports:
     - "1434:1433"  # Usa el puerto 1434 en tu máquina
   ```

## Datos iniciales (Seeding)

Al iniciar por primera vez, la aplicación siembra automáticamente:

- **5 Compañías**:
  - Acme Corporation
  - TechStart Inc
  - Global Solutions Ltd
  - Innovation Labs
  - Digital Dynamics

- **8 Portales** distribuidos entre las compañías

Estos datos están disponibles a través de los endpoints:
- `GET /companies` - Lista todas las compañías
- `GET /portals?companyId={id}` - Lista portales de una compañía

## Estructura de archivos Docker

```
EmployeeManagement/
├── Dockerfile                    # Construcción de la imagen de la API
├── docker-compose.yml            # Orquestación de servicios
├── .dockerignore                 # Archivos excluidos del build
├── .env.example                  # Ejemplo de variables de entorno
├── .env                          # Variables de entorno (no en git)
└── README.Docker.md              # Esta documentación
```

## Persistencia de datos

Los datos de SQL Server se almacenan en un volumen Docker llamado `sqlserver_data`. Este volumen persiste incluso si detienes los contenedores.

Para eliminar completamente los datos:
```bash
docker-compose down -v
```

## Producción

Para usar en producción:

1. Cambia `ASPNETCORE_ENVIRONMENT` a `Production`
2. Configura una contraseña segura para SQL Server
3. Considera usar secretos de Docker en lugar de variables de entorno
4. Configura un servidor SQL Server dedicado en lugar del contenedor
5. Implementa backups regulares de la base de datos
6. Configura HTTPS con certificados válidos
7. Usa una red privada para la comunicación entre servicios

## Soporte

Si encuentras problemas, revisa:
1. Los logs de Docker: `docker-compose logs`
2. El estado de los contenedores: `docker-compose ps`
3. La configuración de red: `docker network inspect employeemanagement_employeemanagement-network`
