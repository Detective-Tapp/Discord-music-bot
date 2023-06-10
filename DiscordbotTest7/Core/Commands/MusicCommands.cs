using Discord.Commands;
using System.Threading.Tasks;
using DiscordbotTest7.Core.Managers;
using Discord;
using Discord.WebSocket;

namespace DiscordbotTest7.Core.Commands
{
    [Name("Music")]
    public class MusicCommands : ModuleBase<SocketCommandContext>
    {
        [Command("join")]
        public async Task JoinCommand()
            => await Context.Channel.SendMessageAsync(await AudioManager.JoinAsync(Context.Guild, Context.User as IVoiceState, Context.Channel as ITextChannel));

        [Command("play")]
        public async Task PlayCommand([Remainder] string search)
            => await Context.Channel.SendMessageAsync(await AudioManager.PlayAsync(Context.User as SocketGuildUser, Context.Guild, search, Context.Channel as ITextChannel));

        [Command("leave")]
        public async Task LeaveCommand()
            => await Context.Channel.SendMessageAsync(await AudioManager.LeaveAsync(Context.Guild));
        
        [Command("pause")]
        public async Task PauseCommand()
            => await Context.Channel.SendMessageAsync(await AudioManager.PauseAsync(Context.Guild));

        [Command("resume")]
        public async Task ResumeCommand()
            => await Context.Channel.SendMessageAsync(await AudioManager.ResumeAsync(Context.Guild));

        [Command("stop")]
        public async Task StopCommand()
            => await Context.Channel.SendMessageAsync(await AudioManager.StopAsync(Context.Guild));

        [Command("skip")]
        public async Task SkipCommand()
            => await Context.Channel.SendMessageAsync(await AudioManager.SkipAsync(Context.Guild));

        [Command("volume")]
        public async Task VolumeCommand(ushort i)
            => await Context.Channel.SendMessageAsync(await AudioManager.VolumeAsync(i, Context.Guild));

        [Command("shuffle")]
        public async Task ShuffleCommand()
            => Context.Channel.SendMessageAsync(await AudioManager.ShuffleAsync(Context.Guild));

        [Command("seek")]
        public async Task SeekCommand(string t)
            => await Context.Channel.SendMessageAsync(await AudioManager.SeekAsync(t, Context.Guild));

        [Command("queue")]
        public async Task QueueCommand()
            => await AudioManager.QueueAsync(Context.Guild, Context.Channel as ITextChannel);

        [Command("goto")]
        public async Task GotoCommand(int i)
            => await Context.Channel.SendMessageAsync(await AudioManager.GotoAsync(Context.Guild, i));
        [Command("goto")]
        public async Task GotoCommand([Remainder] string i)
            => await Context.Channel.SendMessageAsync(await AudioManager.GotoAsync(Context.Guild, i));

        [Command("loop")]
        public async Task LoopCommand()
        {
            if (AudioManager.loop)
            {
                await Context.Channel.SendMessageAsync("Disabled looping");
                AudioManager.loop = false;
                AudioManager.writePlaying = true;
            }
            else
            {
                await Context.Channel.SendMessageAsync("Enabled looping");
                AudioManager.loop = true;
                AudioManager.writePlaying = false;
            }
        }

        [Command("loopplaylist")]
        public async Task LoopPlaylistCommand()
        {
            if (AudioManager.loopPlaylist)
            {
                await Context.Channel.SendMessageAsync("Disabled playlist looping");
                AudioManager.loopPlaylist = false;
                AudioManager.writePlaying = true;
            }
            else
            {
                await Context.Channel.SendMessageAsync("Enabled playlist looping");
                AudioManager.loopPlaylist = true;
                AudioManager.writePlaying = false;
            }
        }
        [Command("verbose")]
        public async Task VerboseCommand()
        {
            if (AudioManager.writePlaying)
            {
                AudioManager.writePlaying = false;
                Console.WriteLine(AudioManager.writePlaying);
                await Context.Channel.SendMessageAsync(AudioManager.writePlaying.ToString());
            }
            else
            {
                AudioManager.writePlaying = true;
                Console.WriteLine(AudioManager.writePlaying);
                await Context.Channel.SendMessageAsync(AudioManager.writePlaying.ToString());
            }
        }
    }

    [Name("Regular")]
    public class RegularCommands : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task Test()
            => Console.WriteLine(Context.Guild.Id);

        [Command("ping")]
        public async Task Ping()
        => await Context.Channel.SendMessageAsync("Pong!"); 

        [Command("connect")]
        [Summary("connects to lavalink")]
        public async Task Connect() 
            => await AudioManager.ConnectAsync();

        [Command("autoplay")]
        public async Task AutoPlay()
            => await AudioManager.AutoplayAsync();

        [Command("banhim")]
        public async Task BanHim(string str)
        {
            IUser user = (IUser)Context.Guild.Users.Where(i => i.DisplayName == str);
            Context.Guild.AddBanAsync(user);
        }

    }
}
