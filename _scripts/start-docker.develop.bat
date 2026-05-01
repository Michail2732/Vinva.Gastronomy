@echo off

echo Building Gastronomy develop...

echo Current directory:
echo %cd%

docker compose -f ../_configs/docker-compose.yml up -d --build

echo Done!
pause