using Microsoft.AspNetCore.Mvc;
using ValourChain.Models;
using ValourChain.Services;

namespace ValourChain.Api;

public static class NetworkApi
{
    public static void MapRoutes(WebApplication app)
    {
        app.MapPost("network/connect", RequestConnection);
        app.MapGet("network/info", RequestInfo);
    }
    
    /// <summary>
    /// Called by a node to request a connection to another node. The node should send its own information in the body.
    /// </summary>
    private static async Task RequestConnection([FromBody] Node node)
    {
        
    }

    /// <summary>
    /// Called by a node to get information about another node.
    /// </summary>
    private static Node RequestInfo(NodeSettings nodeSettings)
    {
        return new Node()
        {
            Name = nodeSettings.Name,
            Description = nodeSettings.Description,
            Location = nodeSettings.NodeLocation,
            LastSeen = DateTime.UtcNow,
            WalletAddress = nodeSettings.WalletAddress
        };
    }
}