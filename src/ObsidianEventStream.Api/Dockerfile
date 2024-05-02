FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ObsidianEventStream.Api/ObsidianEventStream.Api.csproj", "ObsidianEventStream.Api/"]
RUN dotnet restore "ObsidianEventStream.Api/ObsidianEventStream.Api.csproj"
COPY . .
WORKDIR "/src/ObsidianEventStream.Api"
RUN dotnet build "ObsidianEventStream.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ObsidianEventStream.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ObsidianEventStream.Api.dll"]
