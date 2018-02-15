using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;


namespace UCSbot.Modules
{
    public class Moderator : ModuleBase
    {
        uint color = 3901951;
        [Command("mute")]
        [Summary("Mutes user. !mute [user]")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(GuildPermission.MuteMembers)]
        public async Task mute(IGuildUser user = null)
        {
                if (user == null)
                {
                    var embed1 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription("You need to specify a user ... !mute [user]");
                    await Context.Channel.SendMessageAsync("", embed: embed1);
                }
                else
                {
                    await (user as IGuildUser)?.ModifyAsync(x =>
                    {
                        x.Mute = true;
                    });
                    string msg = $"User **{user.Username}** is now muted!";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
        }

        [Command("unmute")]
        [Summary("Unmutes user. !unmute [user]")]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        [RequireBotPermission(GuildPermission.MuteMembers)]
        public async Task unmute(IGuildUser user = null)
        {
            if (user == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify a user ... !unmute [user]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var userR = user ?? await Context.Guild.GetCurrentUserAsync();
                await (userR as IGuildUser)?.ModifyAsync(x =>
                {
                    x.Mute = false;
                });
                string msg = $"User **{userR.Username}** can speak things again!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }

        [Command("deaf")]
        [Summary("Deafs user. !deaf [user]")]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        [RequireBotPermission(GuildPermission.DeafenMembers)]
        public async Task deaf(IGuildUser user = null)
        {
            if (user == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify a user ... !deaf [user]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var userR = user ?? await Context.Guild.GetCurrentUserAsync();

                await (userR as IGuildUser)?.ModifyAsync(x =>
                {
                    x.Deaf = true;
                });
                string msg = $"User **{userR.Username}** is now deaf!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }

        [Command("undeaf")]
        [Summary("Undeafs user. !undeaf [user]")]
        [RequireUserPermission(GuildPermission.DeafenMembers)]
        [RequireBotPermission(GuildPermission.DeafenMembers)]
        public async Task undeaf(IGuildUser user = null)
        {
            if (user == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify a user ... !undeaf [user]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var userR = user ?? await Context.Guild.GetCurrentUserAsync();

                await (userR as IGuildUser)?.ModifyAsync(x =>
                {
                    x.Deaf = false;
                });
                string msg = $"User **{userR.Username}** can hear things again!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            
        }

        [Command("ban")]
        [Summary("Bans user. !ban [user]")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task ban(IGuildUser user = null)
        {
            if (user == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify a user ... !ban [user]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var userR = user ?? await Context.Guild.GetCurrentUserAsync();

                await userR.Guild.AddBanAsync(user);
                string msg = $"User **{userR.Username}** is now banned, bye bye!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            
        }

        [Command("unban")]
        [Summary("unbans user. !unban [user]")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task unban(IGuildUser user = null)
        {
            if (user == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify a user ... !unban [user]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var userR = user ?? await Context.Guild.GetCurrentUserAsync();
                var banlist = await Context.Guild.GetBansAsync();
                var unbanuser = banlist.FirstOrDefault(x => x.User.Username == user.Username);
                var userid = unbanuser.User.Id;
                await userR.Guild.RemoveBanAsync(userid);
                string msg = $"User **{userR.Username}** is now unbanned!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            
        }

        [Command("kick")]
        [Summary("Kicks user. !kick [user]")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task kick(IGuildUser user = null)
        {
            if (user == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify a user ... !kick [user]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var userR = user ?? await Context.Guild.GetCurrentUserAsync();
                string msg = $"User **{userR.Username}** was kicked!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
                await userR.KickAsync();
            }
        }
    }
}
