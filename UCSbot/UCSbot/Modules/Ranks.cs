using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;

namespace UCSbot.Modules
{
    public class Ranks : ModuleBase
    {
        uint color = 3901951;
        RankFilePath _path = new RankFilePath();
        [Command("addrank"), Summary("Adds a rank. !addrank [name]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task addrank([Remainder] string nameUp)
        {
            try
            {
                string name = nameUp.ToLower();
                var lines = File.ReadAllLines(_path.RankFile);
                var count = lines.Where(x => x.Equals(name));
                if(count.Count() == 0)
                {
                    await Context.Guild.CreateRoleAsync(name);
                    using (StreamWriter sw = File.AppendText(_path.RankFile))
                    {
                        sw.WriteLine(name);
                        sw.Close();
                    }
                    string msg = $"Rank **{name}** created";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
                else
                {
                    var embed1 = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription($"Rank {name} already exists!");
                    await Context.Channel.SendMessageAsync("", embed: embed1);
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        [Command("delrank"), Summary("Deletes a rank. !delrank [name]")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task delrank([Remainder] string nameUp)
        {
            try
            {
                string name = nameUp.ToLower();
                var lines = File.ReadAllLines(_path.RankFile);
                var count = lines.Where(x => x.Equals(name));
                var deleteL = lines.Where(x => !x.Equals(name));
                if(count.Count() != 0)
                {
                    File.WriteAllLines(_path.RankFile, deleteL);
                    var allRoles = Context.Guild.Roles;
                    var roletodelete = allRoles.Where(x => x.Name == name);
                    await Task.WhenAll(roletodelete.Select(rolex => rolex.DeleteAsync()));
                    string msg = $"Rank **{name}** deleted";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
                if(count.Count() == 0)
                {
                    string msg2 = $"Rank **{name}** doesn't exist.";
                    var embed2 = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg2);
                    await Context.Channel.SendMessageAsync("", embed: embed2);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Command("ranks"), Summary("Shows available ranks")]
        public async Task ranks()
        {
            var lines = File.ReadAllLines(_path.RankFile);
            if (lines.Count() == 0)
            {
                string msg = "There are no ranks available";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithTitle("**Ranks:**")
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            string s = "**Ranks:**\n";
            string e = "";
            if(lines.Count() > 0)
            {
                for (int index = 0; index < lines.Length; index++)
                {
                    e += String.Format("{0,-1}", lines[index]);
                    var users = await Context.Guild.GetUsersAsync();
                    var roles = Context.Guild.Roles.FirstOrDefault(x => x.Name == lines[index]).Id;
                    var count = users.Where(x => x.RoleIds.Contains(roles)).Count();

                    e += $" - **{count}** members\n";
                }
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithTitle(s)
                    .WithDescription(e);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }

        [Command("rank"), Summary("Joins or Leaves a rank. !rank [name]")]
        public async Task rank([Remainder] string nameUp)
        {
            string name = nameUp.ToLower();
            var allRoles = Context.Guild.Roles;
            var roletoadd = allRoles.First(x => x.Name == name);
            var author = Context.Message.Author.Id;
            var userA = await Context.Guild.GetUserAsync(author);
            var rolecheck = userA.RoleIds.Where(x => x.Equals(roletoadd.Id));
            if(rolecheck.Count() == 0)
            {
                await userA.AddRoleAsync(roletoadd);
                string msg = $"**{userA.Username}** added to rank **{roletoadd.Name}**";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else
            {
                await userA.RemoveRoleAsync(roletoadd);
                string msg = $"**{userA.Username}** removed from the rank **{roletoadd.Name}**";
                var embed = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }

        }
    }
}
