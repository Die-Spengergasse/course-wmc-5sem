#!/bin/bash
set -e

echo Pull changes...
git pull

if ! docker stats --no-stream; then
    echo "Docker is not running. Please start Docker and try again."
    exit 1
fi

DOCKER_IMAGE=nextjs-app

docker rm -f $DOCKER_IMAGE
docker build -t $DOCKER_IMAGE . 
MSYS_NO_PATHCONV=1 docker run -d -p 443:443 --name $DOCKER_IMAGE \
    --env-file .env.local \
    -e "NEXTAUTH_URL=https://localhost" \
    -e "AUTH_TRUST_HOST=true" \
    -v database:/app/database \
    $DOCKER_IMAGE

