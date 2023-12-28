#!/bin/bash
docker exec -it maislocacoes bash

cd MaisLocacoes.WebApi

echo "Adicionando migrações..."

# Obter a data UTC atual no formato YYYYMMDDHHMMSS
migration_name=$(date -u +"%Y%m%d%H%M%S")

dotnet ef migrations add $migration_name -c DataBaseContextAdm;
dotnet ef migrations add $migration_name -c DataBaseContext1;
dotnet ef migrations add $migration_name -c DataBaseContext2;
dotnet ef migrations add $migration_name -c DataBaseContext3;

dotnet ef database update -c DataBaseContextAdm;
dotnet ef database update -c DataBaseContext1;
dotnet ef database update -c DataBaseContext2;
dotnet ef database update -c DataBaseContext3;
