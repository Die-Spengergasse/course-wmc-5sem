#!/bin/bash

SSL_CERT_FILE="todo_backend.pfx"                  # generated with dotnet dev-certs in this script
DOCKER_IMAGE=todo_backend
# Generate random secret (the secret in appsettings.json is empty)
SECRET=$(dd if=/dev/random bs=128 count=1 2> /dev/null | base64)
BRANCH=$(git branch --show-current)
CWD=$(pwd)

# Create HTTPS Certificates
CERT_PASS=$(dd if=/dev/random bs=128 count=1 2> /dev/null | base64)
rm "$HOME/.aspnet/https/$SSL_CERT_FILE"
dotnet dev-certs https -ep "$HOME/.aspnet/https/$SSL_CERT_FILE" -p "$CERT_PASS"
dotnet dev-certs https --trust

docker rm -f $DOCKER_IMAGE
docker build -t $DOCKER_IMAGE . 
MSYS_NO_PATHCONV=1 docker run -d -p 5000:80 -p 5001:443 --name $DOCKER_IMAGE \
    -e "ASPNETCORE_URLS=https://+;http://+" \
    -e ASPNETCORE_Kestrel__Certificates__Default__Password="$CERT_PASS" \
    -e ASPNETCORE_Kestrel__Certificates__Default__Path="/https/$SSL_CERT_FILE" \
    -e "ASPNETCORE_ENVIRONMENT=Production" \
    -e "SECRET=$SECRET" \
    -v $HOME/.aspnet/https:/https/ \
    $DOCKER_IMAGE
