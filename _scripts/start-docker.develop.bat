@echo off
setlocal EnableExtensions EnableDelayedExpansion

REM ============================================================
REM Run local development environment:
REM 1) Check required tools
REM 2) Generate localhost HTTPS certificate for nginx if missing
REM 3) Build .NET projects
REM 4) Run docker compose dev
REM ============================================================

set "SCRIPT_DIR=%~dp0"
cd /d "%SCRIPT_DIR%\.."

set "CERT_DIR=_configs\certs"
set "CERT_KEY=%CERT_DIR%\localhost.key"
set "CERT_CRT=%CERT_DIR%\localhost.crt"
set "DOTNET_BUILD_TARGET=Vinva.Gastronomy.sln"
set "COMPOSE_BASE=_configs\docker-compose.yml"
set "COMPOSE_OVERRIDE=_configs\docker-compose.override.yml"

echo.
echo ============================================================
echo Checking required tools...
echo ============================================================

where dotnet >nul 2>&1
if errorlevel 1 (
    echo [ERROR] dotnet CLI was not found.
    exit /b 1
)

where docker >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Docker CLI was not found.
    exit /b 1
)

docker info >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Docker is not running.
    exit /b 1
)

where openssl >nul 2>&1
if errorlevel 1 (
    echo [ERROR] OpenSSL was not found.
    echo Install Git for Windows or OpenSSL and add it to PATH.
    exit /b 1
)

echo [OK] Required tools are available.

echo.
echo ============================================================
echo Checking local HTTPS certificate...
echo ============================================================

echo Current dir: %cd%
echo CERT_DIR: %CERT_DIR%

if not exist "%CERT_DIR%" (
    echo Creating directory: %CERT_DIR%
    mkdir "%CERT_DIR%"
    if errorlevel 1 (
        echo [ERROR] Failed to create certificate directory.
        exit /b 1
    )
)

set "NEED_GENERATE_CERT=false"

if not exist "%CERT_KEY%" set "NEED_GENERATE_CERT=true"
if not exist "%CERT_CRT%" set "NEED_GENERATE_CERT=true"

if "%NEED_GENERATE_CERT%"=="false" (
    echo [OK] Certificate already exists:
    echo      %CERT_CRT%
    echo      %CERT_KEY%
) else (
    echo Certificate files are missing. Generating self-signed localhost certificate...

    if exist "%CERT_KEY%" del /f /q "%CERT_KEY%" >nul 2>&1
    if exist "%CERT_CRT%" del /f /q "%CERT_CRT%" >nul 2>&1

    openssl req -x509 -nodes -days 365 ^
        -newkey rsa:2048 ^
        -keyout "%CERT_KEY%" ^
        -out "%CERT_CRT%" ^
        -subj "/CN=localhost" ^
        -config NUL

    echo OpenSSL exit code: %errorlevel%

    if errorlevel 1 (
        echo [ERROR] Failed to generate HTTPS certificate.
        exit /b 1
    )

    if not exist "%CERT_KEY%" (
        echo [ERROR] Private key was not created: %CERT_KEY%
        exit /b 1
    )

    if not exist "%CERT_CRT%" (
        echo [ERROR] Certificate was not created: %CERT_CRT%
        exit /b 1
    )

    echo [OK] Certificate generated successfully.
)

echo.
echo ============================================================
echo Building .NET projects...
echo ============================================================

if not exist "%DOTNET_BUILD_TARGET%" (
    echo [ERROR] .NET build target was not found:
    echo        %DOTNET_BUILD_TARGET%
    exit /b 1
)

dotnet restore "%DOTNET_BUILD_TARGET%"
if errorlevel 1 (
    echo [ERROR] dotnet restore failed.
    exit /b 1
)

dotnet build "%DOTNET_BUILD_TARGET%" --configuration Debug --no-restore
if errorlevel 1 (
    echo [ERROR] dotnet build failed.
    exit /b 1
)

echo [OK] .NET build completed.

echo.
echo ============================================================
echo Starting Docker Compose...
echo ============================================================

if exist "%COMPOSE_BASE%" (
    if exist "%COMPOSE_OVERRIDE%" (
        docker compose -f "%COMPOSE_BASE%" -f "%COMPOSE_OVERRIDE%" up --build
    ) else (
        docker compose -f "%COMPOSE_BASE%" up --build
    )
) else (
    docker compose up --build
)

if errorlevel 1 (
    echo [ERROR] Docker Compose failed.
    exit /b 1
)

endlocal