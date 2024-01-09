#!/bin/bash

# Caminho da pasta no sistema Linux
source_folder="/root/Migrations"

# Caminho dentro do contêiner
container_folder="/app/Migrations"

# Copia a pasta para dentro do contêiner no caminho /app
cp -r "$source_folder" "$container_folder"

# Verifica se a pasta foi copiada com sucesso
if [ $? -eq 0 ]; then
  echo "Pasta Migrations copiada com sucesso para o container $container_folder."
else
  echo "Erro ao copiar a pasta Migrations do diretorio do linux ~/Migrations para o container $container_folder. O script será encerrado."
  exit 1
fi

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

# Copia a pasta da migration atualizada de volta para o linux para backup:
# Salva com nome temporario
cp -r "$container_folder" "$source_folder"_temp
# Deleta a pasta antiga
rm -rf "$source_folder"
# Renomeia a de nome temporário para o nome Migrations
mv "$source_folder"_temp "$source_folder"
