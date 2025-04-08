FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 3000

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

COPY ["src/TelegramBotService/TelegramBotService.csproj", "src/TelegramBotService/"]
COPY ["src/TelegramBotService.Contracts/TelegramBotService.Contracts.csproj", "src/TelegramBotService.Contracts/"]
RUN dotnet restore "src/TelegramBotService/TelegramBotService.csproj"
COPY . .
WORKDIR "/src/src/TelegramBotService"
RUN dotnet build "TelegramBotService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TelegramBotService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBotService.dll"]
