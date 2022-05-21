FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["Planetwide.Members.Api/Planetwide.Members.Api.csproj", "Planetwide.Members.Api/"]
COPY ["Planetwide.Graphql.Shared/Planetwide.Graphql.Shared.csproj", "Planetwide.Graphql.Shared/"]
COPY ["Planetwide.Shared/Planetwide.Shared.csproj", "Planetwide.Shared/"]
RUN dotnet restore "Planetwide.Members.Api/Planetwide.Members.Api.csproj"
COPY . .
WORKDIR "/Planetwide.Members.Api"
RUN dotnet build "Planetwide.Members.Api.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/Planetwide.Members.Api"
RUN dotnet publish "Planetwide.Members.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planetwide.Members.Api.dll"]
