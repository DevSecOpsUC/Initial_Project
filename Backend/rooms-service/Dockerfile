# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia sólo los archivos de proyecto primero (mejor cache)
COPY *.csproj ./
# Si tu .csproj está en un subdirectorio o tiene otro nombre, se ajusta abajo.
RUN dotnet restore

# Copia todo el código y publica
COPY . .
# publica la(s) dll(s) en /app/publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80

# Copia los artefactos publicados
COPY --from=build /app/publish ./

# Listar archivos para depuración (opcional - elimina en prod)
# RUN ls -la /app

# Intenta ejecutar el primer *.dll que encuentre (establecer el nombre si lo conoces)
# Si conoces el nombre final, reemplaza `rooms-service.dll` por el nombre real.
ENTRYPOINT ["dotnet", "rooms-service.dll"]
