#!/bin/bash

cd ~/MaisLocacoesApi/src/

git pull

docker-compose build
docker-compose up -d

cd ~

#tenta criar o arquivo de log apenas por segurança
touch container.log

#Redireciona a saída de logs para o arquivo de log
docker logs -f maislocacoes > /root/container.log &
