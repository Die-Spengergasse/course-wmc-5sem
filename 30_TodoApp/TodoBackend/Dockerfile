# Test with
# docker build -t todo_backend . && docker run -d -p 5001:5001 -e "ASPNETCORE_HTTPS_PORT=5001" --name todo_backend todo_backend
# Build container. Load full SDK as base image.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# TODO: Adapt the directories!
COPY src TodoBackend

# Compile the app
RUN dotnet restore "TodoBackend"
RUN dotnet build   "TodoBackend" -c Release -o /app/build
RUN dotnet publish "TodoBackend" -c Release -o /app/publish /p:UseAppHost=false

# App container. Only needs runtime (smaller image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
EXPOSE 80
EXPOSE 443
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoBackend.dll"]