using Microsoft.AspNetCore.Mvc;
using ValourChain.Models;

namespace ValourChain.Api;

public static class NetworkApi
{
    public static void MapRoutes(WebApplication app)
    {
        app.MapPost("network/connect", RequestConnection);
    }
    
    /// <summary>
    /// Called by a node to request a connection to another node. The node should send its own information in the body.
    /// </summary>
    public static async Task RequestConnection([FromBody] Node node)
    {
        
    }
}