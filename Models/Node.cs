namespace ValourChain.Models;

public enum NodeTyle
{
    Validator,
    FullNode,
    LightNode
}

public class Node
{
    /// <summary>
    /// The uri of a node is where it can be accessed by other nodes.
    /// </summary>
    public required string Uri { get; set; }
    
    /// <summary>
    /// The wallet address of a node is where it can receive awards and where credits are staked.
    /// </summary>
    public required string WalletAddress { get; set; }
    
    /// <summary>
    /// The name of a node is for human-readable identification.
    /// </summary>
    public string Name { get; set; } = "Unnamed Node";
    
    /// <summary>
    /// The description of a node is for human-readable information.
    /// </summary>
    public string Description { get; set; } = "A Valour Chain node.";
    
    /// <summary>
    /// The last recorded network latency of the node.
    /// </summary>
    public float Latency { get; set; }
    
    /// <summary>
    /// The last time the node has been communicated with.
    /// </summary>
    public DateTime LastSeen { get; set; }
}