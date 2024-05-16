FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ObsidianEventStream.Console/ObsidianEventStream.Console.csproj", "ObsidianEventStream.Console/"]
RUN dotnet restore "ObsidianEventStream.Console/ObsidianEventStream.Console.csproj"
COPY . .
WORKDIR "/src/ObsidianEventStream.Console"
RUN dotnet build "ObsidianEventStream.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ObsidianEventStream.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ObsidianEventStream.Console.dll"]
