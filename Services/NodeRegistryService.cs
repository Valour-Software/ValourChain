using System.Collections.Concurrent;
using ValourChain.Models;

namespace ValourChain;

public class NodeRegistryService
{
    private readonly ConcurrentBag<Node> _allNodes = new();

    public NodeRegistryService()
    {
        
    }
    
    public void RegisterNode(Node node)
    {
        _allNodes.Add(node);
    }
    
    public List<Node> GetAllNodes()
    {
        return _allNodes.ToList();
    }
}