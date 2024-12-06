# Prueba SisteCredito

## Descripción
Este sistema gestiona horas extras de empleados, homologación de áreas y relaciones jerárquicas. Incluye flujos de aprobación y registro de auditorías.

## Arquitectura
El sistema utiliza arquitectura hexagonal:
- **Application**: Maneja controladores y casos de uso.
- **Domain**: Contiene entidades y lógica de negocio.
- **Infrastructure**: Maneja bases de datos y servicios externos.

## Tecnologías Utilizadas
- .NET 9 para API Restful. (dotnet add package Swashbuckle.AspNetCore)
- SQL Server para datos relacionales.
- MongoDB para auditorías.  (dotnet add package MongoDB.Driver)
- JWT para autenticación.  (dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer)
- EPPlus leer archivos excel. (dotnet add package EPPlus)
- XUnit pruebas unitarias. (dotnet add package xunit  - dotnet add package Moq)
## Requisitos
- Docker
- .NET SDK 9
- SQL Server

## Despliegue Local
1. Descomprimir el repositorio:
2. Levantar servicios con Docker:
   - docker-compose up --build
3.  Acceder a la API
    - Base URL: http://localhost:5000
    - Swagger: http://localhost:5000/swagger
4. Importar en Postman los archivos 
   - PruebaSisteCredito.postman_collection.json
   - PruebasSisteCredito.postman_environment.json


## Despliegue Infraestructua
Se usa docker.

::: mermaid
architecture-beta
    group api(cloud)[API]

    service SQLServer(database)[PruebasSisteCredito] in api
    service Mongo(database)[Mongo] in api
    service server(server)[Server] in api

    SQLServer:L -- R:server
    Mongo:T -- B:server
:::