using System.Reflection;
using System.Runtime.CompilerServices;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Planetwide.Challenge.Api.Infrastructure.Directives;

public class ChallengeDirectiveType : DirectiveType
{
    public const string NAME = "challenge";
    
    protected override void Configure(IDirectiveTypeDescriptor descriptor)
    {
        descriptor.Name("challenge");
        descriptor.Description("If this field could cause a challenge to be issued");
        descriptor.Location(DirectiveLocation.InputObject | DirectiveLocation.Object | DirectiveLocation.FieldDefinition);
    }
}

public static class ChallengeObjectFieldDescriptorExtension
{
    public static IObjectFieldDescriptor UseChallengeDirective(
        this IObjectFieldDescriptor descriptor)
    {
        return descriptor.Directive<ChallengeDirectiveType>();
    }
}

public class ChallengeAttribute : ObjectFieldDescriptorAttribute
{
    public ChallengeAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }

    public override void OnConfigure(IDescriptorContext context,
        IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.UseChallengeDirective();
    }
}