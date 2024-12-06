# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Definir el directorio de trabajo en la imagen
WORKDIR /app

# Copiar el archivo .csproj y restaurar dependencias (el archivo .csproj debe estar en la raíz del proyecto)
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código fuente del proyecto
COPY . ./

# Compilar el proyecto
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Imagen para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Establecer el directorio de trabajo en la imagen base
WORKDIR /app

# Copiar el archivo publicado desde la etapa de construcción
COPY --from=build /app/publish .

# Exponer el puerto de la aplicación
EXPOSE 80

# Establecer el punto de entrada de la aplicación (comando para ejecutar la aplicación)
ENTRYPOINT ["dotnet", "PruebaSisteCredito.dll"]
