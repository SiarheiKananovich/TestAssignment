FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ApiServer/ApiServer.csproj ApiServer/
COPY BusinessLogic/BusinessLogic.csproj BusinessLogic/
COPY BusinessLogic.Interface/BusinessLogic.Interface.csproj BusinessLogic.Interface/
COPY Database/Database.csproj Database/
COPY Database.Interface/Database.Interface.csproj Database.Interface/
RUN dotnet restore ApiServer/ApiServer.csproj
COPY . .
WORKDIR /src/ApiServer
RUN dotnet build ApiServer.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiServer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiServer.dll"]
