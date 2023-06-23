FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY MaisLocacoes.WebApi/appsettingsDocker.json ./MaisLocacoes.WebApi/appsettings.json
COPY MaisLocacoes.WebApi/*.csproj ./MaisLocacoes.WebApi/
COPY MaisLocacoes.WebApi/1Controllers ./MaisLocacoes.WebApi/1Controllers
COPY MaisLocacoes.WebApi/2Service ./MaisLocacoes.WebApi/2Service
COPY MaisLocacoes.WebApi/3Repository ./MaisLocacoes.WebApi/3Repository
COPY MaisLocacoes.WebApi/Configuration ./MaisLocacoes.WebApi/Configuration
COPY MaisLocacoes.WebApi/DataBase ./MaisLocacoes.WebApi/DataBase
COPY MaisLocacoes.WebApi/Domain ./MaisLocacoes.WebApi/Domain
COPY MaisLocacoes.WebApi/Exceptions ./MaisLocacoes.WebApi/Exceptions
COPY MaisLocacoes.WebApi/Infrastructure ./MaisLocacoes.WebApi/Infrastructure
COPY MaisLocacoes.WebApi/Properties ./MaisLocacoes.WebApi/Properties
COPY MaisLocacoes.WebApi/Utils ./MaisLocacoes.WebApi/Utils
COPY MaisLocacoes.WebApi/Program.cs ./MaisLocacoes.WebApi/Program.cs
COPY MaisLocacoes.WebApi/libman.json ./MaisLocacoes.WebApi/libman.json
COPY entrypoint.sh /usr/local/bin/entrypoint.sh
COPY MaisLocacoes.WebApi.sln MaisLocacoes.WebApi.sln

RUN chmod +x /usr/local/bin/entrypoint.sh

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY MaisLocacoes.WebApi/*.csproj ./MaisLocacoes.WebApi/

RUN dotnet restore "MaisLocacoes.WebApi/MaisLocacoes.WebApi.csproj"

COPY MaisLocacoes.WebApi/. ./MaisLocacoes.WebApi/
WORKDIR "/src/MaisLocacoes.WebApi"

RUN dotnet build "MaisLocacoes.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MaisLocacoes.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["/usr/local/bin/entrypoint.sh", "dotnet", "MaisLocacoes.WebApi.dll"]