using System.Collections.Concurrent;
using System.Text.Json;
using ValourChain.Models;

namespace ValourChain.Services;

public class NodeRegistryService
{
    const string NodeDataFolder = "NodeData";
    
    /// <summary>
    /// Nodes that this node knows of.
    /// </summary>
    private readonly ConcurrentDictionary<string, Node> _allNodes = new();
    
    /// <summary>
    /// Nodes this node is connected to.
    /// </summary>
    private readonly ConcurrentDictionary<string, Node> _connectedNodes = new();
    
    private readonly Logger<NodeRegistryService> _logger;
    private readonly NodeSettings _nodeSettings;
    private HttpClient _http;

    public NodeRegistryService(Logger<NodeRegistryService> logger, NodeSettings nodeSettings, HttpClient http)
    {
        _logger = logger;
        _nodeSettings = nodeSettings;
        _http = http;

        // Ensure NodeData folder exists
        if (!Directory.Exists(NodeDataFolder))
        {
            _logger.LogWarning("NodeData folder does not exist. Creating...");
            try
            {
                Directory.CreateDirectory(NodeDataFolder);
            } catch (Exception e)
            {
                _logger.LogError($"Failed to create NodeData folder: {e.Message} \n Check that Valour Chain has permission to write to the filesystem.");
            }

            _logger.LogSuccess("NodeData folder created.");
        }
        
        _logger.Log($"We are node {_nodeSettings.Name} at {_nodeSettings.NodeLocation}");
    }
    
    /// <summary>
    /// Ensures a node is valid and registers it using the uri.
    /// </summary>
    public async Task RegisterNode(string location, string? id = null)
    {
        Node? node = null;
        
        // Reach out to the node for node info

        var tries = 0;
        
        // Try HTTPS first
        while (node is null && tries < 3)
        {
            try
            {
                node = await _http.GetFromJsonAsync<Node>("https://" + location + "/network/info");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to reach node {location} via HTTPS: {e.Message}");
            }
            
            tries++;
        }
        
        tries = 0;

        // Try HTTP if HTTPS fails
        while (node is null && tries < 3)
        {
            try
            {
                node = await _http.GetFromJsonAsync<Node>("http://" + location + "/network/info");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to reach node {location} via HTTP: {e.Message}");
            }
            
            tries++;
        }
        
        if (node is null)
        {
            _logger.LogError("Failed to reach node " + location);
            return;
        }
        
        // If we gave an ID we already had for the node, set it. 
        if (id is not null)
        {
            node.Id = id;
        }

        _logger.Log("Registering node: " + node.Id);
        _allNodes[node.Id] = node;
        
        // Save to Nodes folder
        _logger.LogSuccess("Registered node: " + node.Id);
    }
    
    public async Task TrustNode(Node node)
    {
        node.BlockReason = null;
        node.Trusted = true;
        
        await SaveNode(node);
    }
    
    public async Task BlockNode(Node node, NodeBlockReason reason)
    {
        node.Trusted = false;
        node.BlockReason = reason;
        
        await SaveNode(node);
    }
    
    public ICollection<Node> GetAllNodes()
    {
        return _allNodes.Values;
    }

    public async Task SaveNode(Node node)
    {
        var json = JsonSerializer.Serialize(node);
        await File.WriteAllTextAsync($"{NodeDataFolder}/{node.Id}.json", json);
    }
    
    public async Task<Node?> LoadNode(string id)
    {
        var json = await File.ReadAllTextAsync($"{NodeDataFolder}/{id}.json");
        return JsonSerializer.Deserialize<Node>(json);
    }
    
    public async Task LoadAllNodes()
    {
        var files = Directory.GetFiles(NodeDataFolder);
        _logger.Log($"Read {files.Length} node files.");
        foreach (var file in files)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var node = JsonSerializer.Deserialize<Node>(json);
                if (node is null)
                {
                    _logger.LogWarning($"Failed to load node from {file}");
                    continue;
                }
                
                await RegisterNode(node.Location);
            } catch (Exception e)
            {
                _logger.LogError($"Error loading node from {file}: {e.Message}");
            }
        }
    }
}