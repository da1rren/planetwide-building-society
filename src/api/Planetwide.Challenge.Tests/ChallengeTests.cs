namespace Planetwide.Challenge.Tests;

public class ChallengeTests
{
    [Fact]
    public void Server_Challenge_Should_Validate_Successfully()
    {
        var key = "SuperDuperSecret";

        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            key
        );

        serverChallenge.Validate(key).ShouldBe(ChallengeOutcome.Valid);
    }

    [Fact]
    public void Server_Challenge_Should_Not_Validate_With_Wrong_Key()
    {
        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            "SuperDuperSecret"
        );

        serverChallenge.Validate("SuperWrongSecret").ShouldBe(ChallengeOutcome.InvalidSignature);
    }

    [Fact]
    public void Server_Challenge_Should_Not_Validate_When_Request_Hash_Has_Changed()
    {
        var key = "SuperDuperSecret";

        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            key
        );

        serverChallenge = serverChallenge with
        {
            RequestHash = "Imwrong"
        };

        serverChallenge.Validate(key).ShouldBe(ChallengeOutcome.InvalidSignature);
    }

    [Fact]
    public void Server_Challenge_Should_Not_Validate_When_Challenge_Methods_Has_Changed()
    {
        var key = "SuperDuperSecret";

        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            key
        );

        serverChallenge = serverChallenge with
        {
            ChallengeMethods = "all-your-bases-belong-to-us"
        };

        serverChallenge.Validate(key).ShouldBe(ChallengeOutcome.InvalidSignature);
    }

    [Fact]
    public void Server_Challenge_Should_Not_Validate_When_Date_Stamp_Has_Changed()
    {
        var key = "SuperDuperSecret";

        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            key
        );

        serverChallenge = serverChallenge with
        {
            RequestTimestamp = DateTimeOffset.Now.AddMinutes(500)
                .ToUnixTimeSeconds()
        };

        serverChallenge.Validate(key).ShouldBe(ChallengeOutcome.InvalidSignature);
    }

    [Fact]
    public void Expired_Server_Challenge_Should_Not_Validate()
    {
        var key = "SuperDuperSecret";

        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            key,
            expiresAt: DateTimeOffset.Now.AddMinutes(-5)
        );

        serverChallenge = serverChallenge with
        {
            RequestTimestamp = DateTimeOffset.Now.AddMinutes(-5)
                .ToUnixTimeSeconds()
        };

        serverChallenge.Validate(key).ShouldBe(ChallengeOutcome.Expired);

    }

    [Fact]
    public void Signed_Http_Request_Header_Should_Validate()
    {
        var key = "SuperDuperSecret";
        
        var serverChallenge = ServerChallenge.CreateSignedChallenge(
            Guid.NewGuid().ToString(),
            "card-reader",
            key,
            expiresAt: DateTimeOffset.Now.AddMinutes(-5)
        );

        var message = new HttpRequestMessage();
        var headers = message.Headers;
        
        serverChallenge.SignMessage(headers);

        headers.Contains(WellKnown.Headers.Signature).ShouldBeTrue();
        headers.Contains(WellKnown.Headers.ChallengeMethods).ShouldBeTrue();
        headers.Contains(WellKnown.Headers.RequestHash).ShouldBeTrue();
        headers.Contains(WellKnown.Headers.RequestTimestamp).ShouldBeTrue();

        var parsedResponse = ServerChallenge.ParseHttpResponse(message);
        
        parsedResponse.IsSuccess.ShouldBeTrue();
    }
}