# Mount /app to your publish/bin directory to use
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
ENTRYPOINT ["dotnet", "Planetwide.Transactions.Api.dll"]
