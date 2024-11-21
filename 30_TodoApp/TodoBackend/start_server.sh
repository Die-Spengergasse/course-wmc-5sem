#!/bin/bash
git pull
dotnet restore --no-cache src/TodoBackend.sln
dotnet run -c Debug --project src
