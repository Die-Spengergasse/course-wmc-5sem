@echo off
setlocal

echo Pull changes...
git pull

REM Prüfen, ob Docker läuft
docker stats --no-stream >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Docker is not running. Please start Docker and try again.
    exit /b 1
)

set DOCKER_IMAGE=nextjs-app
docker rm -f %DOCKER_IMAGE% >nul 2>&1

docker build -t %DOCKER_IMAGE% .
if errorlevel 1 (
    echo [ERROR] Failed to build Docker container.
    pause
    exit /b 1
)

REM Starte den Container
REM Achte darauf, dass NEXTAUTH_URL auf den richtigen Port gesetzt ist.
SET DATABASE_PATH=%CD%\database
if not exist "%DATABASE_PATH%" (
    echo [WARNING] Das Verzeichnis database existiert nicht. Du kannst mit npm run init_db die Datenbank erstellen.
    pause
)
if not exist ".env.local" (
    echo [ERROR] Die Datei .env.local, die docker run übergeben werden soll, existiert nicht.
    pause
    exit /b 1
)

docker run -d -p 80:80 --name %DOCKER_IMAGE% ^
    --env-file .env.local ^
    -e "NEXTAUTH_URL=http://localhost" ^
    -e "AUTH_TRUST_HOST=true" ^
    -v "%DATABASE_PATH%":/app/database ^
    %DOCKER_IMAGE%
if errorlevel 1 (
    echo [ERROR] Failed to start Docker container.
    pause
    exit /b 1
)

echo [INFO] Docker container started successfully.
start http://localhost
