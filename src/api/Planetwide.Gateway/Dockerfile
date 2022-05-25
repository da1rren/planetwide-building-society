FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Planetwide.Gateway/Planetwide.Gateway.csproj", "Planetwide.Gateway/"]
COPY ["Planetwide.Shared/Planetwide.Shared.csproj", "Planetwide.Shared/"]
RUN dotnet restore "Planetwide.Gateway/Planetwide.Gateway.csproj"
COPY . .
WORKDIR "/src/Planetwide.Gateway"
RUN dotnet build "Planetwide.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planetwide.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planetwide.Gateway.dll"]
