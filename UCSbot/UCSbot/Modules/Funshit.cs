using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;


namespace UCSbot.Modules
{
    public class Funshit : ModuleBase
    {
        uint color = 3901951;
        [Command("say"), Summary("Echos a message. !say [message]")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            if(echo == null)
            {
                var embed1 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify what i should say... !say [message]");
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                var embed = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription(echo);
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
        }

        [Command("flipcoin"), Summary("Flips a coin")]
        public async Task flipcoin()
        {
            string msg = "Flips a coin...";
            var embed = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription(msg);
            await Context.Channel.SendMessageAsync("", embed: embed);

            await Task.Delay(2000);
            Random rnd = new Random();
            int side = rnd.Next(1, 10);

            if (side > 5)
            {
                string msg1 = "Aaaand its Heads!";
                var embed1 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg1);
                await Context.Channel.SendMessageAsync("", embed: embed1);
            }
            else
            {
                string msg2 = "Aaaand its Tails!";
                var embed2 = new EmbedBuilder()
                    .WithColor(new Color(color))
                    .WithDescription(msg2);
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
        }

        [Command("roll"), Summary("Rolls a dice. !roll [maxvalue]")]
        public async Task Roll(int maxvalue = 0)
        {
            if(maxvalue < 0)
            {
                var embed3 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("Value cannot be negative! just like your attitude <3");
                await Context.Channel.SendMessageAsync("", embed: embed3);
            }

            if (maxvalue == 0)
            {
                var embed2 = new EmbedBuilder()
                .WithColor(new Color(color))
                .WithDescription("You need to specify the maximum value ... !roll [maxvalue]");
                await Context.Channel.SendMessageAsync("", embed: embed2);
            }
            if (maxvalue > 0)
            {
                if(maxvalue > 2000)
                {
                    string msg5 = "The number is too big... That's what she said. hue";
                    var embed5 = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg5);
                    await Context.Channel.SendMessageAsync("", embed: embed5);
                }
                else
                {
                    string msg = "Rolls a dice...";
                    var embed = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg);
                    await Context.Channel.SendMessageAsync("", embed: embed);
                    await Task.Delay(2000);

                    Random rnd = new Random();
                    int roll = rnd.Next(1, maxvalue);

                    string msg1 = "" + roll;
                    var embed1 = new EmbedBuilder()
                        .WithColor(new Color(color))
                        .WithDescription(msg1);
                    await Context.Channel.SendMessageAsync("", embed: embed1);
                }
            }
        }
    }
}
