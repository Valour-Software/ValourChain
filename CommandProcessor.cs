using ValourChain.Services;

namespace ValourChain;

public class CommandProcessor
{
    private readonly Dictionary<string, Command> _commands;
    private readonly Logger<CommandProcessor> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public CommandProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _logger = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<Logger<CommandProcessor>>();
        _logger.Log("Command processor initialized.");
        
        _commands = new()
        {
            { "help", new("Help", "help", Help) },
            { "addnode", new Command("Add Node", "addnode <ip/url>", AddNode)}
        };
    }

    public class Command
    {
        public string Name { get; set; }
        public string Usage { get; set; }
        public Func<Task> Action { get; set; }
        
        public Command(string name, string usage, Func<Task> action)
        {
            Name = name;
            Usage = usage;
            Action = action;
        }
    }
    
    public async Task ProcessCommand(string input)
    {
        var commandParts = input.Split(' ');
        var commandName = commandParts[0].ToLower();
        
        _commands.TryGetValue(commandName, out var command);

        if (command is null)
        {
            _logger.LogWarning("Command not found: " + commandName);
            return;
        }

        try
        {
            await command.Action();
        }
        catch (Exception e)
        {
            _logger.LogError($"Error executing command \"{commandName}\": " + e.Message);
        }
    }
    
    private Task Help()
    {
        Console.WriteLine("Available commands:");
        foreach (var command in _commands)
        {
            Console.WriteLine($"{command.Key}\n- Usage: {command.Value.Usage}");
        }
        
        return Task.CompletedTask;
    }

    private async Task AddNode()
    {
        using var scope = _scopeFactory.CreateScope();
        var nodeRegistryService = scope.ServiceProvider.GetRequiredService<NodeRegistryService>();
    }
}