FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["Planetwide.Accounts.Api/Planetwide.Accounts.Api.csproj", "Planetwide.Accounts.Api/"]
COPY ["Planetwide.Shared/Planetwide.Shared.csproj", "Planetwide.Shared/"]
RUN dotnet restore "Planetwide.Accounts.Api/Planetwide.Accounts.Api.csproj"
COPY . .
WORKDIR "/Planetwide.Accounts.Api"
RUN dotnet build "Planetwide.Accounts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planetwide.Accounts.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planetwide.Accounts.Api.dll"]
