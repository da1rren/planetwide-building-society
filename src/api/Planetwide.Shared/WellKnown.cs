namespace Planetwide.Shared;

public static class WellKnown
{
    public static class Database
    {
        public const string MongoDatabase = "planetwide";
    }

    public static class Schemas
    {
        public static IReadOnlyList<string> All = new List<string>
        {
            Accounts,
            Members,
            Transactions,
            Prompts
        };

        public const string SchemaKey = "planetwide";
        public const string Accounts = "accounts";
        public const string Members = "members";
        public const string Transactions = "transactions";
        public const string Prompts = "prompts";

    }
}