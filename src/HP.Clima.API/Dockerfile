FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files for dependency resolution
COPY ["src/HP.Clima.API/HP.Clima.API.csproj", "src/HP.Clima.API/"]
COPY ["src/HP.Clima.Domain/HP.Clima.Domain.csproj", "src/HP.Clima.Domain/"]
COPY ["src/HP.Clima.Infra/HP.Clima.Infra.csproj", "src/HP.Clima.Infra/"]
COPY ["src/HP.Clima.Service/HP.Clima.Service.csproj", "src/HP.Clima.Service/"]
COPY ["test/integration/HP.Clima.Test.Integration/HP.Clima.Test.Integration.csproj", "test/integration/HP.Clima.Test.Integration/"]
COPY ["test/unit/HP.Clima.Test.Integration/HP.Clima.Test.Integration.csproj", "test/unit/HP.Clima.Test.Integration/"]

# Restore all dependencies
RUN dotnet restore "src/HP.Clima.API/HP.Clima.API.csproj"

# Copy all source code
COPY . .

# Build all projects
WORKDIR "/src/src/HP.Clima.API"
RUN dotnet build "./HP.Clima.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Test stage - run unit and integration tests
FROM build AS test
WORKDIR /src
RUN dotnet test "test/unit/HP.Clima.Test.Integration/HP.Clima.Test.Integration.csproj" -c $BUILD_CONFIGURATION --no-build --verbosity normal
RUN dotnet test "test/integration/HP.Clima.Test.Integration/HP.Clima.Test.Integration.csproj" -c $BUILD_CONFIGURATION --no-build --verbosity normal

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/HP.Clima.API"
RUN dotnet publish "./HP.Clima.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HP.Clima.API.dll"]
