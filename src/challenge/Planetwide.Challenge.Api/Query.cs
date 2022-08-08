using HotChocolate.Types;
using Planetwide.Challenge.Api.Infrastructure.Directives;

namespace Planetwide.Challenge.Api;

public static class PersonService
{
    public static Person Data = new Person
    {
        Name = "Luke Skywalker",
        Address = "Address"
    };
}

public class Queries
{
    public Person GetPerson()
    {
        return PersonService.Data;
    }
}

public class Mutations
{
    [Challenge]
    public Person UpdatePerson(PersonInput input)
    {
        PersonService.Data.Address = input.Address;
        return PersonService.Data;
    }
}