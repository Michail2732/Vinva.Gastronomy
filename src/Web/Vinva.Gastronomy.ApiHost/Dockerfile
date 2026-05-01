FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY . .

WORKDIR /app/src/Web/Vinva.Gastronomy.ApiHost

EXPOSE 8080

ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]