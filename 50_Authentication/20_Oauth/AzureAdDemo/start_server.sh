#!/bin/bash
git pull
dotnet restore --no-cache
dotnet run -c Debug
