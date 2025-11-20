# ========================================================
# Stage 1: Build Stage (SDK Image)
# ========================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file and all project files
COPY ["HP.Clima.API.sln", "./"]
COPY ["src/HP.Clima.API/HP.Clima.API.csproj", "src/HP.Clima.API/"]
COPY ["src/HP.Clima.Domain/HP.Clima.Domain.csproj", "src/HP.Clima.Domain/"]
COPY ["src/HP.Clima.Infra/HP.Clima.Infra.csproj", "src/HP.Clima.Infra/"]
COPY ["src/HP.Clima.Service/HP.Clima.Service.csproj", "src/HP.Clima.Service/"]

# Restore dependencies (cached layer)
RUN dotnet restore "src/HP.Clima.API/HP.Clima.API.csproj"

# Copy all source code
COPY ["src/", "src/"]

# Build the application
WORKDIR "/src/src/HP.Clima.API"
RUN dotnet build "HP.Clima.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# ========================================================
# Stage 2: Publish Stage
# ========================================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "HP.Clima.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ========================================================
# Stage 3: Runtime Stage (ASP.NET Runtime - Minimal Image)
# ========================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Install curl for healthcheck and ICU libraries for globalization
RUN apk add --no-cache curl icu-libs

# Create non-root user for security
RUN addgroup -g 1000 appuser && \
    adduser -u 1000 -G appuser -s /bin/sh -D appuser

# Copy published files from publish stage
COPY --from=publish --chown=appuser:appuser /app/publish .

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "HP.Clima.API.dll"]
