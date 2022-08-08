using System.Diagnostics;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Language.Visitors;
using HotChocolate.Types;
using HotChocolate.Types.Introspection;
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

    protected override ISyntaxVisitorAction Enter(SelectionSetNode node, IDocumentValidatorContext context)
    {
        if (context.Types.TryPeek(out var type) &&
            type.NamedType() is IComplexOutputType ot)
        {
            return HasChallengeDirective(ot.Directives);
        }
        
        return base.Enter(node, context);
    }

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
            var action = HasChallengeDirective(of.Directives);
            context.OutputFields.Push(of);
            context.Types.Push(of.Type);
            return action;
        }

        return Continue;
    }
    
    protected override ISyntaxVisitorAction Leave(
        FieldNode node,
        IDocumentValidatorContext context)
    {
        context.Types.Pop();
        context.OutputFields.Pop();
        return Continue;
    }
    
    private ISyntaxVisitorAction HasChallengeDirective(IDirectiveCollection directiveCollection)
    {
        if (directiveCollection.Any(x => x.Name == ChallengeDirectiveType.NAME))
        {
            ChallengeRequired = true;
            return Break;
        }
        
        return Continue;
    }
    
}