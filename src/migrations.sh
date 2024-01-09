#!/bin/bash

# Obtenha a data/hora UTC atual no formato YYYYMMDD_HHMMSS
current_datetime=$(date -u +"%Y%m%d_%H%M%S")

# Nomes dinâmicos para as migrações
migration_name="Migration_${current_datetime}"

# Roda as migrações para os bancos de dados desejados
dotnet ef migrations add ${migration_name} -c DataBaseContextAdm;
dotnet ef migrations add ${migration_name} -c DataBaseContext1;
dotnet ef migrations add ${migration_name} -c DataBaseContext2;
dotnet ef migrations add ${migration_name} -c DataBaseContext3;

# Atualiza os bancos de dados
dotnet ef database update -c DataBaseContextAdm;
dotnet ef database update -c DataBaseContext1;
dotnet ef database update -c DataBaseContext2;
dotnet ef database update -c DataBaseContext3;