#!/bin/bash

# Caminho da pasta no sistema Linux
source_folder="/root/Migrations"

# Verifica se a pasta existe
if [ ! -d "$source_folder" ]; then
  echo "A pasta $source_folder de backup não existe no linux. A pasta deve sempre ser a atualizada. O script será encerrado."
  exit 1
fi

# Caminho dentro do contêiner
container_name="container-maislocacoes"
container_folder="/app/Migrations"

# Copia a pasta para dentro do contêiner no caminho /app
docker cp "$source_folder" "$container_name":"$container_folder"

# Obtenha a data/hora UTC atual no formato YYYYMMDD_HHMMSS
current_datetime=$(date -u +"%Y%m%d_%H%M%S")

# Nomes dinâmicos para as migrações
migration_name="Migration_${current_datetime}"

# Roda as migrações para os bancos de dados desejados
dotnet ef migrations add ${migration_name} -c DataBaseContextAdm;
dotnet ef migrations add ${migration_name} -c DataBaseContext1;
dotnet ef migrations add ${migration_name} -c DataBaseContext2;
dotnet ef migrations add ${migration_name} -c DataBaseContext3;

# Atualiza todos os bancos de dados
dotnet ef database update -c DataBaseContextAdm;
dotnet ef database update -c DataBaseContext1;
dotnet ef database update -c DataBaseContext2;
dotnet ef database update -c DataBaseContext3;

# Copia a pasta da migration atualizada de volta pro linux para backup:
# Salva com nome temporatio
mv "$container_folder" "$source_folder"_temp
# Deleta a pasta antiga
rm -rf "$source_folder"
# Renomeia a de nome temporário para o nome Migrations
mv "$source_folder"_temp "$source_folder"