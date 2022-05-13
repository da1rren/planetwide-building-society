namespace Planetwide.Members.Api.Features;

/// <summary>
/// This entity acts as our root node, from here we hang each of entities off it via queries
/// </summary>
public class QueryRoot
{
    public string Version => "Demo";
}