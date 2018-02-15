using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using UCSbot.Modules;

namespace UCSbot.Modules
{
    public class Help : ModuleBase
    {
        uint color = 3901951;
        [Command("help"), Summary("Generates list of available commands... !help [section]")]
        public async Task help(string section1 = null)
        {
            string one = "admin";
            string two = "ranks";
            string three = "fun";
            string four = "info";
            string five = "music";
            string section;
            var serverimage = Context.Guild.IconUrl;
            if (section1 == null)
            {
                var embed1 = new EmbedBuilder()
                .WithTitle("__Help__")
                .WithColor(new Color(color))
                .WithDescription("Choose a section you would like to know more about **[admin, ranks, fun, info, music]** using !help [section]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }           
            string field1 = "!delete [amount] **~Deletes the specified amount of messages.**\n" +
                "!setnick [user] [nickname] **~Sets nickname to a user.**\n" +
                "!mentionable [role] **~Toggle making a role mentionable on/off.**\n" +
                "!delrole [role] **~Deletes a role.**\n" +
                "!roleadd [user] [role] **~Adds role to user.**\n" +
                "!roleremove [user] [role] **~Removes role from user.**\n" +
                "!roleall [role] **~Removes role from all users.**\n" +
                "!cvc [amount] [name] **~Creates voice channels with given name.**\n" +
                "!dvc [name] **~Deletes all voice channels of certain name.**\n" +
                "!mute [user] **~Mutes user.**\n" +
                "!unmute [user] **~Unmutes user.**\n" +
                "!deaf [user] **~Deafs user.**\n" +
                "!undeaf [user] **~Undeafs user.**\n" +
                "!ban [user] **~Bans user.**\n" +
                "!unban [user] **~Unbans user.**\n" +
                "!kick [user] **~Kicks user.**\n" +
                "!tchange [message] **~Changes `!tournament` message**";
            string field2 = "!ranks **~Shows list of available ranks**\n" +
                "!rank [name] **~Join or leave a rank**\n" +
                "!addrank [name] **~Creates a rank**\n" +
                "!delrank [name] **~Deletes a rank**";
            string field3 = "!rps [rock,paper,scissors] **~Rock Paper Scissors with the bot.**\n" +
                "!say [message] **~Echos a message.**\n" +
                "!flipcoin **~Flips a coin**\n" +
                "!roll [max value] **~Rolls a dice**\n" +
                "!randomcolor **~Picks random hex color with preview**";
            string field4 = "!server **~Shows server info**\n" +
                "!ping **~Check if bot is responding**\n" +
                "!user [user] **~Shows user info**\n" +
                "!members **~Shows current amount of members in server**\n" +
                "!listmods **~Shows list of available Moderators**\n" +
                "!training **~Shows kimble's training packs**\n" +
                "!uptime **~Shows uptime of the bot**\n" +
                "!donate **~Shows donation links**\n" +
                "!tournament **~Upcoming tournament date and signup link**\n" +
                "!twitch **~Links UCS twitch channel**\n" +
                "!twitter **~Yeah we have that too**\n" +
                "!ts **~Shows tournament start time and time zones converter**\n" +
                "!rlstats [steamid] **~Checks ranking of a player**";
            string field5 = "!join **~Joins a voice channel you are currently in**\n" +
                "!leave **~Leaves voice channel**\n" +
                "!play [song] **~Plays desired song/puts songs in queue**\n" +
                "!queue **~Shows song queue**\n" +
                "!stop **~Stops the player**\n" +
                "!next **~Skips a song**";

            if (section1 != null)
            {
                section = section1.ToLower();
                if (section == one)
                {
                    var embed2 = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .AddInlineField("__Administration/Moderator__", field1)
                        .WithThumbnailUrl(serverimage);
                    await Context.Channel.SendMessageAsync("", embed: embed2);
                }
                if (section == two)
                {
                    var embed3 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .AddInlineField("__Ranks__", field2)
                    .WithThumbnailUrl(serverimage);
                    await Context.Channel.SendMessageAsync("", embed: embed3);
                }
                if (section == three)
                {
                    var embed4 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .AddInlineField("__Fun__", field3)
                    .WithThumbnailUrl(serverimage);
                    await Context.Channel.SendMessageAsync("", embed: embed4);
                }
                if (section == four)
                {
                    var embed5 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .AddInlineField("__Info__", field4)
                    .WithThumbnailUrl(serverimage);
                    await Context.Channel.SendMessageAsync("", embed: embed5);
                }
                if (section == five)
                {
                    var embed6 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithThumbnailUrl(serverimage)
                    .AddInlineField("__Music Player__", field5);
                    await Context.Channel.SendMessageAsync("", embed: embed6);
                }

                if (section != one & section != two & section != three & section != four & section != five & section != null)
                {
                    var embed1 = new EmbedBuilder()
                    .WithTitle("__Help__")
                    .WithColor(new Color(color))
                    .WithDescription($"There is no section called **{section1}**");
                    await Context.Channel.SendMessageAsync("", embed: embed1);
                }
            }
        }
    }
}
