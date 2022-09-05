using System.Linq;
using System.Net.Http.Headers;
using FluentResults;

namespace Planetwide.Challenge.Extensions;

public static class HttpRequestMessageExtensions
{
    public static Result<string> TryGetValue(this HttpHeaders headers, string name)
    {
        if (!headers.TryGetValues(name, out var values))
        {
            return Result.Fail($"Header {name} not found");
        }

        values = values.ToArray();
        
        if (!values.Any())
        {
            return Result.Fail($"Header {name} does not include a value");
        }

        if (values.Count() != 1)
        {
            return Result.Fail($"Header {name} occurs {values.Count()} times");
        }

        var value = values.Single();

        return string.IsNullOrWhiteSpace(value) ? 
            Result.Fail($"header {name} is empty") : Result.Ok(value);
    }
}