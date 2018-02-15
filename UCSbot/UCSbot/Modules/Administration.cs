using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace UCSbot.Modules
{
    public class Administration : ModuleBase
    {
        uint color = 3901951;
        uint red = 15597568;
        ulong logchannel = 338422549673410561;
        [Command("delete", RunMode = RunMode.Async)]
        [Summary("Deletes the specified amount of messages. !delete [amount]")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task DeleteMsgs(int amount = 0)
        {

            if(amount > 100)
            {
                var embed2 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription("Message purging is limited to 100 at a time!");
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
            if(amount < 0)
            {
                var embed1 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription("Howbout you try deleting negative amount of messages!");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            if(amount == 0)
            {
                    var embed7 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription("Can't delete zero messages, it's like saying you did your homework but you didn't do anything. I did it just now, see? nothing happened.");
                    await Context.Channel.SendMessageAsync("", embed: embed7);
            }
            if(amount < 100 && amount > 0)
            {
                var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messages);
                const int delay = 3000;
                string msg = $"Messages purged, this message will be deleted in {delay} seconds";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                var m = await Context.Channel.SendMessageAsync("", embed: embed);
                var x = m.CreatedAt;
                await Task.Delay(delay);
                await m.DeleteAsync();

                var author = Context.Message.Author;
                var log = await Context.Guild.GetChannelAsync(logchannel) as IMessageChannel;

                var footer = new EmbedFooterBuilder()
                            .WithText(x.ToString());
                var embed4 = new EmbedBuilder()
                          .WithColor(new Color(red))
                          .AddField("Messages Deleted", $"{author.Mention} Deleted **{amount}** messages in **{Context.Channel.Name}** channel")
                          .WithThumbnailUrl(author.GetAvatarUrl())
                          .WithFooter(footer);
                await log.SendMessageAsync("",embed: embed4);
            }
        }

        [Command("cvc", RunMode = RunMode.Async)]
        [Summary("Creates voice channels with given name. !cvc [amount] [name]")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]

        public async Task CreateVC(int amount, [Remainder]string name) //remainder processes multi args (white spaces)
        {
            Console.WriteLine(amount);

            if(amount == 0 | name == null)
            {
                var embed1 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription("You need to specify the amount and name of the channel... !cvc [amount] [name]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                int x = 0;
                if (amount > 50)
                {
                    string msg = "The amount is limited to 50";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
                else
                {
                    while (x < amount)
                    {
                        await Context.Guild.CreateVoiceChannelAsync(name);
                        x++;
                    }

                    string msg = $"Voice channels **{name}** created!";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    var m = await Context.Channel.SendMessageAsync("", embed: embed);

                    var author = Context.Message.Author;
                    var log = await Context.Guild.GetChannelAsync(logchannel) as IMessageChannel;

                    var footer = new EmbedFooterBuilder()
                                .WithText(m.CreatedAt.ToString());
                    var embed4 = new EmbedBuilder()
                              .WithColor(new Color(red))
                              .AddField("Channels created", $"{author.Mention} Created **{amount}** channels with name: **{name}**")
                              .WithThumbnailUrl(author.GetAvatarUrl())
                              .WithFooter(footer);
                    await log.SendMessageAsync("", embed: embed4);
                }
            }
        }

        [Command("dvc", RunMode = RunMode.Async)]
        [Summary("Deletes voice channels. !dvc [name]")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]

        public async Task DeleteVC([Remainder]string name)
        {
            var allChannels = await Context.Guild.GetVoiceChannelsAsync();
            var nochannel = allChannels.Where(x => x.Name == name);
            if(nochannel.Count() == 0)
            {
                string msg = "There are no voice channels with that name";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else
            {
                allChannels.Where(x => x.Name == name).ToList().ForEach(async (x) =>
                {
                    await x.DeleteAsync();
                });
                string msg = $"Voice channels **{name}** deleted!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                var m = await Context.Channel.SendMessageAsync("", embed: embed);

                var author = Context.Message.Author;
                var log = await Context.Guild.GetChannelAsync(logchannel) as IMessageChannel;

                var footer = new EmbedFooterBuilder()
                            .WithText(m.CreatedAt.ToString());
                var embed4 = new EmbedBuilder()
                          .WithColor(new Color(red))
                          .AddField("Channels deleted", $"{author.Mention} Deleted channels with name: **{name}**")
                          .WithThumbnailUrl(author.GetAvatarUrl())
                          .WithFooter(footer);
                await log.SendMessageAsync("", embed: embed4);
            }
        }

        [Command("setnick"), Summary("Sets nickname to a user. !setnick [user] [nickname]")]
        [RequireUserPermission(GuildPermission.ManageNicknames)]
        [RequireBotPermission(GuildPermission.ManageNicknames)]
        public async Task setnick(IGuildUser user, [Remainder]string nickname)
        {
            await (user as IGuildUser)?.ModifyAsync(x =>
            {
                    x.Nickname = $"{nickname}";
                    string msg = "Nickname changed!";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    Context.Channel.SendMessageAsync("", embed: embed);
            });
        }

        [Command("mentionable", RunMode = RunMode.Async)]
        [Summary("Toggle making a role mentionable on/off. !mentionable [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]

        public async Task mentionable([Remainder]string role)
        {
            var allRoles = Context.Guild.Roles;
            var roletotoggle = allRoles.Where(x => x.Name == role);
            var mentionableX = allRoles.Where(x => x.Name == role).Any(roleXx => roleXx.IsMentionable);
            var checker = allRoles.Where(x => x.Name == role);

            if(checker.Count() != 0)
            {
                if (mentionableX == true)
                {
                    await Task.WhenAll(roletotoggle.Select(roleX => roleX.ModifyAsync(x =>
                    {
                        x.Mentionable = false;
                    })));
                    string msg = $"Role **{role}** is not mentionable anymore!";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
                else
                {
                    await Task.WhenAll(roletotoggle.Select(roleX => roleX.ModifyAsync(x =>
                    {
                        x.Mentionable = true;
                    })));
                    string msg2 = $"Role **{role}** is now mentionable!";
                    var embed2 = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg2);
                    await Context.Channel.SendMessageAsync("", embed: embed2);
                }
            }
            else
            {
                string msg3 = $"Yea i don't know about that one man, seems like {role} doesn't exist in this server. Bummer";
                var embed3 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg3);
                await Context.Channel.SendMessageAsync("", embed: embed3);
            }
        }

        [Command("delrole", RunMode = RunMode.Async)]
        [Summary("Deletes a role. !delrole [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]

        public async Task delrole([Remainder]string role)
        {
            var allRoles = Context.Guild.Roles;
            var roletodelete = allRoles.Where(x => x.Name == role);
            if(roletodelete.Count() != 0)
            {
                await Task.WhenAll(roletodelete.Select(rolex => rolex.DeleteAsync()));
                string msg2 = $"Role **{role}** deleted!";
                var embed2 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg2);
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
            else
            {
                string msg = $"Role {role} cannot be delete, because it doesn't exist in this server, therefore it cannot be deleted... because it doesn't exist... so i won't delete it.";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }

        [Command("roleadd")]
        [Summary("Add role to user. !roleadd [user] [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task roleadd(IGuildUser user, [Remainder]string role)
        {
            var allRoles = Context.Guild.Roles;
            var roletoadd = allRoles.First(x => x.Name == role);
            var userR = user ?? await Context.Guild.GetCurrentUserAsync();
            if (roletoadd != null)
            {
                await userR.AddRoleAsync(roletoadd);
                string msg = $"Role **{role}** added to {userR.Username}!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else
            {
                string msg2 = $"Yea i don't think that role exists dude";
                var embed2 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg2);
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
            
        }

        [Command("roleremove")]
        [Summary("Remove role from user. !roleremove [user] [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task roleremove(IGuildUser user, [Remainder]string role)
        {
            var allRoles = Context.Guild.Roles;
            var roletoremove = allRoles.First(x => x.Name == role);
            var userR = user ?? await Context.Guild.GetCurrentUserAsync();
            if(roletoremove != null)
            {
                await userR.RemoveRoleAsync(roletoremove);
                string msg = $"Role **{role}** removed from {userR.Username}!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else
            {
                await userR.RemoveRoleAsync(roletoremove);
                string msg2 = $"Role {role} is out of my jurisdiction. It's just a nice way to say you messed up your spelling...hue";
                var embed2 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg2);
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
        }

        [Command("roleall")]
        [Summary("Remove role from all users. !roleall [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task roleall([Remainder]string role)
        {
            var allRoles = Context.Guild.Roles;
            var rolex = allRoles.First(x => x.Name == role);
            var usersAll = await Context.Guild.GetUsersAsync();
            if(rolex != null)
            {
                await Task.WhenAll(usersAll.Select(u => u.RemoveRoleAsync(rolex)));
                string msg = $"Role **{role}** removed from all users!";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else
            {
                string msg2 = $"That is some weird named role. Try again please";
                var embed2 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg2);
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
        }
    }
}
