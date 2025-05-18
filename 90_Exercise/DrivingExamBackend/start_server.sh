#!/bin/bash
git pull
dotnet restore --no-cache src/DrivingExamBackend.sln
dotnet run -c Debug --project src
