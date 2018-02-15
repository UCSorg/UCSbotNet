using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using UCSbot.Modules;
using UCSbot;
using System.Diagnostics;
using RLSApi;
using RLSApi.Data;
using System.IO;

namespace UCSbot.Modules
{
    public class Info : ModuleBase
    {
        string Tmsg = "[UCS Gaming 2v2 Open Tournament #7 (09/09/17)](https://smash.gg/ucsopentournament11)";
        string matcherino = "https://matcherino.com/b/tournaments/7886";

        uint color = 3901951;
        [Command("donate"), Summary("Donation links")]
        public async Task donate()
        {
            string title = "__Donations__";
            string msg = $"If you're looking to help us out for upcoming events/tournaments, feel free to donate through [Matcherino]({matcherino}) or on our [Twitch jar](https://twitch.streamlabs.com/ucsgamingna#/) thank you!";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(color))
                .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("tournament"), Summary("Tournament date and link for signup")]
        public async Task tournament()
        {
            string title = "__Tournament__";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(color))
                .WithDescription(Tmsg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }
        [Command("tchange"), Summary("Changes tourney link")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task tchange([Remainder]string change)
        {
            string title = "__Tournament__";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(color))
                .WithDescription($"Content of !tournament has been changed to: `{change}`");
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("twitch"), Summary("Twitch channel")]
        public async Task twitch()
        {
            string title = "__Twitch__";
            string msg = "Check us out on [Twitch!](https://www.twitch.tv/ucsgamingna)";
            var embed = new EmbedBuilder()
                 .WithTitle(title)
                 .WithColor(new Color(color))
                 .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("twitter"), Summary("Twitter, yeah we have that too")]
        public async Task twitter()
        {
            string title = "__Twitter__";
            string msg = "Follow us on [Twitter](https://twitter.com/ucsgamingrl) for updates and other announcements!";
            var embed = new EmbedBuilder()
                 .WithTitle(title)
                 .WithColor(new Color(color))
                 .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("ts"), Summary("Tournament start and time zones converter")]
        public async Task ts()
        {
            string title = "__Tournament Start__";
            string msg = "Tournament Start Time: __**12 PM CDT/ 12AM PST/  1 PM EST/ 7 PM CEST/ 5 PM GMT**__ , if you would like to know the time for your country or you are not familiar with time zones use [Time Zone converter](https://www.timeanddate.com/worldclock/converter.html)";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(color))
                .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("userinfo"), Summary("Returns info about the current user. !userinfo [username]")]
        [Alias("user", "whois")]
        public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            System.DateTimeOffset guildCreatedAt = new System.DateTimeOffset();
            guildCreatedAt = userInfo.CreatedAt;
            string guildcreated = String.Format("{0:dd/MM/yyyy 'at' h:mm tt}", guildCreatedAt);
            var userimage = userInfo.GetAvatarUrl();
            var game = userInfo.Game;
            string display;
            if (game == null)
            {
                display = "User isn't playing anything";
            }
            else
            {
                display = game.Value.Name;
            }

            var embed = new EmbedBuilder()
                .WithColor(new Color(color))
                .AddInlineField("[Username]", userInfo.Username)
                .AddInlineField("[Account created]", guildcreated)
                .AddInlineField("[Currently playing]", display)
                .AddInlineField("[Status]", userInfo.Status)
                .WithThumbnailUrl(userimage);

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("serverinfo"), Summary("Returns info about UCS server")]
        [Alias("server")]
        public async Task ServerInfo()
        {
            var memberCount = await Context.Guild.GetUsersAsync();
            var owner = await Context.Guild.GetOwnerAsync();
            var channelsT = await Context.Guild.GetTextChannelsAsync();
            var channelsV = await Context.Guild.GetVoiceChannelsAsync();
            int roles = Context.Guild.Roles.Count;
            string regionid = Context.Guild.VoiceRegionId;
            var region = await Context.Client.GetVoiceRegionAsync(regionid);
            string guildName = Context.Guild.Name;
            var serverimage = Context.Guild.IconUrl;

            System.DateTimeOffset guildCreatedAt = new System.DateTimeOffset();
            guildCreatedAt = Context.Guild.CreatedAt;
            var emotes = Context.Guild.Emotes;
            var emotelist = "";
            foreach(var emote in emotes)
            {
                emotelist += emote.ToString() + " ";
            }
            string guildcreated = String.Format("{0:dd/MM/yyyy 'at' h:mm tt}", guildCreatedAt);
            var embed = new EmbedBuilder()
                .WithColor(new Color(color))
                .AddInlineField("[Name]", guildName)
                .AddInlineField("[Owner]", owner.Username)
                .AddInlineField("[Server created]", guildcreated)
                .AddInlineField("[Member count]", memberCount.Count)
                .AddInlineField("[Voice channels]", channelsV.Count)
                .AddInlineField("[Text channels]", channelsT.Count)
                .AddInlineField("[Roles]", roles)
                .AddInlineField("[Region]", region.Name)
                .AddInlineField("[Emotes]", emotelist)
                .WithThumbnailUrl(serverimage);

            await Context.Channel.SendMessageAsync("", embed: embed);

        }

        [Command("members"), Summary("Shows current member count")]
        public async Task members()
        {
            var memberCount = await Context.Guild.GetUsersAsync();
            string msg = $"There are currently **{memberCount.Count}** members in {Context.Guild.Name}!";
            string title = "__**Members**__";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(color))
                .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("randomcolor"), Summary("Generates random hex color with preview")]
        [Alias("randomcolour")]
        public async Task randomcolor()
        {
            var random = new Random();
            var colorR = String.Format("{0:X6}", random.Next(0x1000000));
            uint decColor = Convert.ToUInt32(colorR, 16);

            string msg = "Your hex color is ["+colorR+"](https://color-hex.com/color/"+colorR+")";
            string title = "__Random Color__";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(decColor))
                .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("uptime"), Summary("Shows uptime of the bot")]
        public async Task uptime()
        {
            var delta = DateTime.Now - Process.GetCurrentProcess().StartTime;
            var embed = new EmbedBuilder()
                .WithTitle("__Uptime__")
                .WithColor(new Color(color))
                .WithDescription($"Bot has been online for: **{delta.Days} days : {delta.Hours} hr : {delta.Minutes} min : {delta.Seconds} sec**");
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("training"), Summary("Training Packs")]
        public async Task training()
        {
            string title = "__Kimble's training pack:__";
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithColor(new Color(color))
                .AddInlineField("Aerial Shots", "E7B5-BC78-B4CE-3622")
                .AddInlineField("Domsee Dish Practice", "28B9-19D1-4853-5150")
                .AddInlineField("Drift Practice", "6D56-D57E-ACD2-3495")
                .AddInlineField("Flick Practice", "D2F1-727A-B9ED-E768")
                .AddInlineField("Wall Aerial Shots", "177A-F5DB-C488-51BD")
                .AddInlineField("Wall Aerial Defense", "3594-C422-43BE-5C2F")
                .AddInlineField("Air Dribble from Ground", "3021-EB38-6A0C-F8B7")
                .AddInlineField("Other Training packs", "[Google Docs](https://docs.google.com/document/d/19Ny86c4W3eAALkmUsGFR26cZoNm8swvcr0G1AGbN8IA/edit?usp=sharing)");
            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("listmods"), Summary("Generates list of available Moderators")]
        public async Task listmods()
        {
            var jonny = await Context.Guild.GetUserAsync(168969339842723840);
            var bryce = await Context.Guild.GetUserAsync(121745137142464514);
            var bax = await Context.Guild.GetUserAsync(234921600036765696);
            var nic = await Context.Guild.GetUserAsync(200008417132281858);
            var eny = await Context.Guild.GetUserAsync(87233414591156224);
            var steve = await Context.Guild.GetUserAsync(248668776395046913);
            var chris = await Context.Guild.GetUserAsync(238421441488355332);
            var anthony = await Context.Guild.GetUserAsync(201506635959631873);
            var kingmuff = await Context.Guild.GetUserAsync(144671762779602944);
            var memlo = await Context.Guild.GetUserAsync(137378950895632384);
            var kimble = await Context.Guild.GetUserAsync(113483577777233920);
            var era = await Context.Guild.GetUserAsync(257243382429122561);
            var mike = await Context.Guild.GetUserAsync(152990577036623873);
            var lzr = await Context.Guild.GetUserAsync(144939048132280321);

            string field1 = $"{jonny.Mention}\n" +
                $"{bryce.Mention}\n" +
                $"{bax.Mention}";
            string field2 =$"{nic.Mention}\n" +
                $"{eny.Mention}";
            string field3 = $"{steve.Mention}\n" +
                $"{chris.Mention}\n" +
                $"{anthony.Mention}\n" +
                $"{kingmuff.Mention}\n" +
                $"{memlo.Mention}\n" +
                $"{kimble.Mention}\n" +
                $"{mike.Mention}\n" +
                $"{era.Mention}\n" +
                $"{lzr.Mention}";
            var embed = new EmbedBuilder()
                .WithColor(new Color(color))
                .AddInlineField("__Available Owners:__", field1)
                .AddInlineField("__Available Admins:__", field2)
                .AddInlineField("__Available Mods:__", field3);
            await Context.Channel.SendMessageAsync("", embed: embed);
        }
        [Command("ping"), Summary("Check if bot is responding")]
        public async Task pring()
        {
            var connected = Context.Client.ConnectionState;
            if(connected == ConnectionState.Connected)
            {
                var embed = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription($"I am here ! it only took me {Program.response} ms");

                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }

        [Command("rlstats")]
        [Summary("Checks ranking of a player. !rlstats [steamid]")]
        public async Task rlstats([Remainder] string steamid = null)
        {
            string token = File.ReadAllText("RLToken.txt");
            if (steamid != null)
            {
                var _client = new RLSClient(token);
                //string msg = "";
                //string stats = "";

                var player = await _client.GetPlayerAsync(RlsPlatform.Steam, steamid);
                //var tier = await _client.GetTiersAsync();
                //var playerSeasonFive = player.RankedSeasons.FirstOrDefault(x => x.Key == RlsSeason.Seven);
                //stats = $"**Wins**: {player.Stats.Wins}\n" +
                //    $"**Mvps**: {player.Stats.Mvps}\n" +
                //    $"**Goals**: {player.Stats.Goals}\n" +
                //    $"**Assists**: {player.Stats.Assists}\n" +
                //    $"**Saves**: {player.Stats.Saves}\n" +
                //    $"**Shots**: {player.Stats.Shots}";
                //if (playerSeasonFive.Value != null)
                //{
                //    foreach (var playerRank in playerSeasonFive.Value)
                //    {
                //        var displayTier = tier.FirstOrDefault(x => x.Id == playerRank.Value.Tier);

                //        msg += $"**{playerRank.Key}**: {displayTier.Name} Div. {playerRank.Value.Division + 1}\n" +
                //            $"\n";
                //    }
                //}

                //var embed1 = new EmbedBuilder()
                //.WithColor(new Color(color))
                //.WithTitle(player.DisplayName)
                //.WithDescription($"For more info head over to [Rocket League tracker](https://rocketleague.tracker.network/profile/steam/{steamid})")
                //.WithThumbnailUrl(player.Avatar)
                //.AddInlineField("Stats", stats)
                //.AddInlineField("Ranking", msg);
                //await Context.Channel.SendMessageAsync("", embed: embed1);
                await Context.Channel.SendMessageAsync("http://signature.rocketleaguestats.com/normal/steam/"+player.UniqueId+".png");

            }
        }
    }
}
