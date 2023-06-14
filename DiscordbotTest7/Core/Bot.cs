using Discord.WebSocket;
using Discord.Commands;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Victoria;
using DiscordbotTest7.Core.Managers;
using DiscordbotTest7.Core.Commands;
using Victoria.Node;
using Microsoft.Extensions.Logging;

namespace DiscordbotTest7.Core
{
    public class Bot
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
         

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = Discord.LogSeverity.Debug,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            _commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel= Discord.LogSeverity.Debug,
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = true
                
            }) ;

            var collection = new ServiceCollection();

            collection.AddLogging();
            collection.AddSingleton(_client);
            collection.AddSingleton(_commandService);
            collection.AddSingleton<NodeConfiguration>();
            collection.AddSingleton<LavaNode>();
            collection.AddLavaNode(x =>
            {
                x.SelfDeaf = false;
                x.SocketConfiguration = new Victoria.WebSocket.WebSocketConfiguration { BufferSize = 1200 };
            });
            

            ServiceManager.SetProvider(collection);

        }
        public async Task MainAsync()
        {
            if (string.IsNullOrWhiteSpace(ConfigManager.Config.Token)) return;
            await CommandManager.LoadCommandsAsync();
            await EventManager.LoadCommands();
            await _client.LoginAsync(TokenType.Bot, ConfigManager.Config.Token);
            await _client.StartAsync();

            AudioManager.loopPlaylist = false;
            AudioManager.loop = false;
            AudioManager.writePlaying = true;

            await Task.Delay(-1);
        }
    } 
}
