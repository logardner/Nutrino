# Build context is the repo root (Nutrino folder) — matches DockerfileContext in Nutrino.Web.csproj.

# ---- base: runtime image (Visual Studio's Fast debug mode uses only this stage) ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ---- build: restore and compile with the .NET 10 SDK ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["web/src/Nutrino.Web/Nutrino.Web.csproj", "web/src/Nutrino.Web/"]
RUN dotnet restore "./web/src/Nutrino.Web/Nutrino.Web.csproj"
COPY . .
WORKDIR "/src/web/src/Nutrino.Web"
RUN dotnet build "./Nutrino.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---- publish ----
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Nutrino.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---- final: what runs in production / docker compose ----
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nutrino.Web.dll"]
