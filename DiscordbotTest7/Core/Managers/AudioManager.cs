using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Victoria;
using Victoria.Node;
using Victoria.Node.EventArgs;
using Victoria.Player;
using Victoria.Responses.Search;

namespace DiscordbotTest7.Core.Managers
{
    public static class AudioManager
    {
        static IEnumerable<string> songsFolder = Directory.EnumerateDirectories("D:\\osu!\\Songs");
        public static string? playlist { get; private set; }
        public static bool writePlaying { get; set; }
        public static bool loopPlaylist { get; set; }
        public static bool loop { get; set; }
        private static bool fixVanDeEeuw;
        private static readonly LavaNode _lavaNode = ServiceManager.Provider.GetRequiredService<LavaNode>();

        public static async Task<string> JoinAsync(IGuild guild, IVoiceState voiceState, ITextChannel channel)
        {
            if (_lavaNode.HasPlayer(guild)) return "Is already connected to a voicechannel";

            if (voiceState.VoiceChannel is null) return "You must be connected to a voicechannel!";

            try
            {
                if (playlist != null)
                    playlist = null;
                await _lavaNode.JoinAsync(voiceState.VoiceChannel, channel);
                if (await voiceState.VoiceChannel.GetUserAsync(1002994967154802851) != null)
                    return $"Has entered {voiceState.VoiceChannel.Name}";
                else
                {
                    try
                    {
                        await _lavaNode.LeaveAsync(voiceState.VoiceChannel);
                    }
                    catch (Exception ex)
                    {
                    }
                    return "Failed to join channel.";
                }
                    
            }
            catch (Exception ex)
            {
                return $"ERROR\n{ex.Message}";
            }

        }
        public static async Task<string> PlayAsync(SocketGuildUser user, IGuild guild, string searchQuery, ITextChannel channel)
        {
            string RetMsg = null;
            if (fixVanDeEeuw) fixVanDeEeuw = false;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return "Please provide search terms.";
            }

            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                var voiceState = user as IVoiceState;
                if (voiceState?.VoiceChannel == null)
                {
                    return "You must be connected to a voice channel!";
                }

                try
                {
                    player = await _lavaNode.JoinAsync(voiceState.VoiceChannel, channel);
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
            
            SearchResponse searchResponse = new SearchResponse();

            if (searchQuery[0] == 'h' && searchQuery[1] == 't' && searchQuery[2] == 't')
            {
                
                searchResponse = await _lavaNode.SearchAsync(Uri.IsWellFormedUriString(searchQuery, UriKind.Absolute) ? SearchType.Direct : SearchType.YouTube, searchQuery);
                Console.WriteLine(searchResponse.ToString());
            }
            else if ((searchQuery[0] == 'D' && searchQuery[1] == ':') || (searchQuery[0] == 'C' && searchQuery[1] == ':'))
            {
                searchResponse = await _lavaNode.SearchAsync(SearchType.Direct, searchQuery);
                Console.WriteLine(searchResponse.ToString());
            }
            else
            { 
                searchResponse = await _lavaNode.SearchAsync(SearchType.YouTube, searchQuery);
                if (searchResponse.Status == SearchStatus.NoMatches)
                {
                    searchResponse = await _lavaNode.SearchAsync(SearchType.SoundCloud, searchQuery);
                }
                Console.WriteLine(searchResponse.ToString());
            }

            if (searchResponse.Status is SearchStatus.LoadFailed or SearchStatus.NoMatches)
            {
                return $"I wasn't able to find anything for `{searchQuery}`.";
            }

            if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
            {
                player.Vueue.Enqueue(searchResponse.Tracks);
                RetMsg = ($"Envueued {searchResponse.Tracks.Count} songs.");
            }
            else
            {
                var track = searchResponse.Tracks.FirstOrDefault();
                player.Vueue.Enqueue(track);
                RetMsg = ($"Envueued {track?.Title}\t `{track?.Duration}`");
            }

            if (player.PlayerState is PlayerState.Playing or PlayerState.Paused)
            {
                return RetMsg;
            }

            player.Vueue.TryDequeue(out var lavaTrack);
            await player.PlayAsync(lavaTrack);
            return $"now playing {lavaTrack.Title}";
        }
        public static async Task<string> LeaveAsync(IGuild guild)
        {
            try
            {
                playlist = null;
                _lavaNode.TryGetPlayer(guild, out var player);
                if (player.PlayerState is PlayerState.Playing) await player.StopAsync();
                await _lavaNode.LeaveAsync(player.VoiceChannel);

                Console.WriteLine($"[{DateTime.Now}]\t(AUDIO)\tBot has left a voicechannel");
                return "Left the voicechannel!";
            }
            catch (InvalidOperationException ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public static async Task ConnectAsync()
        {
            try
            {
                await _lavaNode.ConnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public static async Task<string> PauseAsync(IGuild guild)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                return "I cannot pause when I'm not playing anything!";
            }

            try
            {
                await player.PauseAsync();
                return $"Paused: {player.Track.Title}";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        public static async Task<string> ResumeAsync(IGuild guild)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (player.PlayerState != PlayerState.Paused)
            {
                return "I cannot resume when I'm not playing anything!";
            }

            try
            {
                await player.ResumeAsync();
                return $"Resumed: {player.Track.Title}";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        public static async Task<string> StopAsync(IGuild guild)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (player.PlayerState == PlayerState.Stopped)
            {
                return "Woaaah there, I can't stop the stopped forced.";
            }

            try
            {
                playlist = null;
                await player.StopAsync();
                if (player.Vueue.Count > 0)
                {
                    player.Vueue.Clear();
                }
                return "No longer playing anything.";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        public static async Task<string> SkipAsync(IGuild guild)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                return "Woaaah there, I can't skip when nothing is playing.";
            }

            if (player.Vueue.Count < 1)
            {
                if (loop) { loop = false; }
                playlist = null;
                var track = player.Track;
                await player.StopAsync();
                return $"Skipped: {track.Title}";
            }

            try
            {
                // just in case it somehow makes it through the other statement.
                if (player.Vueue.Count <= 1)
                {
                    if (loop) { loop = false; }
                }
                var (skipped, currenTrack) = await player.SkipAsync(); fixVanDeEeuw = true;
                return $"Skipped: {skipped.Title}\nNow Playing: {currenTrack.Title}";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        public static async Task QueueAsync(IGuild guild, ITextChannel channel)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                await channel.SendMessageAsync("Can't show the *'Vueue'* when i'm not playing"); return;
            }

            try
            {
                if (player.Vueue.Count < 1)
                {
                    await channel.SendMessageAsync("No tracks in the queue");
                    return;
                }
                    
                await channel.SendMessageAsync($"Now showing All {player.Vueue.Count} tracks in *'Vueue'*");
                int i = 0;
                string formatted = "";
                foreach (var item in player.Vueue)
                {
                    i++;
                    formatted += $"Track {i} = {item.Title}\t `{item.Duration}`\n";
                    if (formatted.Length >= 1850)
                    {
                        await channel.SendMessageAsync(formatted);
                        formatted = "";
                    }
                }
                await channel.SendMessageAsync(formatted);
                return;
            }
            catch (Exception ex)
            {
                await channel.SendMessageAsync(ex.Message);
                return;
            }
        }
        public static async Task<string> SeekAsync(string t, IGuild guild)
        {
            TimeSpan timeSpan = TimeSpan.Parse(t);

            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (player.PlayerState != PlayerState.Playing)
            {
                return "Woaaah there, I can't seek when nothing is playing.";
            }

            try
            {
                await player.SeekAsync(timeSpan);
                return $"I've seeked `{player.Track.Title}` to {timeSpan}.";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        public static async Task<string> VolumeAsync(ushort volume, IGuild guild)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            try
            {
                await player.SetVolumeAsync(volume);
                return $"I've changed the player volume to {volume}.";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        public static async Task<string> ShuffleAsync(IGuild guild)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            try
            {
                player.Vueue.Shuffle();
                return "Shuffled the *'Vueue'*!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static async Task<string> GotoAsync(IGuild guild, int pos)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (pos > player.Vueue.Count) return "Not that many tracks in the *'Vueueu'*";

            try
            {
                LavaTrack t = player.Vueue.ElementAt(pos -1);
                fixVanDeEeuw = true;
                player.Vueue.TryDequeue(out t);
                await player.PlayAsync(t);
                return $"Now playing: *{t.Title}* by *{t.Author}*";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        public static async Task<string> GotoAsync(IGuild guild, string title)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            try
            {
                LavaTrack t = new LavaTrack();
                foreach (var track in player.Vueue)
                {
                    if (track.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                    {
                        t = track;
                        break;
                    }
                    else
                    {
                        string[] a = title.Split(' ');
                        int count = 0;
                        for (int i =0; i < a.Length; i++)
                        {
                            if (track.Title.Contains(a[i], StringComparison.OrdinalIgnoreCase))
                            {
                                count++;
                            }
                        }
                        if (count == a.Length)
                        {
                            t = track;
                            break;
                        }
                    }
                }

                if (t.Title is null || t.Title == "") return $"Couldn't find {title} in queue";

                
                fixVanDeEeuw = true;
                player.Vueue.TryDequeue(out t);
                await player.PlayAsync(t);
                return $"Now playing: *{t.Title}* by *{t.Author}*";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        public static async Task createplaylistAsync(IGuild guild, ITextChannel textChannel, string playlist)
        {
            if (Directory.Exists("Resources"))
            {
                created:;
                if (Directory.Exists($"Resources/{guild.Id}"))
                {
                    if (!File.Exists($"Resources/{guild.Id}/{playlist}.txt"))
                    {
                        File.Create($"Resources/{guild.Id}/{playlist}.txt").Close();
                        await textChannel.SendMessageAsync($"Created {playlist}");
                    }
                    else
                    {
                        await textChannel.SendMessageAsync("That file already exists");
                    }
                }
                else
                {
                    Directory.CreateDirectory($"Resources/{guild.Id}");
                    goto created;
                }
            }
            else
                await textChannel.SendMessageAsync("Error 404");
        }
        public static async Task playplaylistAsync(SocketGuildUser user, IGuild guild, ITextChannel textChannel, string playlist)
        {
            if (Directory.Exists($"Resources/{guild.Id}"))
            {
                if (!File.Exists($"Resources/{guild.Id}/{playlist}.txt"))
                {
                    await textChannel.SendMessageAsync("That file does not exist");
                }
                else
                {
                    AudioManager.playlist = playlist;
                    string[] lines = File.ReadAllLines($"Resources/{guild.Id}/{playlist}.txt");
                    foreach (string str in lines)
                    {
                        _lavaNode.TryGetPlayer(guild, out var player);
                        if (player != null)
                        {
                            if (player.PlayerState is PlayerState.Playing)
                            {
                                if (str != null)
                                    await PlayAsync(user, guild, str, textChannel);
                            }
                        }
                        else
                        {
                            #region fuck zanger rinus
                            if (string.IsNullOrWhiteSpace(str))
                            {
                                return;
                            }


                            var voiceState = user as IVoiceState;
                            if (voiceState?.VoiceChannel == null)
                            {
                                return;
                            }

                            try
                            {
                                player = await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
                            }
                            catch (Exception exception)
                            {
                                return;
                            }

                            SearchResponse searchResponse = new SearchResponse();

                            if (str[0] == 'h' && str[1] == 't' && str[2] == 't')
                            {
                                searchResponse = await _lavaNode.SearchAsync(Uri.IsWellFormedUriString(str, UriKind.Absolute) ? SearchType.Direct : SearchType.YouTube, str);
                                Console.WriteLine(searchResponse.ToString());
                            }
                            else
                            {
                                searchResponse = await _lavaNode.SearchAsync(SearchType.YouTube, str);
                                if (searchResponse.Status == SearchStatus.NoMatches)
                                {
                                    await textChannel.SendMessageAsync("Couldn't find anything on Youtube now trying Soundcloud");
                                    searchResponse = await _lavaNode.SearchAsync(SearchType.SoundCloud, str);
                                }
                                Console.WriteLine(searchResponse.ToString());
                            }

                            if (searchResponse.Status is SearchStatus.LoadFailed or SearchStatus.NoMatches)
                            {
                                return;
                            }

                            if (!string.IsNullOrWhiteSpace(searchResponse.Playlist.Name))
                            {
                                player.Vueue.Enqueue(searchResponse.Tracks);
                                if (writePlaying)
                                    await textChannel.SendMessageAsync($"Envueued {searchResponse.Tracks.Count} songs.");
                            }
                            else
                            {
                                var track = searchResponse.Tracks.FirstOrDefault();
                                player.Vueue.Enqueue(track);
                                if (writePlaying)
                                    await textChannel.SendMessageAsync($"Envueued {track?.Title}\t `{track?.Duration}`");
                            }
                            player.Vueue.TryDequeue(out var lavaTrack);
                            await player.PlayAsync(lavaTrack);
                            continue;
                            #endregion
                        }
                    }
                }
            }
            else
                await textChannel.SendMessageAsync("Error 404");
        }
        public static async Task<string> ListPlaylists(IGuild guild, ITextChannel textChannel)
        {
            if (Directory.Exists($"Resources/{guild.Id}"))
            {
                string[] playlists = Directory.GetFileSystemEntries($"Resources/{guild.Id}");
                string msg = "";
                for (int i = 0; i < playlists.Length; i++)
                {
                    string[] tmp = playlists[i].Split('\\');
                    string[] results = tmp[1].Split('.');
                    msg += results[0] + "\n";
                }
                return msg;
            }
            return "Error 404";
        }
        public static async Task removefromAsync(IGuild guild, ITextChannel textChannel, string playlist, string name)
        {
            if (Directory.Exists($"Resources/{guild.Id}"))
            {
                if (!File.Exists($"Resources/{guild.Id}/{playlist}.txt"))
                {
                    await textChannel.SendMessageAsync("That playlist does not exist");
                }
                else
                {
                    int count = 0;
                    string[] lines = File.ReadAllLines($"Resources/{guild.Id}/{playlist}.txt");
                    foreach (string item in lines)
                    {
                        if (item.ToLower() == name.ToLower() || item.ToLower().Contains(name.ToLower()))
                            goto found;
                        count++;
                    }
                found:;
                    lines[count] = null;
                    File.WriteAllLines($"Resources/{guild.Id}/{playlist}.txt" ,lines.ToArray());
                }
            }
            else
                await textChannel.SendMessageAsync("Error 404");
        }
        public static async Task addtoAsync(IGuild guild, ITextChannel textChannel, string playlist, string? name, SocketGuildUser user)
        {
            if (Directory.Exists($"Resources/{guild.Id}"))
            {
                if (!File.Exists($"Resources/{guild.Id}/{playlist}.txt"))
                {
                    await textChannel.SendMessageAsync("That playlist does not exist");
                }
                else
                {

                    if (name != null)
                    {
                        File.AppendAllText($"Resources/{guild.Id}/{playlist}.txt", name + Environment.NewLine);
                        if (AudioManager.playlist == playlist)
                        {
                            if (!_lavaNode.TryGetPlayer(guild, out var player))
                            {
                                await textChannel.SendMessageAsync("I'm not connected to a voice channel.");
                                return;
                            }
                            await PlayAsync(user, guild, name, textChannel);
                            return;
                        }
                    }

                    else
                    {

                        if (!_lavaNode.TryGetPlayer(guild, out var player))
                        {
                            await textChannel.SendMessageAsync("I'm not connected to a voice channel.");
                            return;
                        }
                        File.AppendAllText($"Resources/{guild.Id}/{playlist}.txt", player.Track.Title.ToString() + Environment.NewLine);
                        await textChannel.SendMessageAsync($"Added {player.Track.Title.ToString()} to {playlist}");
                    }
                }
            }
            else
                await textChannel.SendMessageAsync("Error 404");
        }
        public static async Task addtoAsync(IGuild guild, ITextChannel textChannel, string? name, SocketGuildUser user)
        {
            if (playlist == null) 
                return;

            if (Directory.Exists($"Resources/{guild.Id}"))
            {
                if (!_lavaNode.TryGetPlayer(guild, out var player))
                {
                    await textChannel.SendMessageAsync("I'm not connected to a voice channel.");
                    return;
                }
                if (!File.Exists($"Resources/{guild.Id}/{playlist}.txt"))
                {
                    await textChannel.SendMessageAsync("That playlist does not exist");
                }
                else
                {

                    if (name != null)
                        File.AppendAllText($"Resources/{guild.Id}/{playlist}.txt", name + Environment.NewLine);
                    else
                    {
                        
                        File.AppendAllText($"Resources/{guild.Id}/{playlist}.txt", player.Track.Title.ToString() + Environment.NewLine);
                        await textChannel.SendMessageAsync($"Added {player.Track.Title.ToString()} to {playlist}");
                    }
                }
            }
            else
                await textChannel.SendMessageAsync("Error 404");
        }
        public static async Task<string> ClearAsync(IGuild guild)
        {
            playlist = null;
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (player.Vueue.Count < 1)
            {
                return "No tracks in the queue";
            }
            try
            {
                player.Vueue.Clear();
                return "Cleared the queue";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public static async Task<string> RemoveAsync(IGuild guild, string title)
        {
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            try
            {
                LavaTrack t = new LavaTrack();
                foreach (var track in player.Vueue)
                {
                    if (track.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                    {
                        t = track;
                        break;
                    }
                    else
                    {
                        string[] a = title.Split(' ');
                        int count = 0;
                        for (int i = 0; i < a.Length; i++)
                        {
                            if (track.Title.Contains(a[i], StringComparison.OrdinalIgnoreCase))
                            {
                                count++;
                            }
                        }
                        if (count == a.Length)
                        {
                            t = track;
                            break;
                        }
                    }
                }

                if (t.Title is null || t.Title == "") return $"Couldn't find {title} in queue";


                fixVanDeEeuw = true;
                player.Vueue.Remove(t);
                return $"Removed: *{t.Title}* by *{t.Author}* from the queue";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static async Task<string> RemoveAsync(IGuild guild, int pos)
        {

            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                return "I'm not connected to a voice channel.";
            }

            if (pos > player.Vueue.Count) return "Not that many tracks in the queue";

            try
            {
                LavaTrack t = player.Vueue.ElementAt(pos-1);
                fixVanDeEeuw = true;
                player.Vueue.Remove(t);
                return $"Removed: *{t.Title}* by *{t.Author}* from queue";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static async Task<string> PlayOsuAsync(IGuild guild, ITextChannel textChannel, SocketGuildUser user, int? rolls)
        {
            Console.WriteLine("in PlayOsu");
            // check if player is connected and connect if not.
            if (!_lavaNode.TryGetPlayer(guild, out var player))
            {
                var voiceState = user as IVoiceState;

                if (voiceState?.VoiceChannel == null)
                {
                    return "You must be connected to a voice channel!";
                }

                try
                {
                    player = await _lavaNode.JoinAsync(voiceState.VoiceChannel, textChannel);
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
            // if no number of rolls is provided set it to roll once.
            if (rolls == null) { rolls = 1; }

            string message = "";

            Random random = new Random();

            for (int i = 0; i < rolls; i++)
            {
                int number = random.Next(songsFolder.Count());

                IEnumerable<string> f = Directory.EnumerateFiles(songsFolder.ElementAt(number));

                var name = from a in f where a.EndsWith(".mp3") select a;

                if (name.Any())
                {
                    SearchResponse searchResponse = new SearchResponse();

                    #region error checking the searchresponse
                    try
                    {
                        searchResponse = await _lavaNode.SearchAsync(SearchType.Direct, name.FirstOrDefault());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{DateTime.Now}] \t{ex.Message}");
                        rolls++;
                        continue;
                    }

                    if (searchResponse.Status is SearchStatus.LoadFailed or SearchStatus.NoMatches)
                    {
                        rolls++;
                        continue;
                    }

                    if (searchResponse.Tracks.Count < 1)
                    {
                        rolls++;
                        continue;
                    }
                    #endregion

                    Console.WriteLine(searchResponse.Tracks.First().Url);

                    // If the player is playing or paused enque the track.
                    if (player.PlayerState == PlayerState.Playing || player.PlayerState == PlayerState.Paused)
                    {
                        try
                        {
                            message += $"Enqueued: {((searchResponse.Tracks.FirstOrDefault().Url.Split("Songs\\"))[1].Split('\\'))[0]} \t `{searchResponse.Tracks.FirstOrDefault().Duration.ToString().Remove(9)}`\n";
                            player.Vueue.Enqueue(searchResponse.Tracks.FirstOrDefault());
                        }
                        catch (Exception ex) { return ex.Message; }
                    } // else play the track.
                    else
                    {
                        try
                        {
                            player.Vueue.Enqueue(searchResponse.Tracks.FirstOrDefault());
                            player.Vueue.TryDequeue(out LavaTrack track);
                            await player.PlayAsync(track);
                            message += $"Now playing: {((track.Url.Split("Songs\\"))[1].Split('\\'))[0]} \t `{track.Duration.ToString().Remove(9)}`\n";
                        }
                        catch (Exception ex) { return ex.Message; }
                    }
                }
            }
            // break up the message if it is too long.
            if (message.Length >= 1800)
            {
                return message.Substring(0, 1800);
            }

            return message;
        }
        public static async Task AutoplayAsync()
        {
            _lavaNode.OnTrackEnd += TrackEnded;
        }
        public static Task OnTrackExceptionAsync(TrackExceptionEventArg<LavaPlayer<LavaTrack>, LavaTrack> arg)
        {
            arg.Player.Vueue.Enqueue(arg.Track);
            return arg.Player.TextChannel.SendMessageAsync($"{arg.Track} has been requeued because it threw an exception.");
        }
        public static Task OnTrackStuckAsync(TrackStuckEventArg<LavaPlayer<LavaTrack>, LavaTrack> arg)
        {
            arg.Player.Vueue.Enqueue(arg.Track);
            return arg.Player.TextChannel.SendMessageAsync($"{arg.Track} has been requeued because it got stuck.");
        }
        public static async Task TrackEnded(TrackEndEventArg<LavaPlayer<LavaTrack>, LavaTrack> args)
        {
            Console.WriteLine($"Finished playing: [{args.Track.Title}]");

            if (loop)
            {
                Console.WriteLine($"Now playing: *{args.Track.Title}* by *{args.Track.Author}*");
                await args.Player.PlayAsync(args.Track);
                if (writePlaying)
                    await args.Player.TextChannel.SendMessageAsync($"Now playing: *{args.Track.Title}* by *{args.Track.Author}*");
                return;
            }

            if (args.Reason == TrackEndReason.LoadFailed) return;
           
            if (!args.Player.Vueue.TryDequeue(out var queueable )) return;

            if (fixVanDeEeuw) { fixVanDeEeuw = false; return; }

            if (!(queueable is LavaTrack track))
            {
                await args.Player.TextChannel.SendMessageAsync("Next item in *'Vueue'* is not a track");
                return;
            }

            if (loopPlaylist)
            {
                args.Player.Vueue.Enqueue(args.Track);
            }

            if (args.Player.Vueue.Count < 1 || args.Player.Vueue == null)
                playlist = null;

            await args.Player.PlayAsync(track);

            Console.WriteLine($"Now playing: *{track.Title}* by *{track.Author}*");

            if (writePlaying)
                await args.Player.TextChannel.SendMessageAsync($"Now playing: *{track.Title}* by *{track.Author}*");

            return;
        }
    }
}