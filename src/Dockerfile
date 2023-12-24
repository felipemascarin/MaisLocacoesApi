FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
COPY ["entrypoint.sh", "/usr/local/bin/entrypoint.sh"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["*.sln", "./"]
COPY ["MaisLocacoes.WebApi/*.csproj", "./MaisLocacoes.WebApi/"]

RUN chmod +x /usr/local/bin/entrypoint.sh

RUN dotnet restore "MaisLocacoes.WebApi/MaisLocacoes.WebApi.csproj"
COPY . ./
WORKDIR "/src/MaisLocacoes.WebApi"
RUN mv appsettingsDocker.json appsettings.json
RUN dotnet build "MaisLocacoes.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MaisLocacoes.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["/usr/local/bin/entrypoint.sh", "dotnet", "MaisLocacoes.WebApi.dll"]