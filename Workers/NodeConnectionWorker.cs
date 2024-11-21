using ValourChain.Services;

namespace ValourChain.Workers;

/// <summary>
/// The NodeConnectionWorker is responsible for managing the connections between nodes.
/// It will connect to new nodes, disconnect from old nodes, and manage the latency between nodes.
/// </summary>
public class NodeConnectionWorker : IHostedService, IDisposable
{
    private readonly NodeRegistryService _nodeRegistryService;
    private readonly Logger<NodeConnectionWorker> _logger;
    
    public NodeConnectionWorker(NodeRegistryService nodeRegistryService, Logger<NodeConnectionWorker> logger)
    {
        _nodeRegistryService = nodeRegistryService;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Log("Starting worker...");
        await _nodeRegistryService.LoadAllNodes();
    }

    /// <summary>
    /// Uses stored known-nodes.json to connect to known nodes.
    /// </summary>
    public async Task ConnectToKnown()
    {
        
    }
    
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        
    }
}