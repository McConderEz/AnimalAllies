FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AnimalAllies.API/AnimalAllies.API.csproj", "src/AnimalAllies.API/"]
COPY ["src/AnimalAllies.Application/AnimalAllies.Application.csproj", "src/AnimalAllies.Application/"]
COPY ["src/AnimalAllies.Domain/AnimalAllies.Domain.csproj", "src/AnimalAllies.Domain/"]
COPY ["src/AnimalAllies.Infrastructure.Postgres/AnimalAllies.Infrastructure.Postgres.csproj", "src/AnimalAllies.Infrastructure.Postgres/"]
RUN dotnet restore "src/AnimalAllies.API/AnimalAllies.API.csproj"
COPY . .
WORKDIR "/src/src/AnimalAllies.API"
RUN dotnet build "AnimalAllies.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AnimalAllies.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnimalAllies.API.dll"]
