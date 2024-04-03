FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EOSC.Bot.csproj", "./"]
RUN dotnet restore --force "EOSC.Bot.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "EOSC.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build
ENV DISCORD_TOKEN=${DISCORD_TOKEN}

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EOSC.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EOSC.Bot.dll"]