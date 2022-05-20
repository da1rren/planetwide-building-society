namespace Planetwide.Gateway;

public static class WellKnown
{
    public static class Schemas
    {
        public static IReadOnlyList<string> All = new List<string>
        {
            Accounts,
            Members,
            Transactions
        };
        
        public const string Accounts = "Accounts";
        public const string Members = "Members";
        public const string Transactions = "Transactions";
    }
}