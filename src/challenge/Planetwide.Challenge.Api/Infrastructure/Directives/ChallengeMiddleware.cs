using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Validation;
using Microsoft.AspNetCore.Http;
using Planetwide.Challenge.Api.Infrastructure.DocumentValidators;
using Planetwide.Challenge.Api.Services;
using RequestDelegate = HotChocolate.Execution.RequestDelegate;

namespace Planetwide.Challenge.Api.Infrastructure.Directives;

public class ChallengeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly DocumentValidatorContextPool _contextPool;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ChallengeService _challengeService;
    private static readonly ConcurrentDictionary<string, bool> ChallengeDocumentCache = new();
    private const string SecretKeyThatShouldBeInVault = "TodoPullThisFormVaultOrSomething";

    public ChallengeMiddleware(RequestDelegate next,
        DocumentValidatorContextPool contextPool,
        IHttpContextAccessor httpContextAccessor,
        ChallengeService challengeService)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _contextPool = contextPool ?? throw new ArgumentNullException(nameof(contextPool));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _challengeService = challengeService;
    }

    public async ValueTask InvokeAsync(IRequestContext context)
    {
        if (context.Document == null)
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (HasChallengeResponse())
        {
            if (await ValidateChallengeResponse())
            {
                context.ContextData.Add(WellKnown.Context.ChallengedKey, true);
                await _next(context).ConfigureAwait(false);
                return;
            }

            context.Result = QueryResultBuilder.CreateError(
                ErrorBuilder.New()
                    .SetCode(WellKnown.Errors.ChallengeFailed)
                    .SetMessage("Your challenge failed, go away.")
                    .Build());

            return;
        }

        var (queryId, isChallengeRequired) = IsChallengeRequired(context);

        if (isChallengeRequired)
        {
            context.Result = await IssueChallenge(queryId);
            return;
        }

        await _next(context).ConfigureAwait(false);
    }

    private bool HasChallengeResponse()
    {
        var request = _httpContextAccessor.HttpContext?.Request ?? throw new ArgumentException("No http request");
        return request.Headers.ContainsKey(WellKnown.Headers.ChallengeResult);
    }

    private async Task<bool> ValidateChallengeResponse()
    {
        var headers = _httpContextAccessor.HttpContext?.Request.Headers ??
                      throw new ArgumentException("No http request or headers");
        var parsedChallenge = ServerChallenge.ParseHttpResponse(headers);
        var challengeOutcome = parsedChallenge.Value.Validate(SecretKeyThatShouldBeInVault);

        if (challengeOutcome != ChallengeOutcome.Valid)
        {
            return false;
        }

        var challengeResponse = headers[WellKnown.Headers.ChallengeResult];
        return await _challengeService.ValidateChallenge(challengeResponse);
    }

    private (string queryId, bool isChallengeRequired) IsChallengeRequired(IRequestContext context)
    {
        var document = context.Document;
        ArgumentNullException.ThrowIfNull(document);

        var queryId = context.DocumentId ?? context.DocumentHash ??
            throw new ArgumentException("The query has no id, we cannot issue the challenge");

#if DEBUG
        bool isChallengeRequired;
#else
        if (ChallengeDocumentCache.TryGetValue(queryId, out var isChallengeRequired))
        {
            return (queryId, isChallengeRequired);
        }
#endif

        var validatorContext = _contextPool.Get();
        PrepareContext(context, document, validatorContext);

        var visitor = new ChallengeVisitor();
        visitor.Visit(context.Document!, validatorContext);
        isChallengeRequired = visitor.ChallengeRequired;

        ChallengeDocumentCache.TryAdd(queryId, isChallengeRequired);
        return (queryId, isChallengeRequired);
    }

    private async Task<IQueryResult> IssueChallenge(string queryId)
    {
        var challengeMethods = await _challengeService.GetChallengeMethods();
        var challenge = ServerChallenge.CreateSignedChallenge(queryId,
            string.Join(',', challengeMethods), SecretKeyThatShouldBeInVault);

        var response = _httpContextAccessor.HttpContext?.Response;
        if (response == null)
        {
            throw new ArgumentException("The http context is missing");
        }

        challenge.SignMessage(response.Headers);

        var error = QueryResultBuilder.CreateError(
            ErrorBuilder.New()
                .SetCode(WellKnown.Errors.ChallengeRequired)
                .SetMessage("Additional Validation Required.")
                .Build());

        return error;
    }

    private void PrepareContext(
        IRequestContext requestContext,
        DocumentNode document,
        DocumentValidatorContext validatorContext)
    {
        validatorContext.Schema = requestContext.Schema;

        foreach (var node in document.Definitions)
        {
            if (node is FragmentDefinitionNode fragmentDefinition)
            {
                validatorContext.Fragments[fragmentDefinition.Name.Value] = fragmentDefinition;
            }
        }

        validatorContext.ContextData = requestContext.ContextData;
    }
}