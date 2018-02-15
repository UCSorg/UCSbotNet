using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using UCSbot.Music.Handlers;
using UCSbot.Music.Services;
using UCSbot.Music.Processes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Discord.Net.WebSockets;
using System.Collections.Generic;
using System.IO;

namespace UCSbot
{
    public class Program
    {
        public CommandService commands;
        public DiscordSocketClient client;
        public IServiceProvider services;
        public IWebSocketClient websocket;
        private Settings _settings;
        private Media _media;
        protected ulong voiceID;
        protected string gamename;
        protected bool checker = true;
        ulong logchannel = 338422549673410561;
        public string token = File.ReadAllText("BotToken.txt");

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        public async Task Start()
        {
            commands = new CommandService();
            _settings = new Settings();
            _media = new Media(_settings);
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000
            });
            client.Log += Logger;
            commands = new CommandService();
            await InstallCommands();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            //handler = new ConsoleEventDelegate(ConsoleEventCallback);
            //SetConsoleCtrlHandler(handler, true);
            //Console.ReadLine();

            await Task.Delay(-1);
        }

        //public bool ConsoleEventCallback(int eventType)
        //{
        //    if (eventType == 2)
        //    {
        //        client.StopAsync();
        //        Thread.Sleep(1000);
        //    }
        //    return false;
        //}
        //static ConsoleEventDelegate handler;  
        //private delegate bool ConsoleEventDelegate(int eventType);
        //[DllImport("kernel32.dll", SetLastError = true)]
        //private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        public async Task InstallCommands()
        {
            IServiceCollection map = new ServiceCollection();

            map.AddSingleton(_settings);
            map.AddSingleton(_media);
            map.AddSingleton(new Youtube());

            services = map.BuildServiceProvider();

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());

            client.UserJoined += MemberJoined;
            client.MessageReceived += HandleCommand;
            //client.UserVoiceStateUpdated += VoiceHandler;
            client.GuildMemberUpdated += UserUpdateHandler;
            client.UserLeft += MemberLeft;
            client.UserBanned += MemberBanned;
            client.UserUnbanned += MemberUnbanned;
            client.RoleDeleted += RoleDeleted;
        }

        public async Task RoleDeleted(SocketRole roleDeleted)
        {
            RankFilePath _path = new RankFilePath();
            string rolelower = roleDeleted.Name.ToLower();
            var lines = File.ReadAllLines(_path.RankFile);
            var count = lines.Where(x => x.Equals(rolelower));
            var deleteL = lines.Where(x => !x.Equals(rolelower));
            if (count.Count() != 0)
            {
                File.WriteAllLines(_path.RankFile, deleteL);
            }

        }

        public async Task UserUpdateHandler(SocketGuildUser userbefore, SocketGuildUser userafter)
        {
            if(userbefore.Nickname != userafter.Nickname)
            {
                var log = userbefore.Guild.GetChannel(logchannel) as IMessageChannel;

                var embed4 = new EmbedBuilder()
                          .WithColor(new Color(3901951))
                          .AddField("Nickname Changed", $"{userbefore.Mention}'s nickname has been changed from **{userbefore.Nickname}** to **{userafter.Nickname}**")
                          .WithThumbnailUrl(userbefore.GetAvatarUrl());
                await log.SendMessageAsync("", embed: embed4);
            }

            if(userbefore.Roles.Count() != userafter.Roles.Count())
            {
                var rolesbefore = userbefore.Roles;
                var rolesafter = userafter.Roles;
                string msg = "";
                if (rolesafter.Count() > rolesbefore.Count())
                {
                    var roleadded = rolesafter.Except(rolesbefore).FirstOrDefault();
                    msg = $"Role **{roleadded.Name}** has been added to {userbefore.Mention}";
                }
                if(rolesafter.Count() < rolesbefore.Count())
                {
                    var roletaken = rolesbefore.Except(rolesafter).FirstOrDefault();
                    msg = $"Role **{roletaken.Name}** has been taken from {userbefore.Mention}";
                }

                var log = userbefore.Guild.GetChannel(logchannel) as IMessageChannel;
                var embed5 = new EmbedBuilder()
                          .WithColor(new Color(3901951))
                          .AddField("Roles Changed", msg)
                          .WithThumbnailUrl(userbefore.GetAvatarUrl());
                await log.SendMessageAsync("", embed: embed5);
            }


        }
        public async Task MemberBanned(SocketUser user, SocketGuild guild)
        {
            var log = guild.GetChannel(logchannel) as IMessageChannel;

            var embed4 = new EmbedBuilder()
                      .WithColor(new Color(16711680))
                      .AddField("User Banned", $"**{user.Mention}** has been banned!")
                      .WithThumbnailUrl(user.GetAvatarUrl());
            await log.SendMessageAsync("", embed: embed4);
        }

        public async Task MemberUnbanned(SocketUser user, SocketGuild guild)
        {
            var log = guild.GetChannel(logchannel) as IMessageChannel;

            var embed4 = new EmbedBuilder()
                      .WithColor(new Color(65280))
                      .AddField("User Unbanned", $"**{user.Mention}** has been unbanned!")
                      .WithThumbnailUrl(user.GetAvatarUrl());
            await log.SendMessageAsync("", embed: embed4);
        }

        //public async Task VoiceHandler(SocketUser user, SocketVoiceState voicebefore, SocketVoiceState voiceafter)
        //{
        //    try
        //    {
        //        var guild = user as socketguilduser;

        //        if (user.game.hasvalue == true)
        //        {
        //            gamename = user.game.value.name;
        //        }

        //        var channelstocreate2v2 = guild.guild.voicechannels.where(x => x.users.count() == 0 & x.name == "2v2 private");
        //        var channelstocreate3v3 = guild.guild.voicechannels.where(x => x.users.count() == 0 & x.name == "3v3 private");
        //        var gamechannels = guild.guild.voicechannels.where(x => x.name == "game" || x.name == gamename);

        //        if (voiceafter.voicechannel != null)
        //        {
        //            if (voiceafter.voicechannel.name == "2v2 private")
        //            {
        //                if (channelstocreate2v2.count() == 0)
        //                {
        //                    var position2v2 = guild.guild.voicechannels.firstordefault(x => x.name == "2v2 private").position;
        //                    await task.delay(50);
        //                    var channel2 = await guild.guild.createvoicechannelasync(voiceafter.voicechannel.name);
        //                    await (channel2 as ivoicechannel)?.modifyasync(x =>
        //                    {
        //                        x.position = position2v2;
        //                        x.userlimit = 2;
        //                    });

        //                }
        //            }
        //            if (voiceafter.voicechannel.name == "3v3 private")
        //            {
        //                if (channelstocreate3v3.count() == 0)
        //                {
        //                    var position3v3 = guild.guild.voicechannels.firstordefault(x => x.name == "3v3 private").position;
        //                    await task.delay(50);
        //                    var channel3 = await guild.guild.createvoicechannelasync(voiceafter.voicechannel.name);
        //                    await (channel3 as ivoicechannel)?.modifyasync(x =>
        //                    {
        //                        x.position = position3v3;
        //                        x.userlimit = 3;
        //                    });
        //                }
        //            }
        //            if (voiceafter.voicechannel.name == "game")
        //            {
        //                if (gamename != null)
        //                {
        //                    await voiceafter.voicechannel.modifyasync(x => x.name = gamename);
        //                    await task.delay(500);
        //                }
        //            }

        //            if (voiceafter.voicechannel.name == "scrims competetive" && checker == true)
        //            {
        //                var positionblue = guild.guild.voicechannels.firstordefault(x => x.name == "scrims competetive").position;
        //                await voiceafter.voicechannel.modifyasync(x => x.name = "🔵 scrims competetive");
        //                var orangecomp = await guild.guild.createvoicechannelasync("🔴 scrims competetive");
        //                await orangecomp.modifyasync(x => x.position = positionblue);
        //                checker = false;
        //            }
        //        }
        //        if (voicebefore.voicechannel != null)
        //        {
        //            if (voicebefore.voicechannel.name == "2v2 private")
        //            {
        //                var channelstodelete2 = guild.guild.voicechannels.where(x => x.name == "2v2 private" & x.users.count() == 0 & x.id != voicebefore.voicechannel.id);
        //                await task.whenall(channelstodelete2.select(async (x) =>
        //                {
        //                    await x.deleteasync();
        //                }));
        //            }
        //            if (voicebefore.voicechannel.name == "3v3 private")
        //            {
        //                var channelstodelete2 = guild.guild.voicechannels.where(x => x.name == "3v3 private" & x.users.count() == 0 & x.id != voicebefore.voicechannel.id);
        //                await task.whenall(channelstodelete2.select(async (x) =>
        //                {
        //                    await x.deleteasync();
        //                }));
        //            }

        //            if (voicebefore.voicechannel.name != "game")
        //            {

        //                if (voicebefore.voicechannel.users.count() == 0 && (voicebefore.voicechannel.id == 340396334534361089 || voicebefore.voicechannel.id == 354882713997672449 || voicebefore.voicechannel.id == 354882749489741825 || voicebefore.voicechannel.id == 354882759623311362))
        //                {
        //                    await voicebefore.voicechannel.modifyasync(x => x.name = "game");
        //                }
        //            }
        //            if (voicebefore.voicechannel.name == "🔵 scrims competetive" || voicebefore.voicechannel.name == "🔴 scrims competetive")
        //            {
        //                var channelblue = guild.guild.voicechannels.firstordefault(x => x.name == "🔵 scrims competetive");
        //                var channelorange = guild.guild.voicechannels.firstordefault(x => x.name == "🔴 scrims competetive");

        //                if (voicebefore.voicechannel == channelblue && voicebefore.voicechannel.users.count() == 0 && channelorange.users.count() == 0)
        //                {
        //                    await voicebefore.voicechannel.modifyasync(x => x.name = "scrims competetive");
        //                    await guild.guild.voicechannels.firstordefault(x => x.name == "🔴 scrims competetive").deleteasync();
        //                    checker = true;
        //                }
        //                if (voicebefore.voicechannel == channelorange && voicebefore.voicechannel.users.count() == 0 && channelblue.users.count() == 0)
        //                {
        //                    await voicebefore.voicechannel.modifyasync(x => x.name = "scrims competetive");
        //                    await guild.guild.voicechannels.firstordefault(x => x.name == "🔵 scrims competetive").deleteasync();
        //                    checker = true;
        //                }
        //            }

        //        }
        //    }
        //    catch (exception e)
        //    {
        //        console.writeline(e);
        //    }

        //}

        public async Task MemberJoined(SocketGuildUser user)
        {
            var channel = client.GetChannel(242462394318585876) as SocketTextChannel;
            await channel.SendMessageAsync("Hello " + user.Mention + " , welcome to UCS Gaming! If you're looking to start finding people to play with, please get in touch with an online Mod or Admin on getting your role set up, or go to the #update-season-rank-role channel and show proof of your rank there using a bot or screenshot. If you want to know more about UCS Gaming and what we have to offer, please read through our #welcome page. Good luck and have fun!");
            var role = user.Roles.FirstOrDefault(x => x.Name == "Rank Not Determined");
            await user.AddRoleAsync(role);
            var log = user.Guild.GetChannel(logchannel) as IMessageChannel;
            var embed5 = new EmbedBuilder()
                      .WithColor(new Color(65280))
                      .AddField("Member Joined", $"{user.Mention} {user.Username}#{user.Discriminator}")
                      .WithThumbnailUrl(user.GetAvatarUrl());
            await log.SendMessageAsync("", embed: embed5);
        }
        public async Task MemberLeft(SocketGuildUser user)
        {
            var log = user.Guild.GetChannel(logchannel) as IMessageChannel;
            var embed5 = new EmbedBuilder()
                      .WithColor(new Color(16711680))
                      .AddField("Member Left", $"{user.Mention} RIP {user.Username}#{user.Discriminator}")
                      .WithThumbnailUrl(user.GetAvatarUrl());
            await log.SendMessageAsync("", embed: embed5);
        }


        public async Task HandleCommand(SocketMessage messageParam)
        {

            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
            var context = new CommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
            if (message.Content.ToLower() == "!ping")
            {
                response = client.Latency;
            }

        }

        private static Task Logger(LogMessage message)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }
        public static int response;
        public static int Response
        {
            get => response;
            set => response = value;
        }
    }
    public class Troll
    {
        private string username;
        public string Author
        {
            get { return username; }
            set { username = value; }
        }
    }

    public class RankFilePath
    {
        private string filePath = @"C:\Users\srna.patrik\Desktop\UCSbot\UCSbot\UCSbot\Modules\RanksDatabase.txt";
        public string RankFile
        {
            get { return filePath; }
        }
    }
}
