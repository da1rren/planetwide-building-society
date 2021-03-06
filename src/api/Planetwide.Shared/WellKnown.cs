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
        public const string Accounts = "Accounts";
        public const string Members = "Members";
        public const string Transactions = "Transactions";
        public const string Prompts = "Prompts";

    }
}