@echo off

echo Building Gastronomy develop...

echo Current directory:
echo %cd%

docker compose -f ../_configs/docker-compose.yml -f ../_configs/docker-compose.prod.yml up -d --build

echo Done!
pause