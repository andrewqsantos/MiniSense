FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MiniSense.API/MiniSense.API.csproj", "src/MiniSense.API/"]
COPY ["src/MiniSense.Application/MiniSense.Application.csproj", "src/MiniSense.Application/"]
COPY ["src/MiniSense.Domain/MiniSense.Domain.csproj", "src/MiniSense.Domain/"]
COPY ["src/MiniSense.Infrastructure/MiniSense.Infrastructure.csproj", "src/MiniSense.Infrastructure/"]
RUN dotnet restore "src/MiniSense.API/MiniSense.API.csproj"
COPY . .
WORKDIR "/src/src/MiniSense.API"
RUN dotnet build "./MiniSense.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MiniSense.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniSense.API.dll"]
