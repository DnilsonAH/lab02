# Etapa 1: Construcción (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos el csproj manteniendo la estructura de carpetas para poder restaurar dependencias
# Nota: El primer argumento es la ruta en tu PC (desde la raíz), el segundo es la ruta dentro de Docker
COPY ["WebAplicationLab2/WebAplicationLab2.csproj", "WebAplicationLab2/"]

# Restauramos las dependencias (descarga de paquetes NuGet)
RUN dotnet restore "WebAplicationLab2/WebAplicationLab2.csproj"

# Copiamos todo el resto del código fuente desde la raíz
COPY . .

# Cambiamos al directorio del proyecto para compilar
WORKDIR "/src/WebAplicationLab2"

# Compilamos y publicamos en modo Release
RUN dotnet build "WebAplicationLab2.csproj" -c Release -o /app/build
RUN dotnet publish "WebAplicationLab2.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime (Imagen final ligera para producción)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponemos el puerto 8080 (Estándar para contenedores .NET modernos)
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

# Punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "WebAplicationLab2.dll"]