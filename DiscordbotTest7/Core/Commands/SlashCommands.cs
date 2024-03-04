using Discord;
using Discord.Extensions;
using Discord.Rest;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using DiscordbotTest7.Core.Managers;

namespace DiscordbotTest7.Core.Commands
{
    public static class SlashCommands
    {
        private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
        public static async Task Client_Ready()
        {
            #region Global Command Inits
            /*
            var clearGlobalCommand = new SlashCommandBuilder()
               .WithName("clear")
               .WithDescription("Clears the queue");

            var removeGlobalCommand = new SlashCommandBuilder()
               .WithName("remove")
               .WithDescription("Removes a track from the queue")
               .AddOption("querry", ApplicationCommandOptionType.String, "the position or name of the track", isRequired: true);

            
            var listplaylistsGlobalCommand = new SlashCommandBuilder()
               .WithName("listplaylists")
               .WithDescription("Lists all the playlists of this server");
            
            var playGlobalCommand = new SlashCommandBuilder()
               .WithName("play")
               .WithDescription("Searches for or plays the track")
               .AddOption("querry", ApplicationCommandOptionType.String, "the link or name of the track", isRequired: true).Build();
            
            var skipGlobalCommand = new SlashCommandBuilder()
               .WithName("skip")
               .WithDescription("Skips to the next track in queue");
            
            var pauseGlobalCommand = new SlashCommandBuilder()
               .WithName("pause")
               .WithDescription("Pauses the player");
            
            var seekGlobalCommand = new SlashCommandBuilder()
               .WithName("seek")
               .WithDescription("Seeks a position in the track")
               .AddOption("time", ApplicationCommandOptionType.String, "hour:minute:second time format [00:00:00]", isRequired: true);

            var stopGlobalCommand = new SlashCommandBuilder()
               .WithName("stop")
               .WithDescription("Stops the player");

            var shuffleGlobalCommand = new SlashCommandBuilder()
               .WithName("shuffle")
               .WithDescription("Shuffles the queue");

            var resumeGlobalCommand = new SlashCommandBuilder()
               .WithName("resume")
               .WithDescription("Resumes the player");

            var loopGlobalCommand = new SlashCommandBuilder()
               .WithName("loop")
               .WithDescription("Loops the current track");

            var loopPlaylistGlobalCommand = new SlashCommandBuilder()
               .WithName("loopplaylist")
               .WithDescription("Loops the whole queue");

            var verboseGlobalCommand = new SlashCommandBuilder()
               .WithName("verbose")
               .WithDescription("Toggles printing current track in channel");

            var joinGlobalCommand = new SlashCommandBuilder()
               .WithName("join")
               .WithDescription("Joins the VC");

            var queueuGlobalCommand = new SlashCommandBuilder()
               .WithName("queue")
               .WithDescription("Prints all tracks in the queue");

            var leaveGlobalCommand = new SlashCommandBuilder()
               .WithName("leave")
               .WithDescription("Leaves the VC");

            var volumeGlobalCommand = new SlashCommandBuilder()
               .WithName("volume")
               .WithDescription("Sets the Volume of the player")
               .AddOption("volume", ApplicationCommandOptionType.Integer, "A value of 0 to 1000", isRequired: true);

            var gotoGlobalCommand = new SlashCommandBuilder()
               .WithName("goto")
               .WithDescription("Goes to a certain track or position in the queue")
               .AddOption("track", ApplicationCommandOptionType.String, "Name or position of the Track", isRequired: false);
            
            var addtoGlobalCommand = new SlashCommandBuilder()
               .WithName("addto")
               .WithDescription("Adds a certain track to A or The specified playlist")
               .AddOption("querry", ApplicationCommandOptionType.String, "the link or name of the track", isRequired: false)
               .AddOption("playlist", ApplicationCommandOptionType.String, "Name of the playlist", isRequired: true);

            var removefromGlobalCommand = new SlashCommandBuilder()
               .WithName("removefrom")
               .WithDescription("Removes a certain track from playlist")
               .AddOption("querry", ApplicationCommandOptionType.String, "the link or name of the track", isRequired: true)
               .AddOption("playlist", ApplicationCommandOptionType.String, "Name of the playlist", isRequired: true);

            var playplaylistGlobalCommand = new SlashCommandBuilder()
               .WithName("playplaylist")
               .WithDescription("Queues up a playlist")
               .AddOption("playlist", ApplicationCommandOptionType.String, "Name of the playlist", isRequired: true);

            var createplaylistGlobalCommand = new SlashCommandBuilder()
               .WithName("createplaylist")
               .WithDescription("Creates a playlist")
               .AddOption("playlist", ApplicationCommandOptionType.String, "Name of the playlist", isRequired: true);
            
            var addGlobalCommand = new SlashCommandBuilder()
               .WithName("add")
               .WithDescription("Adds a or the current track to the currently playing playlist.")
               .AddOption("querry", ApplicationCommandOptionType.String, "Name of the track or a link", isRequired: false);

            var playOsuGlobalCommand = new SlashCommandBuilder()
               .WithName("playosu")
               .WithDescription("Plays a random osu song from my drive.")
               .AddOption("rolls", ApplicationCommandOptionType.Integer, "number of songs to queue", isRequired: false);*/
            #endregion

            try
            {
                #region Global Command Builds
                /*
                await _client.CreateGlobalApplicationCommandAsync(playGlobalCommand);
                await _client.CreateGlobalApplicationCommandAsync(skipGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(joinGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(gotoGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(pauseGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(queueuGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(resumeGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(loopGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(loopPlaylistGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(stopGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(volumeGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(seekGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(verboseGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(leaveGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(addtoGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(shuffleGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(addtoGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(createplaylistGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(removefromGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(playplaylistGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(listplaylistsGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(clearGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(removeGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(addGlobalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(playOsuGlobalCommand.Build());*/
                #endregion
            }
            catch (HttpException exception)
            {
                Console.WriteLine(exception.Message.ToString());  
            }
        }
        public static async Task SlashCommandHandler(SocketSlashCommand command)
        {
            SocketGuildUser user;
            IGuild guild;
            ITextChannel channel;

            try
            {
                guild = _client.GetGuild(command.GuildId.Value);
                user = command.User as SocketGuildUser;
                channel = command.Channel as ITextChannel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); return;
            }

            Console.WriteLine($"[{DateTime.Now}] \texecuted: {command.Data.Name}");

            switch (command.Data.Name)
            {
                case "play":
                    try
                    {
                        await command.RespondAsync(await AudioManager.PlayAsync(user, guild, command.Data.Options.First().Value.ToString(), channel), ephemeral: true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await command.RespondAsync("Error, sorry bout that.", ephemeral: true);
                    }
                    break;

                case "skip":
                    if (AudioManager.writePlaying)
                        await command.RespondAsync(await AudioManager.SkipAsync(guild), ephemeral: true);
                    else { 
                    await command.RespondAsync(await AudioManager.SkipAsync(guild), ephemeral: true);
                    }
                    break;

                case "pause":
                    await command.RespondAsync(await AudioManager.PauseAsync(guild), ephemeral: true);
                    break;

                case "resume":
                    await command.RespondAsync(await AudioManager.ResumeAsync(guild), ephemeral: true);
                    break;

                case "stop":
                    await command.RespondAsync(await AudioManager.StopAsync(guild), ephemeral: true);
                    break;

                case "leave":
                    await command.RespondAsync(await AudioManager.LeaveAsync(guild), ephemeral: true);
                    break;

                case "volume":
                    await command.RespondAsync(await AudioManager.VolumeAsync(ushort.Parse(command.Data.Options.First().Value.ToString()), guild), ephemeral: true);
                    break;

                case "shuffle":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.ShuffleAsync(guild));
                    break;

                case "seek":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.SeekAsync(command.Data.Options.First().Value.ToString(), guild));
                    break;

                case "goto":
                    command.DeferAsync(ephemeral: true);
                    if (command.Data.Options.First().Value.ToString().Length <= 4)
                        try
                        {
                            int k = Int16.Parse(command.Data.Options.First().Value.ToString());
                            await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.GotoAsync(guild, k));
                        }
                        catch
                        {
                            await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.GotoAsync(guild, command.Data.Options.First().Value.ToString()));
                        }

                    else
                        await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.GotoAsync(guild, command.Data.Options.First().Value.ToString()));
                    break;

                case "loop":
                    if (AudioManager.loop)
                    {
                        await command.RespondAsync("Disabled looping", ephemeral: true);
                        AudioManager.loop = false;
                        AudioManager.writePlaying = true;
                    }
                    else
                    {
                        await command.RespondAsync("Enabled looping", ephemeral: true);
                        AudioManager.loop = true;
                        AudioManager.writePlaying = false;
                    }
                    break;

                case "loopplaylist":
                    if (AudioManager.loopPlaylist)
                    {
                        await command.RespondAsync("Disabled playlist looping", ephemeral: true);
                        AudioManager.loopPlaylist = false;
                        AudioManager.writePlaying = true;
                    }
                    else
                    {
                        await command.RespondAsync("Enabled playlist looping", ephemeral: true);
                        AudioManager.loopPlaylist = true;
                        AudioManager.writePlaying = false;
                    }
                    break;

                case "verbose":
                    if (AudioManager.writePlaying)
                    {
                        AudioManager.writePlaying = false;
                        Console.WriteLine(AudioManager.writePlaying);
                        await command.RespondAsync("Disabled verbose mode", ephemeral: true);
                    }
                    else
                    {
                        AudioManager.writePlaying = true;
                        Console.WriteLine(AudioManager.writePlaying);
                        await command.RespondAsync("Enabled verbose mode", ephemeral: true);
                    }
                    break;

                case "join":
                    try
                    {
                        await command.RespondAsync(await AudioManager.JoinAsync(guild, user, channel), ephemeral: true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await command.RespondAsync("Error, sorry bout that.", ephemeral: true);
                    }
                    break;

                case "queue":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync( x => x.Content = "executed queue");
                    await AudioManager.QueueAsync(guild, channel);
                    break;

                case "addto":
                    command.DeferAsync(ephemeral: true);
                    string? val = null;
                    Console.WriteLine(command.Data.Options.Count);
                    if (command.Data.Options.Count > 1)
                    {
                        val = command.Data.Options.Last().Value.ToString();
                        Console.WriteLine(val);
                    }
                    await command.ModifyOriginalResponseAsync( x => x.Content = "executet AddTo");
                    await AudioManager.addtoAsync(guild, channel, command.Data.Options.First().Value.ToString(), val, user);
                    break;

                case "removefrom":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync( x => x.Content = "executet RemoveFrom");
                    await AudioManager.removefromAsync(guild, channel, command.Data.Options.Last().Value.ToString(), command.Data.Options.First().Value.ToString());
                    break;

                case "playplaylist":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync( x => x.Content = "executet PlayPlaylist");
                    await AudioManager.playplaylistAsync(user, guild, channel, command.Data.Options.First().Value.ToString());
                    break;

                case "createplaylist":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync( x => x.Content = "executet CreatePlaylist");
                    await AudioManager.createplaylistAsync(guild, command.Channel as ITextChannel, command.Data.Options.First().Value.ToString());
                    break;

                case "listplaylist":
                    command.DeferAsync(ephemeral: true);
                    await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.ListPlaylists(guild, channel));
                    break;

                case "remove":
                    command.DeferAsync(ephemeral: true);
                    if (command.Data.Options.First().Value.ToString().Length <= 4)
                        try
                        {
                            int j = Int16.Parse(command.Data.Options.First().Value.ToString());
                            await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.RemoveAsync(guild, j));
                        }
                        catch
                        {
                            await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.RemoveAsync(guild, command.Data.Options.First().Value.ToString()));
                        }

                    else
                        await command.ModifyOriginalResponseAsync(async x => x.Content = await AudioManager.RemoveAsync(guild, command.Data.Options.First().Value.ToString()));
                    break;

                case "clear":
                    await command.RespondAsync(await AudioManager.ClearAsync(guild), ephemeral: true);
                    break;

                case "add":
                    await command.RespondAsync("executed Add");
                    await AudioManager.addtoAsync(guild, channel, command.Data.Options.First().Value.ToString(), user);
                    break;

                case "playosu":
                    command.DeferAsync(ephemeral: true);
                    int? i = 1;
                    try
                    {
                        if (command.Data.Options.Count > 0)
                        {
                            i = int.Parse(command.Data.Options.FirstOrDefault().Value.ToString());
                        }
                        string str = await AudioManager.PlayOsuAsync(guild, channel, user, i);
                        await command.ModifyOriginalResponseAsync(x => x.Content = str);
                    }
                    catch { Console.WriteLine($"[{DateTime.Now}]error in playosu slash handler."); 
                        await command.ModifyOriginalResponseAsync(x => x.Content = "Unknown error, sorry bout that"); }
                    break;

                default: Console.WriteLine("could not find command in switch block.");
                    await command.ModifyOriginalResponseAsync(x => x.Content = "Error 404");
                    return;
            }
            
            return;
        }
    }
}




/* 
 * if (command.Data.Name == "play")
            {
                await command.Channel.SendMessageAsync(await AudioManager.PlayAsync(command.User as SocketGuildUser, _client.GetGuild(command.GuildId.Value), command.Data.Options.First().Value.ToString(), command.Channel as ITextChannel));
            }
            if (command.Data.Name == "skip")
            {
                if (AudioManager.writePlaying)
                    await command.Channel.SendMessageAsync(await AudioManager.SkipAsync(_client.GetGuild(command.GuildId.Value)));
                else
                    await AudioManager.SkipAsync(_client.GetGuild(command.GuildId.Value));
            }
            if (command.Data.Name == "pause")
            {
                await command.Channel.SendMessageAsync(await AudioManager.PauseAsync(_client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "resume")
            {
                await command.Channel.SendMessageAsync(await AudioManager.ResumeAsync(_client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "stop")
            {
                await command.Channel.SendMessageAsync(await AudioManager.StopAsync(_client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "leave")
            {
                await command.Channel.SendMessageAsync(await AudioManager.LeaveAsync(_client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "volume")
            {
                await command.Channel.SendMessageAsync(await AudioManager.VolumeAsync(ushort.Parse(command.Data.Options.First().Value.ToString()), _client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "shuffle")
            {
                await command.Channel.SendMessageAsync(await AudioManager.ShuffleAsync(_client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "seek")
            {
                await command.Channel.SendMessageAsync(await AudioManager.SeekAsync(command.Data.Options.First().Value.ToString(), _client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "goto")
            {
                if (command.Data.Options.First().Value.ToString().Length <= 4)
                    try
                    {
                        int i = Int16.Parse(command.Data.Options.First().Value.ToString());
                        await command.Channel.SendMessageAsync(await AudioManager.GotoAsync(_client.GetGuild(command.GuildId.Value), i));
                    }
                    catch
                    {
                        await command.Channel.SendMessageAsync(await AudioManager.GotoAsync(_client.GetGuild(command.GuildId.Value), command.Data.Options.First().Value.ToString()));
                    }
                    
                else
                    await command.Channel.SendMessageAsync(await AudioManager.GotoAsync(_client.GetGuild(command.GuildId.Value), command.Data.Options.First().Value.ToString()));
            }
            if (command.Data.Name == "loop")
            {
                if (AudioManager.loop)
                {
                    await command.Channel.SendMessageAsync("Disabled looping");
                    AudioManager.loop = false;
                    AudioManager.writePlaying = true;
                }
                else
                {
                    await command.Channel.SendMessageAsync("Enabled looping");
                    AudioManager.loop = true;
                    AudioManager.writePlaying = false;
                }
            }

            if (command.Data.Name == "loopplaylist")
            {
                if (AudioManager.loopPlaylist)
                {
                    await command.Channel.SendMessageAsync("Disabled playlist looping");
                    AudioManager.loopPlaylist = false;
                    AudioManager.writePlaying = true;
                }
                else
                {
                    await command.Channel.SendMessageAsync("Enabled playlist looping");
                    AudioManager.loopPlaylist = true;
                    AudioManager.writePlaying = false;
                }
            }

            if (command.Data.Name == "verbose")
            {
                if (AudioManager.writePlaying)
                {
                    AudioManager.writePlaying = false;
                    Console.WriteLine(AudioManager.writePlaying);
                    await command.Channel.SendMessageAsync(AudioManager.writePlaying.ToString());
                }
                else
                {
                    AudioManager.writePlaying = true;
                    Console.WriteLine(AudioManager.writePlaying);
                    await command.Channel.SendMessageAsync(AudioManager.writePlaying.ToString());
                }
            }
            if (command.Data.Name == "join")
            {
                await command.Channel.SendMessageAsync(await AudioManager.JoinAsync(_client.GetGuild(command.GuildId.Value), command.User as IVoiceState, command.Channel as ITextChannel));
            }
            if (command.Data.Name == "queue")
            {
                await AudioManager.QueueAsync(_client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel); 
            }
            if (command.Data.Name == "addto")
            {
                string? val = null;
                Console.WriteLine(command.Data.Options.Count);
                if (command.Data.Options.Count > 1)
                {
                    val = command.Data.Options.Last().Value.ToString();
                    Console.WriteLine(val);
                }
                await AudioManager.addtoAsync(_client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel, command.Data.Options.First().Value.ToString(), val, command.User as SocketGuildUser);
            }
            if (command.Data.Name == "removefrom")
            {
                await AudioManager.removefromAsync(_client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel, command.Data.Options.Last().Value.ToString(), command.Data.Options.First().Value.ToString());
            }
            if (command.Data.Name == "playplaylist")
            {
                await AudioManager.playplaylistAsync(command.User as SocketGuildUser, _client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel, command.Data.Options.First().Value.ToString());
            }
            if (command.Data.Name == "createplaylist")
            {
                await AudioManager.createplaylistAsync(_client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel, command.Data.Options.First().Value.ToString());
            }
            if (command.Data.Name == "listplaylists")
            {
                await command.Channel.SendMessageAsync(await AudioManager.ListPlaylists(_client.GetGuild(command.GuildId.Value),command.Channel as ITextChannel));
            }
            if (command.Data.Name == "remove")
            {
                if (command.Data.Options.First().Value.ToString().Length <= 4)
                    try
                    {
                        int i = Int16.Parse(command.Data.Options.First().Value.ToString());
                        await command.Channel.SendMessageAsync(await AudioManager.RemoveAsync(_client.GetGuild(command.GuildId.Value), i));
                    }
                    catch
                    {
                        await command.Channel.SendMessageAsync(await AudioManager.RemoveAsync(_client.GetGuild(command.GuildId.Value), command.Data.Options.First().Value.ToString()));
                    }

                else
                    await command.Channel.SendMessageAsync(await AudioManager.RemoveAsync(_client.GetGuild(command.GuildId.Value), command.Data.Options.First().Value.ToString()));
            }
            if (command.Data.Name == "clear")
            {
                await command.Channel.SendMessageAsync(await AudioManager.ClearAsync(_client.GetGuild(command.GuildId.Value)));
            }
            if (command.Data.Name == "add")
            {
                await AudioManager.addtoAsync(_client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel, command.Data.Options.First().Value.ToString(), command.User as SocketGuildUser);
            }
            if (command.Data.Name == "playosu")
            {
                int? i = 1;
                try 
                {
                    if (command.Data.Options.Count > 0)
                    {
                        i = int.Parse(command.Data.Options.FirstOrDefault().Value.ToString());
                    }
                    await command.Channel.SendMessageAsync(await AudioManager.PlayOsuAsync(_client.GetGuild(command.GuildId.Value), command.Channel as ITextChannel, command.User as SocketGuildUser, i));
                }
                catch { Console.WriteLine("error in playosu slash handler."); }
                 
            }
*/





































/*  Delete commands.
 * var cmdList = await _client.GetGlobalApplicationCommandsAsync(true, null, null);
            foreach (var cmd in cmdList)
            {
                Console.WriteLine(cmd.Name);
                if (cmd.Name == "f")
                    await cmd.DeleteAsync();
                if (cmd.Name == "first")
                    await cmd.DeleteAsync();
                if (cmd.Name == "first-command")
                    await cmd.DeleteAsync();
                if (cmd.Name == "first-global-comman")
                    await cmd.DeleteAsync();
                if (cmd.Name == "first-global-command")
                    await cmd.DeleteAsync();
                if (cmd.Name == "heki")
                    await cmd.DeleteAsync();
            }*/



/*            var guild = _client.GetGuild(315396478078287872);

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var playGuildCommand = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            playGuildCommand.WithName("play");

            // Descriptions can have a max length of 100.
            playGuildCommand.WithDescription("Searches for or plays the track");

            playGuildCommand.AddOption("querry",ApplicationCommandOptionType.String,"the link or name of the track", isRequired: true);

            await guild.CreateApplicationCommandAsync(playGuildCommand.Build());
*/
