#!/bin/bash
set -e

apt-get update && apt-get install -y curl

# Instalação do SDK do .NET 6.0
curl -SL --output dotnet-sdk.tar.gz https://download.visualstudio.microsoft.com/download/pr/dd7d2255-c9c1-4c6f-b8ad-6e853d6bb574/c8e1b5f47bf17b317a84487491915178/dotnet-sdk-6.0.408-linux-x64.tar.gz \
    && dotnet_sha512='d5eed37ce6c07546aa217d6e786f3b67be2b6d97c23d5888d9ee5d5398e8a9bfc06202b14e3529245f7ec78f4036778caf69bdbe099de805fe1f566277e8440e' \
    && echo "$dotnet_sha512 dotnet-sdk.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet-sdk.tar.gz \
    && rm dotnet-sdk.tar.gz \
    && if [ ! -e /usr/bin/dotnet ]; then ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet; fi

dotnet tool install --global dotnet-ef --version 6.0.16

export PATH="$PATH:/root/.dotnet/tools"

# Redireciona a saída padrão para um arquivo no host
exec > >(tee /root/container.log) 2>&1

exec "$@"