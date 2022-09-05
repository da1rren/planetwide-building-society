using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Validation;
using Planetwide.Challenge.Api.Infrastructure.Directives;
using Planetwide.Challenge.Api.Infrastructure.DocumentValidators;
using Shouldly;

namespace Planetwide.Challenge.Api.Tests;

public class ChallengeVisitorTests
{
    public class FooBar
    {
        public string? Name { get; set; }
        
        [Challenge]
        public string? Address { get; set; }
    }
    
    public class FooQuery
    {
        [Challenge]
        public string? ChallengeFoo { get; set; }

        public string? Bar { get; set; }

        public FooBar GetFooBar() => new FooBar();
    }

    public class BarMutation
    {
        [Challenge]
        public string? ChallengeBar { get; set; }

        public string? OpenBar { get; set; }

        public FooBar GetFooBar() => new FooBar();
    }
    

    private static ISchemaBuilder DefaultSchema => SchemaBuilder.New()
        .AddDirectiveType<ChallengeDirectiveType>();
    
    public static bool Visit(DocumentNode documentNode, ISchema schema)
    {
        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(documentNode, new DocumentValidatorContext {Schema = schema});
        return challengeVisitor.ChallengeRequired;
    }
    
    [Fact]
    public void Query_Without_Challenge_Should_Be_False()
    {
        var query = Utf8GraphQLParser.Parse(@"
                query {
                    bar
                }
            ");

        var schema = DefaultSchema
            .AddQueryType<FooQuery>()
            .Create();

        Visit(query, schema).ShouldBeFalse(schema.ToString());
    }
    
    [Fact]
    public void Query_With_Property_Challenge_Should_Be_True()
    {
        var query = Utf8GraphQLParser.Parse(@"
                query {
                    challengeFoo
                }
            ");

        var schema = DefaultSchema
            .AddQueryType<FooQuery>()
            .Create();

        Visit(query, schema).ShouldBeTrue(schema.ToString());
    }
    
    [Fact]
    public void Query_With_Multiple_Properties_Challenge_Should_Be_True()
    {
        var query = Utf8GraphQLParser.Parse(@"
                query {
                    name
                    challengeFoo
                }
            ");

        var schema = DefaultSchema
            .AddQueryType<FooQuery>()
            .Create();

        Visit(query, schema).ShouldBeTrue(schema.ToString());
    }

    [Fact]
    public void Query_With_Nested_Challenge_Should_Be_True()
    {
        var query = Utf8GraphQLParser.Parse(@"
                query {
                    fooBar {
                        address
                    }
                }
            ");

        var schema = DefaultSchema
            .AddQueryType<FooQuery>()
            .Create();

        Visit(query, schema).ShouldBeTrue(schema.ToString());
    }
    
    [Fact]
    public void Mutation_Without_Challenge_Should_Be_False()
    {
        var mutation = Utf8GraphQLParser.Parse(@"
                mutation {
                    openBar
                }
            ");

        
        var schema = DefaultSchema
            .AddQueryType<FooQuery>()
            .AddMutationType<BarMutation>()
            .Create();

        Visit(mutation, schema).ShouldBeFalse(schema.ToString());
    }

    
    [Fact]
    public void Mutation_With_Challenge_On_Field_Should_Be_True()
    {
        var mutation = Utf8GraphQLParser.Parse(@"
                mutation {
                    challengeBar
                }
            ");

        var schema = DefaultSchema            
            .AddQueryType<FooQuery>()
            .AddMutationType<BarMutation>()
            .Create();

        Visit(mutation, schema).ShouldBeTrue(schema.ToString());
    }
    
    
    [Fact]
    public void Mutation_With_Nested_Challenge_Should_Be_True()
    {
        var mutation = Utf8GraphQLParser.Parse(@"
                mutation {
                    fooBar {
                        address
                    }
                }
            ");

        var schema = DefaultSchema            
            .AddQueryType<FooQuery>()
            .AddMutationType<BarMutation>()
            .Create();

        Visit(mutation, schema).ShouldBeTrue(schema.ToString());
    }
}
