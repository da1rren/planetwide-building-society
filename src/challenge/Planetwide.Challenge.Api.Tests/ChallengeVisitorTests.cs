using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Language;
using HotChocolate.Validation;
using Planetwide.Challenge.Api.Infrastructure.Directives;
using Planetwide.Challenge.Api.Infrastructure.DocumentValidators;
using Shouldly;

namespace Planetwide.Challenge.Api.Tests;

public class ChallengeVisitorTests
{
    private readonly DocumentNode _query = Utf8GraphQLParser.Parse(@"
                query {
                    foo {
                        bar
                    }
                }
            ");

    private readonly DocumentNode _mutation = Utf8GraphQLParser.Parse(@"
                mutation {
                    loo {
                        doo
                    }
                }
            ");


    private static ISchemaBuilder DefaultSchema => SchemaBuilder.New()
        .AddDirectiveType<ChallengeDirectiveType>();
    
    [Fact]
    public void Query_Without_Challenge_Should_Be_False()
    {
        var schema = DefaultSchema
            .AddQueryType(t => t.Name("foo").Field("bar").Resolve("Hello"))
            .Create();

        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(_query, new DocumentValidatorContext {Schema = schema});

        challengeVisitor.ChallengeRequired.ShouldBeFalse(schema.ToString());
    }
    
    [Fact]
    public void Query_With_Property_Challenge_Should_Be_True()
    {
        var schema = DefaultSchema
            .AddQueryType(t => t.Name("foo")
                .Directive<ChallengeDirectiveType>()
                .Field("bar")
                .Resolve("Hello"))
            .Create();

        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(_query, new DocumentValidatorContext {Schema = schema});

        challengeVisitor.ChallengeRequired.ShouldBeTrue(schema.ToString());
    }

    [Fact]
    public void Query_With_Field_Challenge_Should_Be_True()
    {
        var schema = DefaultSchema
            .AddQueryType(t => t.Name("foo")
                .Field("bar")
                .Directive<ChallengeDirectiveType>()
                .Resolve("Hello"))
            .Create();

        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(_query, new DocumentValidatorContext {Schema = schema});

        challengeVisitor.ChallengeRequired.ShouldBeTrue(schema.ToString());
    }

    [Fact]
    public void Mutation_Without_Challenge_Should_Be_False()
    {
        var schema = DefaultSchema
            .AddQueryType(t => t.Name("foo")
                .Field("bar")
                .Resolve("Hello"))
            .AddMutationType(t => t.Name("loo")
                .Field("doo")
                .Resolve("Hello"))
            .Create();

        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(_mutation, new DocumentValidatorContext {Schema = schema});
        
        challengeVisitor.ChallengeRequired.ShouldBeFalse(schema.ToString());
    }

    
    [Fact]
    public void Mutation_With_Challenge_On_Field_Should_Be_True()
    {
        var schema = DefaultSchema
            .AddQueryType(t => t.Name("foo")
                .Field("bar")
                .Resolve("Hello"))
            .AddMutationType(t => t.Name("loo")
                .Field("doo")
                .Directive<ChallengeDirectiveType>()
                .Resolve("Hello"))
            .Create();

        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(_mutation, new DocumentValidatorContext {Schema = schema});
        
        challengeVisitor.ChallengeRequired.ShouldBeTrue(schema.ToString());
    }
    
    [Fact]
    public void Mutation_With_Challenge_On_Property_Should_Be_True()
    {
        var schema = DefaultSchema
            .AddQueryType(t => t.Name("foo")
                .Field("bar")
                .Resolve("Hello"))
            .AddMutationType(t => t.Name("loo")
                .Directive<ChallengeDirectiveType>()
                .Field("doo")
                .Resolve("Hello"))
            .Create();

        var challengeVisitor = new ChallengeVisitor();
        challengeVisitor.Visit(_mutation, new DocumentValidatorContext {Schema = schema});
        
        challengeVisitor.ChallengeRequired.ShouldBeTrue(schema.ToString());
    }

}