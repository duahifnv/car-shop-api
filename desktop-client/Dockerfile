# Используем базовый образ для .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем и восстанавливаем проект
COPY desktop-client/desktop-client.csproj desktop-client/
RUN dotnet restore desktop-client/desktop-client.csproj

# Копируем всё остальное и собираем приложение
COPY . .
WORKDIR /src/desktop-client
RUN dotnet publish -c Release -o /app

# Финальный образ
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "desktop-client.dll"]