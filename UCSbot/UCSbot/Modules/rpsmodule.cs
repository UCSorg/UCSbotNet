using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UCSbot.Modules
{
    public class rpsmodule : ModuleBase
    {
        [Command("rps"), Summary("Rock Paper Scissors with the bot. !rps [choice]")]
        public async Task rps(string choiceUp)
        {
            string choice = choiceUp.ToLower();
            Random r = new Random();
            int computerChoice = r.Next(3);

            if (computerChoice == 1)
            {
                if (choice == "rock")
                {
                    await ReplyAsync("I chose rock");
                    await ReplyAsync("It is a tie!");
                }
                else if (choice == "paper")
                {
                    await ReplyAsync("I chose paper");
                    await ReplyAsync("It is a tie!");

                }
                else if (choice == "scissors")
                {
                    await ReplyAsync("I chose scissors");
                    await ReplyAsync("It is a tie!");
                }
                else
                {
                    await ReplyAsync("You must choose rock,paper or scissors!");

                }

            }

            else if (computerChoice == 2)
            {
                if (choice == "rock")
                {
                    await ReplyAsync("I chose paper");
                    await ReplyAsync("Sorry you lose,paper beat rock!");

                }
                else if (choice == "paper")
                {
                    await ReplyAsync("I chose scissors");
                    await ReplyAsync("Sorry you lose,scissors beat paper!");

                }
                else if (choice == "scissors")
                {
                    await ReplyAsync("I chose rock");
                    await ReplyAsync("Sorry you lose,rock beats scissors!");
                }
                else
                {
                    await ReplyAsync("You must choose rock,paper or scissors!");
                }
            }
            else if (computerChoice == 0)
            {
                if (choice == "rock")
                {
                    await ReplyAsync("I chose scissors");
                    await ReplyAsync("You win,rock beats scissors!");

                }
                else if (choice == "paper")
                {
                    await ReplyAsync("I chose rock");
                    await ReplyAsync("You win,paper beats rock!");

                }
                else if (choice == "scissors")
                {
                    await ReplyAsync("I chose paper");
                    await ReplyAsync("You win,scissors beat paper!");

                }
                else
                {
                    await ReplyAsync("You must choose rock,paper or scissors!");

                }

            }
        }
    }
}
