using Planetwide.Challenge.Api.Infrastructure.Directives;

namespace Planetwide.Challenge.Api;

public class Person
{
    public string Name { get; set;  }
    
    [Challenge]
    public string Address { get; set; }
}

public record PersonInput(string? Address, string? Address2);
