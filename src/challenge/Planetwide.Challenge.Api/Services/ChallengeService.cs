using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Planetwide.Challenge.Api.Services;

public class ChallengeService
{
    public async Task<bool> IsRequired()
    {
        return true;
    }

    public async Task<IEnumerable<string>> GetChallengeMethods()
    {
        var methods = new List<string>
        {
            "card-reader",
            "3-of-6",
            "biometric"
        };

        return methods;
    }

    public async Task<bool> ValidateChallenge(string challengeResponse)
    {
        return challengeResponse.Equals("Letmein123");
    }
}