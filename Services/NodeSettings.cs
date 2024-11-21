namespace ValourChain.Services;

public class NodeSettings
{
    public static NodeSettings? Current;
    
    public NodeSettings()
    {
        Current = this;
    }
    
    public string Name { get; set; } = "Unnamed Node";
    public string Description { get; set; } = "A Valour Chain node.";
    public string NodeLocation { get; set; } = "NOT_SET";
    public string WalletAddress { get; set; } = "NOT_SET";
}