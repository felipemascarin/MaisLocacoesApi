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

cd ~/MaisLocacoesApi/src
git clean -df
git pull
docker-compose build
docker-compose up -d

cd ~
#tenta criar o arquivo de log se não existir
touch container.log

#Redireciona a saída de logs para o arquivo de log
docker logs -f maislocacoes >> /root/container.log 2>&1 &