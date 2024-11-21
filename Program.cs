using ValourChain;
using ValourChain.Services;
using ValourChain.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Load NodeSettings
var nodeSettings = builder.Configuration.GetSection("NodeSettings").Get<NodeSettings>();
if (nodeSettings is null || nodeSettings.NodeLocation == "NOT_SET")
{
    Console.WriteLine("Node location is not set or config is missing. Please set the NodeUri in NodeSettings in appsettings.json.");
    return;
}

builder.Services.AddSingleton(nodeSettings);
builder.Services.AddHttpClient();

builder.Services.AddSingleton(typeof(ValourChain.Logger<>));
builder.Services.AddSingleton<NodeRegistryService>();

builder.Services.AddHostedService<NodeConnectionWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("ping" , () => "pong");

_ = app.RunAsync();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var commandProcessor = new CommandProcessor(scopeFactory);

// Use a CancellationTokenSource to signal shutdown
var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;

AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
{
    Console.WriteLine("\nProcess is exiting. Cleaning up resources...");
    cancellationTokenSource.Cancel(); // Signal cancellation
};

Console.CancelKeyPress += (sender, e) =>
{
    Console.WriteLine("\nControl+C pressed. Cleaning up resources...");
    e.Cancel = true; // Prevent immediate termination
    cancellationTokenSource.Cancel(); // Signal cancellation
};

try
{
    while (!cancellationToken.IsCancellationRequested)
    {
        if (Console.KeyAvailable) // Non-blocking check for user input
        {
            var input = Console.ReadLine();
            if (input is not null)
            {
                await commandProcessor.ProcessCommand(input);
            }
        }
        else
        {
            await Task.Delay(100, cancellationToken); // Prevent busy-waiting
        }
    }
}
catch (OperationCanceledException)
{
    // Handle graceful exit
    Console.WriteLine("Operation canceled. Exiting gracefully...");
}
finally
{
    // Perform any cleanup here if needed
    Console.WriteLine("Cleanup complete.");
}