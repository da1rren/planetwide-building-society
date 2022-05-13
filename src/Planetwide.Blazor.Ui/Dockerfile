FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["Planetwide.Blazor.Ui/Planetwide.Blazor.Ui.csproj", "Planetwide.Blazor.Ui/"]
RUN dotnet restore "Planetwide.Blazor.Ui/Planetwide.Blazor.Ui.csproj"
COPY . .
WORKDIR "/Planetwide.Blazor.Ui"
RUN dotnet build "Planetwide.Blazor.Ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planetwide.Blazor.Ui.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planetwide.Blazor.Ui.dll"]
