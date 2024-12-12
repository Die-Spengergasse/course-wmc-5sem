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

set "DOCKER_IMAGE=nextjs-app"
docker rm -f %DOCKER_IMAGE% >nul 2>&1

docker build -t %DOCKER_IMAGE% .
if errorlevel 1 (
    echo [ERROR] Failed to build Docker container.
    pause
    exit /b 1
)

docker run -d -p 443:443 --name %DOCKER_IMAGE% ^
    --env-file .env.local ^
    -e "NEXTAUTH_URL=https://localhost" ^
    -e "AUTH_TRUST_HOST=true" ^
    -v database:/app/database ^
    %DOCKER_IMAGE%
if errorlevel 1 (
    echo [ERROR] Failed to start Docker container.
    pause
    exit /b 1
)

echo [INFO] Docker container started successfully.
start https://localhost
