using System.Net.Http.Json;
using System.Threading.Tasks;
using Planetwide.Accounts.Api.Tests.Fixtures;

namespace Planetwide.Accounts.Api.Tests.Features.Accounts;

public class AccountQueriesTests : IClassFixture<AccountApiApplicationFactory>
{
    private readonly AccountApiApplicationFactory _factory;

    public AccountQueriesTests(AccountApiApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Account_Should_Return_Accounts_List()
    {
        var client = _factory.CreateDefaultClient();
        
        var response = await client.PostAsJsonAsync("/graphql", new
        {
            query = @"
{
  accounts {
    nodes{
      id
      number
    }
  }
}
"
        });

        var json = await response.Content.ReadAsStringAsync();
        Snapshot.Match(json);
    }
}