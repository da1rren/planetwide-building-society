using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Planetwide.Challenge.Api.Services;

public class ChallengeService
{
    public Task<bool> IsRequired()
    {
        return Task.FromResult(true);
    }

    public Task<IEnumerable<string>> GetChallengeMethods()
    {
        var methods = new List<string>
        {
            "card-reader",
            "3-of-6",
            "biometric"
        };

        return Task.FromResult<IEnumerable<string>>(methods);
    }

    public Task<bool> ValidateChallenge(string challengeResponse)
    {
        return Task.FromResult(challengeResponse.Equals("Letmein123"));
    }
}