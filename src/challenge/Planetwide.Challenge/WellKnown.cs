namespace Planetwide.Challenge;

public static class WellKnown
{
    public static class Context
    {
        public const string ChallengedKey = "challenge-passed";
    }
    
    public static class Headers
    {
        public const string RequestHash = "pbs-request-hash";
        public const string RequestTimestamp = "pbs-request-timestamp";
        public const string ChallengeMethods = "pbs-challenge-methods";
        public const string Signature = "pbs-signature";
        public const string ChallengeResult = "pbs-challenge-result";
    }
    
    public static class Errors
    {
        public const string ChallengeRequired = "401-Chalenge";
        
        public const string ChallengeFailed = "403-Forbidden";
    }
}