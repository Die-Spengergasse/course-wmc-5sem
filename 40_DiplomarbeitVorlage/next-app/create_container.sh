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

# Starte den Container
# Achte darauf, dass NEXTAUTH_URL auf den richtigen Port gesetzt ist.
DATABASE_PATH="$(pwd)/database"
if [ ! -d "$DATABASE_PATH" ]; then
    echo "[WARNING] Das Verzeichnis $DATABASE_PATH existiert nicht. Du kannst mit npm run init_db die Datenbank erstellen."
fi
if [ ! -f ".env.local" ]; then
    echo "[ERROR] Die Datei .env.local, die docker run übergeben werden soll, existiert nicht."
    exit 1
fi

docker run -d -p 80:80 --name $DOCKER_IMAGE \
    --env-file .env.local \
    -e "NEXTAUTH_URL=http://localhost" \
    -e "AUTH_TRUST_HOST=true" \
    -v "$DATABASE_PATH":/app/database \
    $DOCKER_IMAGE

