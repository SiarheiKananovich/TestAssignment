FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY TvMazeScraperMicroservice/TvMazeScraper.Host/TvMazeScraper.Host.csproj TvMazeScraperMicroservice/TvMazeScraper.Host/
COPY TvMazeScraperMicroservice/TvMazeScraper.Database/TvMazeScraper.Database.csproj TvMazeScraperMicroservice/TvMazeScraper.Database/
COPY TvMazeScraperMicroservice/TvMazeScraper.Database.Interface/TvMazeScraper.Database.Interface.csproj TvMazeScraperMicroservice/TvMazeScraper.Database.Interface/
COPY TvMazeScraperMicroservice/TvMazeScraper.BusinessLogic.Interface/TvMazeScraper.BusinessLogic.Interface.csproj TvMazeScraperMicroservice/TvMazeScraper.BusinessLogic.Interface/
COPY TvMazeScraperMicroservice/TvMazeScraper.BusinessLogic/TvMazeScraper.BusinessLogic.csproj TvMazeScraperMicroservice/TvMazeScraper.BusinessLogic/
RUN dotnet restore TvMazeScraperMicroservice/TvMazeScraper.Host/TvMazeScraper.Host.csproj
COPY . .
WORKDIR /src/TvMazeScraperMicroservice/TvMazeScraper.Host
RUN dotnet build TvMazeScraper.Host.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TvMazeScraper.Host.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TvMazeScraper.Host.dll"]
