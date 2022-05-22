FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY ["Planetwide.Accounts.Api/Planetwide.Accounts.Api.csproj", "Planetwide.Accounts.Api/"]
COPY ["Planetwide.Members.Api/Planetwide.Members.Api.csproj", "Planetwide.Members.Api/"]
COPY ["Planetwide.Transactions.Api/Planetwide.Transactions.Api.csproj", "Planetwide.Transactions.Api/"]
COPY ["Planetwide.Gateway/Planetwide.Gateway.csproj", "Planetwide.Gateway/"]
COPY ["Planetwide.Blazor.Ui/Planetwide.Blazor.Ui.csproj", "Planetwide.Blazor.Ui/"]
COPY ["Planetwide.Graphql.Shared/Planetwide.Graphql.Shared.csproj", "Planetwide.Graphql.Shared/"]
COPY ["Planetwide.Shared/Planetwide.Shared.csproj", "Planetwide.Shared/"]
COPY ["Planetwide.Accounts.Api.Tests/Planetwide.Accounts.Api.Tests.csproj", "Planetwide.Accounts.Api.Tests/"]
COPY ["Planetwide.Members.Api.Tests/Planetwide.Members.Api.Tests.csproj", "Planetwide.Members.Api.Tests/"]
COPY ["Planetwide.Transactions.Api.Tests/Planetwide.Transactions.Api.Tests.csproj", "Planetwide.Transactions.Api.Tests/"]
COPY ["Planetwide.sln", "Planetwide.sln"]

RUN dotnet restore "Planetwide.sln"
CMD dotnet watch run
