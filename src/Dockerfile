FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY MaisLocacoes.WebApi/*.csproj ./MaisLocacoes.WebApi/
COPY MaisLocacoes.WebApi/. ./MaisLocacoes.WebApi/
COPY entrypoint.sh /usr/local/bin/entrypoint.sh
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