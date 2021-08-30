using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Net.Configuration;
using System.Net;
using System.Collections.Specialized;
using Microsoft.Win32;

namespace ExampleBot
{
    class Program
    {
        public static string prefix = "t!";
        static void Main(string[] args)
        {
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string token = "TOKEN";

            _client.Log += _client_Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            Thread.Sleep(5000);

            await _client_StartLog();

            await Task.Delay(-1);
        }
        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        private async Task _client_StartLog()
        {
            string user = _client.CurrentUser.Username + "#" + _client.CurrentUser.Discriminator;

            Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} Bejelentkezve mint {user}");
        }
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message != null)
            {
                string message_content = message.Content;
                var context = new SocketCommandContext(_client, message);

                if (message.Author.IsBot) return;

                //string prefix = null;
                //StreamReader fileread = new StreamReader(@"prefix.ini");
                //prefix = fileread.ReadToEnd();
                //fileread.Close();
                //int pos = 0;
                //pos = prefix.IndexOf(context.Guild.Id.ToString());
                //prefix = prefix.Substring(pos, prefix.Length - pos);
                //pos = prefix.IndexOf("\r");
                //prefix = prefix.Substring(0, pos);
                //pos = prefix.IndexOf("\t");
                //prefix = prefix.Substring(pos, prefix.Length - pos);
                //prefix = prefix.Replace("\t", null);
                ////Console.WriteLine(prefix);

                int argPos = 0;
                string prefixdefault = "r!";
                if (message.Content.StartsWith(context.Client.CurrentUser.Mention))
                {
                    var builder = new EmbedBuilder();
                    _ = builder.WithFooter(Program.MainFooter);
                    _ = builder.WithCurrentTimestamp();
                    _ = builder.WithAuthor(context.Message.Author);
                    _ = builder.WithTitle("RangerBot • Help");
                    _ = builder.Description = "A jelenlegi ParancsLista megtalálható a **[Weboldalon](https://rangerbot.webnode.hu/parancslista).**";
                    _ = builder.Color = Color.Blue;

                    await context.Channel.SendMessageAsync("", false, builder.Build()).ConfigureAwait(false);

                    Run.Log(context);
                }

                if (message.HasStringPrefix(prefix, ref argPos) || message.HasStringPrefix(prefixdefault, ref argPos))
                {
                    {
                        //if (message.Content.StartsWith(prefix + "say"))
                        //{
                        //    await message.DeleteAsync();
                        //    await context.Channel.SendMessageAsync(message.Content);
                        //}
                        //else if (message.Content.StartsWith(prefix + "csay"))
                        //{
                        //    await context.Channel.SendMessageAsync("Üzenetét sikeresen elküldtük!");
                        //    ulong id = 712960443114717195;
                        //    var channel = _client.GetChannel(id) as IMessageChannel;
                        //    await channel.SendMessageAsync(message.Content);
                        //}
                        //else if (message.Content.StartsWith(prefix + "prefix") || message.Content.StartsWith(prefixdefault + "prefix"))
                        //{
                        //    string prefixnew = null;
                        //    string filecontent = null;
                        //    StreamReader filereadnew = new StreamReader(@"prefix.ini");
                        //    filecontent = filereadnew.ReadToEnd();
                        //    prefixnew = filecontent;
                        //    filereadnew.Close();

                        //    int posnew = 0;
                        //    posnew = prefixnew.IndexOf(context.Guild.Id.ToString());
                        //    prefixnew = prefixnew.Substring(posnew, prefixnew.Length - posnew);
                        //    posnew = prefixnew.IndexOf("\r");
                        //    prefixnew = prefixnew.Substring(0, posnew);
                        //    posnew = prefixnew.IndexOf("\t");
                        //    prefixnew = prefixnew.Substring(posnew, prefixnew.Length - posnew);
                        //    prefixnew = prefixnew.Replace("\t", null);

                        //    filecontent = filecontent.Replace(context.Guild.Id + "\t" + prefixnew, context.Guild.Id + "\t" + message.Content);

                        //    StreamWriter file = new StreamWriter(@"prefix.ini", false);
                        //    file.WriteLine(filecontent);
                        //    file.Close();
                        //    //await context.Channel.SendMessageAsync(message.Content);
                        //    Console.WriteLine(context.Guild.Id + "\t" + message.Content);
                        //    await context.Channel.SendMessageAsync(context.User + ", a prefixet sikeresen átállítottad." + "\n" + "*Ha a parancsot tartalom nélkül írtad be (Így: r!prefix), akkor ez lett az új prefix: r!prefix*" + "\n" + "*A prefix parancsnál mindig működik az <r!> prefix*");

                        //}
                        //else if (message.Content.StartsWith(prefix + "bugreport") || message.Content.StartsWith(prefix + "br"))
                        //{
                        //    string user = context.User.ToString();
                        //    user = user.Substring(0, user.IndexOf("#"));

                        //    var builder = new EmbedBuilder()
                        //    {
                        //        Color = Color.Red,
                        //        Title = "RangerBot • BugReport",
                        //    };

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Server • ServerID";
                        //        x.Value = context.Guild.Name + " - " + context.Guild.Id;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Channel • ChannelID";
                        //        x.Value = context.Channel.Name + " - " + context.Channel.Id;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "User • UserID";
                        //        x.Value = context.User + " - " + context.User.Id;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Időpont";
                        //        x.Value = DateTime.Now.ToString();
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Üzenet";
                        //        x.Value = message.Content;
                        //    });

                        //    await context.Channel.SendMessageAsync("Jelentését sikeresen továbítottuk a *Rangerbot Support and Dev.* csapatnak!");
                        //    ulong id = 712960443114717195;
                        //    var channel = _client.GetChannel(id) as IMessageChannel;
                        //    await channel.SendMessageAsync("", false, builder.Build());
                        //}
                        //else if (message.Content.StartsWith(prefix + "ötlet") || message.Content.StartsWith(prefix + "idea"))
                        //{
                        //    string user = context.User.ToString();
                        //    user = user.Substring(0, user.IndexOf("#"));

                        //    var builder = new EmbedBuilder()
                        //    {
                        //        Color = Color.DarkGreen,
                        //        Title = "**RangerBot • Ötlet**",
                        //    };

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Server • ServerID";
                        //        x.Value = context.Guild.Name + " - " + context.Guild.Id;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Channel • ChannelID";
                        //        x.Value = context.Channel.Name + " - " + context.Channel.Id;
                        //    //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "User • UserID";
                        //        x.Value = context.User + " - " + context.User.Id;
                        //    //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Időpont";
                        //        x.Value = DateTime.Now.ToString();
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Üzenet";
                        //        x.Value = message.Content;
                        //    //x.IsInline = true;
                        //    });

                        //    await context.Channel.SendMessageAsync("Ötletét sikeresen továbítottuk a *Rangerbot Support and Dev.* csapatnak!");
                        //    ulong id = 717760483603513537;
                        //    var channel = _client.GetChannel(id) as IMessageChannel;
                        //    await channel.SendMessageAsync("", false, builder.Build());
                        //}
                        //else if (message.Content.StartsWith(prefix + "support"))
                        //{
                        //    string user = context.User.ToString();
                        //    user = user.Substring(0, user.IndexOf("#"));

                        //    var builder = new EmbedBuilder()
                        //    {
                        //        Color = Color.DarkGreen,
                        //        Title = "**RangerBot • Support**",
                        //    };

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Server • ServerID";
                        //        x.Value = context.Guild.Name + " - " + context.Guild.Id;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Channel • ChannelID";
                        //        x.Value = context.Channel.Name + " - " + context.Channel.Id;
                        //    //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "User • UserID";
                        //        x.Value = context.User + " - " + context.User.Id;
                        //    //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Időpont";
                        //        x.Value = DateTime.Now.ToString();
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Üzenet";
                        //        x.Value = message.Content;
                        //    //x.IsInline = true;
                        //    });

                        //    await context.Channel.SendMessageAsync("Segítségkérését sikeresen továbítottuk a *Rangerbot Support and Dev.* csapatnak!");
                        //    ulong id = 728156581631361044;
                        //    var channel = _client.GetChannel(id) as IMessageChannel;
                        //    await channel.SendMessageAsync("", false, builder.Build());
                        //}
                        //else if (message.Content.StartsWith(prefix + "myuserinfo"))
                        //{
                        //    string user = context.User.ToString();
                        //    user = user.Substring(0, user.IndexOf("#"));
                        //    Color color = Color.DarkBlue;
                        //    var builder = new EmbedBuilder()
                        //    {
                        //        //Optional color
                        //        Color = Color.DarkBlue,
                        //        //Author = ,
                        //        Title = "RangerBot • MyUserInfo",
                        //        //Description = user
                        //        //ImageUrl = 
                        //};

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Mention";
                        //        x.Value = context.User.Mention;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "User • UserID";
                        //        x.Value = context.User.Username + " • " + context.User.Id;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "AvatarID";
                        //        x.Value = context.User.AvatarId;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Status";
                        //        x.Value = context.User.Status;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Activity";
                        //        x.Value = context.User.Activity;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Létrehozva";
                        //        x.Value = context.User.CreatedAt;
                        //        //x.IsInline = true;
                        //    });

                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Bot?";
                        //        x.Value = context.User.IsBot;
                        //        //x.IsInline = true;
                        //    });

                        //    ulong id = context.Channel.Id;
                        //    var channel = _client.GetChannel(id) as IMessageChannel;
                        //    await channel.SendMessageAsync("", false, builder.Build());
                        //}
                    }
                    //if (message.Content.StartsWith(prefix + "test"))
                    //{
                    //    await context.Channel.SendMessageAsync("test");
                    //}
                    if (message.Content.StartsWith(prefix + "dev"))
                    {
                        for (int i_ = 0; i_ < Developers.Count(); i_++)
                        {
                            if (context.User.Id != Developers[i_])
                            {
                                await context.Channel.SendMessageAsync("Ehhez neked nincs jogosultságod.");
                                break;
                            }
                            else
                            {
                                var result = await _commands.ExecuteAsync(context, argPos, _services);
                                //if (!result.IsSuccess) Console.WriteLine(message.Content);
                                if (!result.IsSuccess)
                                {
                                    Console.WriteLine("DevError");
                                    Run.Log(context, "Error", result.ErrorReason);
                                    if (result.ErrorReason.Contains("User not found."))
                                        Run.Error(context, "UserNotFound");
                                    else if (result.ErrorReason.Contains("Channel not found."))
                                        Run.Error(context, "ChannelNotFound");
                                    else if (result.ErrorReason.Contains("Unknown command."))
                                        Run.Error(context, "UnknownCommand");
                                    else if (result.ErrorReason.Contains("The input text has too few parameters."))
                                        Run.Error(context, "TooFewParameters");
                                    else if (result.ErrorReason.Contains("The input text has too many parameters."))
                                        Run.Error(context, "TooManyParameters");
                                    else if (result.ErrorReason.Contains("Response status code does not indicate success: 503 (Service Unavailable)."))
                                        Run.Error(context, "503_ServiceUnavailable");
                                    else
                                    {
                                        Run.Error(context, "UnknownError");
                                        Console.WriteLine(result.ErrorReason);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var result = await _commands.ExecuteAsync(context, argPos, _services);
                        //if (!result.IsSuccess) Console.WriteLine(message.Content);
                        if (!result.IsSuccess)
                        {
                            Console.WriteLine("Error");
                            Run.Log(context, "Error", result.ErrorReason);
                            if (result.ErrorReason.Contains("User not found."))
                                Run.Error(context, "UserNotFound");
                            else if (result.ErrorReason.Contains("Channel not found."))
                                Run.Error(context, "ChannelNotFound");
                            else if (result.ErrorReason.Contains("Unknown command."))
                                Run.Error(context, "UnknownCommand");
                            else if (result.ErrorReason.Contains("The input text has too few parameters."))
                                Run.Error(context, "TooFewParameters");
                            else if (result.ErrorReason.Contains("The input text has too many parameters."))
                                Run.Error(context, "TooManyParameters");
                            else if (result.ErrorReason.Contains("Response status code does not indicate success: 503 (Service Unavailable)."))
                                Run.Error(context, "503_ServiceUnavailable");
                            else
                            {
                                Run.Error(context, "UnknownError");
                                Console.WriteLine(result.ErrorReason);
                            }
                        }
                    }

                    {
                        //string user = context.User.ToString();
                        //user = user.Substring(0, user.IndexOf("#"));
                        //Color color = Color.Blue;
                        //string title = "**ServerLog**";

                        //if (message.Content.StartsWith(prefix + "prefix"))
                        //{
                        //    color = Color.Gold;
                        //    title += " **• Prefix**";
                        //}

                        //if (message.Content.StartsWith(prefix + "bugreport") || message.Content.StartsWith(prefix + "br"))
                        //{
                        //    color = Color.Red;
                        //    title += " **-• BugReport**";
                        //}

                        //if (message.Content.StartsWith(prefix + "ötlet") || message.Content.StartsWith(prefix + "idea"))
                        //{
                        //    color = Color.Green;
                        //    title += " **• Ötlet**";
                        //}

                        //if (message.Content.StartsWith(prefix + "support"))
                        //{
                        //    color = Color.Orange;
                        //    title += " **• Support**";
                        //}

                        //if (message.Content.StartsWith(prefix + "kick"))
                        //{
                        //    color = Color.DarkRed;
                        //    title += " **• Kirúgás**";
                        //}

                        //if (message.Content.StartsWith(prefix + "ban"))
                        //{
                        //    color = Color.DarkRed;
                        //    title += " **• Kitiltás**";
                        //}

                        //if (message.Content.StartsWith(prefix + "purge"))
                        //{
                        //    color = Color.Red;
                        //    title += " **• Purge**";
                        //}

                        //if (message.Content.StartsWith(prefix + "clear"))
                        //{
                        //    color = Color.Red;
                        //    title += " **- Clear**";
                        //}

                        //if (message.Content.StartsWith(prefix + "ping"))
                        //{
                        //    color = Color.LightGrey;
                        //    title += " **• Ping**";
                        //}

                        //if (message.Content.StartsWith(prefix + "linkek"))
                        //{
                        //    color = Color.LightGrey;
                        //    title += " **• Linkek**";
                        //}

                        //if (message.Content.StartsWith(prefix + "parancsok"))
                        //{
                        //    color = Color.LightGrey;
                        //    title += " **• Parancsok**";
                        //}

                        //if (message.Content.StartsWith(prefix + "commands"))
                        //{
                        //    color = Color.LightGrey;
                        //    title += " **• Commands**";
                        //}

                        //if (message.Content.StartsWith(prefix + "say"))
                        //{
                        //    color = Color.LighterGrey;
                        //    title += " **• Say**";
                        //}

                        //if (message.Content.StartsWith(prefix + "serverinfo"))
                        //{
                        //    color = Color.LighterGrey;
                        //    title += " **• ServerInfo**";
                        //}

                        //if (message.Content.StartsWith(prefix + "myuserinfo"))
                        //{
                        //    color = Color.LighterGrey;
                        //    title += " **• MyUserInfo**";
                        //}

                        //if (message.Content.StartsWith(prefix + "myavatar"))
                        //{
                        //    color = Color.LighterGrey;
                        //    title += " **• MyAvatar**";
                        //}

                        //if (message.Content.StartsWith(prefix + "help"))
                        //{
                        //    color = Color.LightGrey;
                        //    title += " **• Help**";
                        //}

                        //var builder = new EmbedBuilder()
                        //    {
                        //    Color = color,
                        //    Title = title,
                        //    };

                        //builder.AddField(x =>
                        //{
                        //    x.Name = "Server • ServerID";
                        //    x.Value = context.Guild.Name + " - " + context.Guild.Id;
                        //    //x.IsInline = true;
                        //});

                        //builder.AddField(x =>
                        //{
                        //    x.Name = "Channel • ChannelID";
                        //    x.Value = context.Channel.Name + " - " + context.Channel.Id;
                        //    //x.IsInline = true;
                        //});

                        //builder.AddField(x =>
                        //{
                        //    x.Name = "User • UserID";
                        //    x.Value = context.User + " - " + context.User.Id;
                        //    //x.IsInline = true;
                        //});

                        //builder.AddField(x =>
                        //{
                        //    x.Name = "Időpont";
                        //    x.Value = DateTime.Now.ToString();
                        //    //x.IsInline = true;
                        //});

                        //builder.AddField(x =>
                        //{
                        //    x.Name = "Teljes Parancs";
                        //    x.Value = message.Content;
                        //    //x.IsInline = true;
                        //});

                        //if (message.Content.StartsWith(prefix + "kick") || (message.Content.StartsWith(prefix + "ban")))
                        //{
                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Kirúgott + Oka";
                        //        x.Value = message.Content;
                        //    //x.IsInline = true;
                        //    });
                        //}

                        //if (message.Content.StartsWith(prefix + "purge") || (message.Content.StartsWith(prefix + "clear")))
                        //{
                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Üzenetek száma";
                        //        x.Value = message.Content;
                        //        //x.IsInline = true;
                        //    });
                        //}

                        //if (message.Content.StartsWith(prefix + "say"))
                        //{
                        //    builder.AddField(x =>
                        //    {
                        //        x.Name = "Üzenet";
                        //        x.Value = message.Content;
                        //        //x.IsInline = true;
                        //    });
                        //}

                        //ulong id = Program.logChannelID;
                        //var channel = _client.GetChannel(id) as IMessageChannel;
                        //await channel.SendMessageAsync("", false, builder.Build());
                    }
                }
            }
        }
    }
}