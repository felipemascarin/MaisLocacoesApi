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

cd ~
#Tenta criar a pasta do repositório da API na raiz se caso não existir
mkdir MaisLocacoesApi

cd MaisLocacoesApi
git pull git@github.com:felipemascarin/MaisLocacoesApi.git

cd MaisLocacoesApi/src
docker-compose build
docker-compose up -d

cd ~
#tenta criar o arquivo de log se não existir
touch container.log

#Redireciona a saída de logs para o arquivo de log
docker logs -f maislocacoes > /root/container.log &
