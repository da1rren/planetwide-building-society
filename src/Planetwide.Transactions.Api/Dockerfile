FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Planetwide.Transactions.Api/Planetwide.Transactions.Api.csproj", "Planetwide.Transactions.Api/"]
COPY ["Planetwide.Graphql.Shared/Planetwide.Graphql.Shared.csproj", "Planetwide.Graphql.Shared/"]
COPY ["Planetwide.Shared/Planetwide.Shared.csproj", "Planetwide.Shared/"]
RUN dotnet restore "Planetwide.Transactions.Api/Planetwide.Transactions.Api.csproj"
COPY . .
WORKDIR "/src/Planetwide.Transactions.Api"
RUN dotnet build "Planetwide.Transactions.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planetwide.Transactions.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planetwide.Transactions.Api.dll"]
