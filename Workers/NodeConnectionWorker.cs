namespace ValourChain.Workers;

/// <summary>
/// The NodeConnectionWorker is responsible for managing the connections between nodes.
/// It will connect to new nodes, disconnect from old nodes, and manage the latency between nodes.
/// </summary>
public class NodeConnectionWorker : IHostedService, IDisposable
{
    private readonly NodeRegistryService _nodeRegistryService;

    public NodeConnectionWorker(NodeRegistryService nodeRegistryService)
    {
        _nodeRegistryService = nodeRegistryService;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        
    }
}