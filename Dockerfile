FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DungeonGenerator2.csproj", "./"]
RUN dotnet restore "DungeonGenerator2.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "DungeonGenerator2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DungeonGenerator2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DungeonGenerator2.dll"]
