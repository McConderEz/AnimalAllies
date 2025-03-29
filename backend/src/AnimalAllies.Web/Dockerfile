# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

 
WORKDIR /src

RUN --mount=type=secret,id=nuget_username \
    --mount=type=secret,id=nuget_password \
    dotnet nuget add source "https://gitlab.com/api/v4/projects/63188031/packages/nuget/index.json" \
    --name "gitlab-mcconder" \
    --username "$(cat /run/secrets/nuget_username)" \
    --password "$(cat /run/secrets/nuget_password)" \
    --store-password-in-clear-text

# Копируем файлы проектов
COPY ["src/AnimalAllies.Web/AnimalAllies.Web.csproj", "src/AnimalAllies.Web/"]
COPY ["src/Accounts/AnimalAllies.Accounts.Infrastructure/AnimalAllies.Accounts.Infrastructure.csproj", "src/Accounts/AnimalAllies.Accounts.Infrastructure/"]
COPY ["src/Shared/AnimalAllies.Framework/AnimalAllies.Framework.csproj", "src/Shared/AnimalAllies.Framework/"]
COPY ["src/Accounts/AnimalAllies.Accounts.Contracts/AnimalAllies.Accounts.Contracts.csproj", "src/Accounts/AnimalAllies.Accounts.Contracts/"]
COPY ["src/Shared/AnimalAllies.Core/AnimalAllies.Core.csproj", "src/Shared/AnimalAllies.Core/"]
COPY ["src/Shared/AnimalAllies.SharedKernel/AnimalAllies.SharedKernel.csproj", "src/Shared/AnimalAllies.SharedKernel/"]
COPY ["src/Accounts/AnimalAllies.Accounts.Domain/AnimalAllies.Accounts.Domain.csproj", "src/Accounts/AnimalAllies.Accounts.Domain/"]
COPY ["src/Accounts/AnimalAllies.Accounts.Application/AnimalAllies.Accounts.Application.csproj", "src/Accounts/AnimalAllies.Accounts.Application/"]
COPY ["src/VolunteerRequests/VolunteerRequests.Contracts/VolunteerRequests.Contracts.csproj", "src/VolunteerRequests/VolunteerRequests.Contracts/"]
COPY ["src/Accounts/AnimalAllies.Accounts.Presentation/AnimalAllies.Accounts.Presentation.csproj", "src/Accounts/AnimalAllies.Accounts.Presentation/"]
COPY ["src/BreedManagement/AnimalAllies.Species.Controllers/AnimalAllies.Species.Presentation.csproj", "src/BreedManagement/AnimalAllies.Species.Controllers/"]
COPY ["src/BreedManagement/AnimalAllies.Species.Application/AnimalAllies.Species.Application.csproj", "src/BreedManagement/AnimalAllies.Species.Application/"]
COPY ["src/PetManagement/AnimalAllies.Volunteer.Contracts/AnimalAllies.Volunteer.Contracts.csproj", "src/PetManagement/AnimalAllies.Volunteer.Contracts/"]
COPY ["src/BreedManagement/AnimalAllies.Species.Domain/AnimalAllies.Species.Domain.csproj", "src/BreedManagement/AnimalAllies.Species.Domain/"]
COPY ["src/BreedManagement/AnimalAllies.Species.Contracts/AnimalAllies.Species.Contracts.csproj", "src/BreedManagement/AnimalAllies.Species.Contracts/"]
COPY ["src/BreedManagement/AnimalAllies.Species.Infrastructure/AnimalAllies.Species.Infrastructure.csproj", "src/BreedManagement/AnimalAllies.Species.Infrastructure/"]
COPY ["src/Discussion/Discussion.Application/Discussion.Application.csproj", "src/Discussion/Discussion.Application/"]
COPY ["src/Discussion/Discussion.Domain/Discussion.Domain.csproj", "src/Discussion/Discussion.Domain/"]
COPY ["src/Discussion/Discussion.Infrastructure/Discussion.Infrastructure.csproj", "src/Discussion/Discussion.Infrastructure/"]
COPY ["src/Discussion/Discussion.Presentation/Discussion.Presentation.csproj", "src/Discussion/Discussion.Presentation/"]
COPY ["src/Discussion/Discussion.Contracts/Discussion.Contracts.csproj", "src/Discussion/Discussion.Contracts/"]
COPY ["src/PetManagement/AnimalAllies.Volunteer.Controllers/AnimalAllies.Volunteer.Presentation.csproj", "src/PetManagement/AnimalAllies.Volunteer.Controllers/"]
COPY ["src/PetManagement/AnimalAllies.Volunteer.Application/AnimalAllies.Volunteer.Application.csproj", "src/PetManagement/AnimalAllies.Volunteer.Application/"]
COPY ["src/PetManagement/AnimalAllies.Volunteer.Domain/AnimalAllies.Volunteer.Domain.csproj", "src/PetManagement/AnimalAllies.Volunteer.Domain/"]
COPY ["src/PetManagement/AnimalAllies.Volunteer.Infrastructure/AnimalAllies.Volunteer.Infrastructure.csproj", "src/PetManagement/AnimalAllies.Volunteer.Infrastructure/"]
COPY ["src/VolunteerRequests/VolunteerRequests.Application/VolunteerRequests.Application.csproj", "src/VolunteerRequests/VolunteerRequests.Application/"]
COPY ["src/VolunteerRequests/VolunteerRequests.Domain/VolunteerRequests.Domain.csproj", "src/VolunteerRequests/VolunteerRequests.Domain/"]
COPY ["src/VolunteerRequests/VolunteerRequests.Infrastructure/VolunteerRequests.Infrastructure.csproj", "src/VolunteerRequests/VolunteerRequests.Infrastructure/"]
COPY ["src/VolunteerRequests/VolunteerRequests.Presentation/VolunteerRequests.Presentation.csproj", "src/VolunteerRequests/VolunteerRequests.Presentation/"]

# Восстанавливаем зависимости
RUN dotnet restore "src/AnimalAllies.Web/AnimalAllies.Web.csproj"

# Копируем остальные файлы и собираем
COPY . .
WORKDIR "/src/src/AnimalAllies.Web"
RUN dotnet build "AnimalAllies.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем приложение
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AnimalAllies.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnimalAllies.Web.dll"]