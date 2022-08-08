using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using HotChocolate.Language;
using HotChocolate.Language.Visitors;
using HotChocolate.Types;
using HotChocolate.Types.Introspection;
using HotChocolate.Utilities;
using HotChocolate.Validation;
using Planetwide.Challenge.Api.Infrastructure.Directives;

namespace Planetwide.Challenge.Api.Infrastructure.DocumentValidators;

public class ChallengeVisitor : TypeDocumentValidatorVisitor
{
    public bool ChallengeRequired { get; private set; }
    
    #if DEBUG
    protected override ISyntaxVisitorAction Enter(
        ISyntaxNode node,
        IDocumentValidatorContext context)
    {
        Debug.WriteLine(node.Kind);
        return base.Enter(node, context);
    }
    #endif
    
    protected override ISyntaxVisitorAction Enter(
        FieldNode node,
        IDocumentValidatorContext context)
    {
        if (IntrospectionFields.TypeName.Equals(node.Name.Value, StringComparison.Ordinal))
        {
            return Skip;
        }
        
        if (context.Types.TryPeek(out var type) &&
            type.NamedType() is IComplexOutputType ot &&
            ot.Fields.TryGetField(node.Name.Value, out var of))
        {
            Debug.WriteLine("Found; " +node.Name);
            var action = HasChallengeDirective(of.Directives);
            context.OutputFields.Push(of);
            context.Types.Push(of.Type);
            return action;
        }

        Debug.WriteLine("Couldn't find; " +node.Name);

        context.UnexpectedErrorsDetected = true;
        return Skip;
    }
    
    protected override ISyntaxVisitorAction Leave(
        FieldNode node,
        IDocumentValidatorContext context)
    {
        context.OutputFields.Pop();
        context.Types.Pop();
        return Continue;
    }
    
    private ISyntaxVisitorAction HasChallengeDirective(params IDirectiveCollection[] directiveCollections)
    {
        foreach (var directiveCollection in directiveCollections)
        {
            if (directiveCollection.Any(x => x.Name == ChallengeDirectiveType.NAME))
            {
                ChallengeRequired = true;
                return Break;
            }
        }
        
        return Continue;
    }
}