using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DiscordbotTest7.Core.Commands;
using Victoria.Node;

namespace DiscordbotTest7.Core.Managers
{
    public static class EventManager
    {
        private static LavaNode _lavaNode = ServiceManager.Provider.GetRequiredService<LavaNode>();
        private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
        private static CommandService _commandService = ServiceManager.GetService<CommandService>();

        public static Task LoadCommands()
        {
            _client.Log += message =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t({message.Source})\t({message.Message})");
                return Task.CompletedTask;
            };

            _commandService.Log += message =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t({message.Source})\t({message.Message})");
                return Task.CompletedTask;
            };

            _client.Ready += ready;
            _client.MessageReceived += OnMessageRecieved;
            _client.SlashCommandExecuted += SlashCommands.SlashCommandHandler;

            return Task.CompletedTask;
        }

        private async static Task OnMessageRecieved(SocketMessage arg)
        {
            Console.WriteLine($"[{DateTime.Now}]\t{arg.ToString()}");
            var argPos = 0;
            var message = arg as SocketUserMessage;
            if (message == null) return;
            var context = new SocketCommandContext(_client, message);

            if (message.Author.IsBot || message.Channel is IDMChannel) return;

            //Console.WriteLine($"[{DateTime.Now}]\t{message.HasCharPrefix('?', ref argPos).ToString()}");

            if (!(message.HasCharPrefix(ConfigManager.Config.Prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var result = await _commandService.ExecuteAsync(context,argPos,ServiceManager.Provider);

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand) return;
            }
        }

        private static async Task ready()
        {
            await Onready();
            await SlashCommands.Client_Ready();
        }

        private static async Task Onready()
        {
            try
            {
                await _lavaNode.ConnectAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            _lavaNode.OnTrackEnd += AudioManager.TrackEnded;
            _lavaNode.OnTrackStuck += AudioManager.OnTrackStuckAsync;
            _lavaNode.OnTrackException += AudioManager.OnTrackExceptionAsync;

            Console.WriteLine($"[{DateTime.Now}]\t(READY)\tBot is ready");
            await _client.SetStatusAsync(Discord.UserStatus.Online);
            await _client.SetGameAsync($"Prefix: {ConfigManager.Config.Prefix}", null, ActivityType.Listening);
        }
    }
}
