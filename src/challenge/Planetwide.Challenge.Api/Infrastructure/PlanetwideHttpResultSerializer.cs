using System.Linq;
using System.Net;
using HotChocolate.AspNetCore.Serialization;
using HotChocolate.Execution;

namespace Planetwide.Challenge.Api.Infrastructure;

public class PlanetwideHttpResultSerializer : DefaultHttpResultSerializer
{
    public override HttpStatusCode GetStatusCode(IExecutionResult result)
    {
        if (result is not IQueryResult queryResult)
        {
            return base.GetStatusCode(result);
        }

        var errors = queryResult.Errors;
        if (errors == null || !errors.Any())
        {
            return base.GetStatusCode(result);
        }

        if (errors.Any(x => x.Code == WellKnown.Errors.ChallengeRequired))
        {
            return HttpStatusCode.Unauthorized;
        }
        
        if (errors.Any(x => x.Code == WellKnown.Errors.ChallengeFailed))
        {
            return HttpStatusCode.Forbidden;
        }
        
        return base.GetStatusCode(result);
    }
}