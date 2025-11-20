# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RetailPlatform.API/RetailPlatform.API.csproj", "RetailPlatform.API/"]
COPY ["RetailPlatform.Core/RetailPlatform.Core.csproj", "RetailPlatform.Core/"]
COPY ["RetailPlatform.Contracts/RetailPlatform.Contracts.csproj", "RetailPlatform.Contracts/"]
COPY ["RetailPlatform.Domain/RetailPlatform.Domain.csproj", "RetailPlatform.Domain/"]
COPY ["RetailPlatform.Persistence/RetailPlatform.Persistence.csproj", "RetailPlatform.Persistence/"]
RUN dotnet restore "./RetailPlatform.API/RetailPlatform.API.csproj"
COPY . .
WORKDIR "/src/RetailPlatform.API"
RUN dotnet build "./RetailPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RetailPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetailPlatform.API.dll"]