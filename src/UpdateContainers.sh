#!/bin/bash

# Verifica se a imagem postgres já existe localmente
image_name="postgres"
if [[ ! "$(docker images -q $image_name 2> /dev/null)" ]]; then
  echo "Imagem postgres não encontrada. Baixando..."
  docker pull $image_name
else
  echo "Imagem postgres já existe localmente. Pulando o download."
fi

docker pull mcr.microsoft.com/dotnet/sdk:6.0
docker pull mcr.microsoft.com/dotnet/aspnet:6.0

# Limpar recursos não utilizados do Docker
echo "Limpando recursos não utilizados do Docker..."
docker system prune -f

cd ~/MaisLocacoesApi/src
git clean -df
git pull

docker-compose down
docker-compose build
docker-compose up -d

cd ~

# Especifica o caminho absoluto para o arquivo de log
log_file="/root/container.log"

# Tenta criar o arquivo de log se não existir
touch $log_file

# Redireciona a saída de logs para o arquivo de log
docker logs -f maislocacoes >> $log_file 2>&1 &