FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SuperRemont/SuperRemont.csproj", "SuperRemont/"]
RUN dotnet restore "SuperRemont/SuperRemont.csproj"
COPY . .
WORKDIR "/src/SuperRemont"
RUN dotnet build "./SuperRemont.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SuperRemont.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS="http://*:80"
ENTRYPOINT ["dotnet", "SuperRemont.dll"]    
