FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Planetwide.Prompts.Api/Planetwide.Prompts.Api.csproj", "Planetwide.Prompts.Api/"]
RUN dotnet restore "Planetwide.Prompts.Api/Planetwide.Prompts.Api.csproj"
COPY . .
WORKDIR "/src/Planetwide.Prompts.Api"
RUN dotnet build "Planetwide.Prompts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planetwide.Prompts.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planetwide.Prompts.Api.dll"]
