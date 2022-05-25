FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ["Planetwide.Blazor.Ui/Planetwide.Blazor.Ui.csproj", "Planetwide.Blazor.Ui/"]
RUN dotnet restore "Planetwide.Blazor.Ui/Planetwide.Blazor.Ui.csproj"
COPY . .
WORKDIR "/Planetwide.Blazor.Ui"
RUN dotnet build "Planetwide.Blazor.Ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planetwide.Blazor.Ui.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY Planetwide.Blazor.Ui/nginx.conf /etc/nginx/nginx.conf
