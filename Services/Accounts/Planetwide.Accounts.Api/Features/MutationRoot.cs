namespace Planetwide.Accounts.Api.Features;

/// <summary>
/// This entity acts as our root node, from here we hang each of mutations off it via annotations
/// </summary>
public class MutationRoot
{
    public string Version => "Demo";
}