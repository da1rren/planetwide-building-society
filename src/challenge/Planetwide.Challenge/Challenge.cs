using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using FluentResults;
using Microsoft.Extensions.Primitives;
using Planetwide.Challenge.Extensions;

namespace Planetwide.Challenge;

public enum ChallengeOutcome
{
    Default = 0,
    InvalidSignature = 1,
    Expired = 2,
    Valid = 4
}

public record ServerChallenge(string RequestHash, long RequestTimestamp, string ChallengeMethods, string Signature)
{
    private static string CreateHashMaterial(string requestHash, long requestTimestamp, 
        string challengeMethods, string secretKey) =>
        $"{requestHash}{requestTimestamp}{challengeMethods}{secretKey}";

    private static string CalculateSignature(string hashMaterial)
    {
        var hashMaterialBytes = Encoding.Default.GetBytes(hashMaterial);
        var hashBytes = SHA384.HashData(hashMaterialBytes);
        var formattedHash = Convert.ToBase64String(hashBytes);
        return formattedHash;
    }
    
    public static ServerChallenge CreateSignedChallenge(string requestHash, string challengeMethods, string secretKey,
        DateTimeOffset? expiresAt = default)
    {
        expiresAt ??= DateTimeOffset.Now.AddMinutes(5);
        var expiresAtUnix = expiresAt.Value.ToUnixTimeSeconds();
        
        var hashMaterial = CreateHashMaterial(requestHash, expiresAtUnix, challengeMethods, secretKey);
        var signature = CalculateSignature(hashMaterial);
        return new ServerChallenge(requestHash, expiresAtUnix, challengeMethods, signature);
    }

    public static Result<ServerChallenge> ParseHttpResponse(IDictionary<string, StringValues> headers)
    {
        var requestHashResult = headers.TryGetValue(
            WellKnown.Headers.RequestHash, out var requestHash);
        
        var challengeMethodsResult = headers.TryGetValue(
            WellKnown.Headers.ChallengeMethods, out var challengeMethod);
        
        var requestTimestampResult = headers.TryGetValue(
            WellKnown.Headers.RequestTimestamp, out var requestTimestamp);

        var canParseRequestTimestamp = long.TryParse(requestTimestamp, out var parsedRequestTimestamp);
        
        var signatureResult = headers.TryGetValue(
            WellKnown.Headers.Signature, out var signature);

        if (!requestHashResult || !challengeMethodsResult || !requestTimestampResult ||
            !canParseRequestTimestamp || !signatureResult )
        {
            return Result.Fail("Headers missing or unparsable");
        }
        
        return new ServerChallenge(
            requestHash,
            parsedRequestTimestamp,
            challengeMethod,
            signature
        );
    }
    
    public static Result<ServerChallenge> ParseHttpResponse(HttpRequestMessage message)
    {
        var requestHashResult = message.Headers.TryGetValue(WellKnown.Headers.RequestHash);
        var challengeMethodsResult = message.Headers.TryGetValue(WellKnown.Headers.ChallengeMethods);
        var requestTimestampResult = message.Headers.TryGetValue(WellKnown.Headers.RequestTimestamp);
        var signatureResult = message.Headers.TryGetValue(WellKnown.Headers.Signature);
            
        if (!long.TryParse(requestTimestampResult.Value, out var timestamp))
        {
            requestTimestampResult = Result.Fail<string>($"Cannot parse {WellKnown.Headers.RequestTimestamp}");
        }

        var parseResult = Result.Merge(
            requestHashResult,
            challengeMethodsResult,
            requestTimestampResult,
            signatureResult);

        if (parseResult.IsFailed)
        {
            return Result.Fail(parseResult.Errors);
        }
        
        return new ServerChallenge(
            requestHashResult.Value,
            timestamp,
            challengeMethodsResult.Value,
            signatureResult.Value
        );
    }

    public void SignMessage(IDictionary<string, StringValues> headers)
    {
        headers.Add(WellKnown.Headers.RequestHash, RequestHash);
        headers.Add(WellKnown.Headers.ChallengeMethods, ChallengeMethods);
        headers.Add(WellKnown.Headers.RequestTimestamp, RequestTimestamp.ToString());
        headers.Add(WellKnown.Headers.Signature, Signature);
    }
    
    public void SignMessage(System.Net.Http.Headers.HttpHeaders headers)
    {
        headers.Add(WellKnown.Headers.RequestHash, RequestHash);
        headers.Add(WellKnown.Headers.ChallengeMethods, ChallengeMethods);
        headers.Add(WellKnown.Headers.RequestTimestamp, RequestTimestamp.ToString());
        headers.Add(WellKnown.Headers.Signature, Signature);
    }

    public ChallengeOutcome Validate(string secretKey)
    {
        var expiredAt = DateTimeOffset.FromUnixTimeSeconds(RequestTimestamp);

        if (DateTimeOffset.Now > expiredAt)
        {
            return ChallengeOutcome.Expired;
        }
        
        var hashMaterial = CreateHashMaterial(RequestHash, RequestTimestamp, ChallengeMethods, secretKey);
        var signature = CalculateSignature(hashMaterial);
        
        return signature.Equals(Signature) ? ChallengeOutcome.Valid : ChallengeOutcome.InvalidSignature;
    }
}
    