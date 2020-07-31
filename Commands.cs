using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.Net;

namespace MayoiBot
{
    [Category("Commands")]
    [DSharpPlus.CommandsNext.Attributes.Description("Commands for everyone")]
    [Cooldown(1, 10, CooldownBucketType.Guild)]

    public class Commands : BaseCommandModule
    {
        [Command("hi")]
        [DSharpPlus.CommandsNext.Attributes.Description("Gives a hearty hello!")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!\n{ctx.User.AvatarUrl}");
        }

        [Command("info")]
        public async Task Info(CommandContext ctx)
        {
            var builder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.White,
                Title = ctx.Client.CurrentUser.Username + " bot info:",
                Description =
                    "This bot is open source and available [here](https://github.com/cataclym/Mayoi-Onee-san.git 'GitHub')!"
            };

            builder.AddField("Language", "C#", true);
            builder.AddField(".NET wrapper", "[D#+](https://github.com/DSharpPlus/DSharpPlus/ 'DSharpPlus')", true);
            builder.AddField("Author", "Cata", true);
            builder.WithImageUrl("https://dsharpplus.emzi0767.com/logo.png");
            builder.WithThumbnail(
                "https://cdn.discordapp.com/attachments/717045059215687691/735462333786095626/C_Sharp_logo.png");

            await ctx.RespondAsync(embed: builder);
        }

        [Command("random")]
        public async Task Random(CommandContext ctx, int min, int max)
        {
            var number = new Random();
            await ctx.Channel.SendMessageAsync("Returned: " + number.Next(min, max));
        }

        private static string EitherNr(int x)
        {
            string[] arr = {" is not very smart!", " is very smart!"};
            return arr[x];
        }

        [Command("smart")]
        public async Task
            Smart(CommandContext ctx, DiscordMember member) // Mention isnt any string. Not just mentions. Help.
        {
            try
            {
                var rndm = new Random();
                var yn = rndm.Next(0, 101);
                if (yn >= 50)
                {
                    yn = 1;
                }
                else
                {
                    yn = 0;
                }

                await ctx.Channel.SendMessageAsync(member.Mention + EitherNr(yn));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("8ball")]
        public async Task EightBall(CommandContext ctx, [RemainingText] string args)
        {
            string[] ebArray =
            {
                "It is certainly possible!", "Yes, I believe so!", "Maybe...",
                "I hope not!", "Yes... Maybe?", "Do you actually think that is possible??",
                "I pity you for even asking."
            };
            var rndm = new Random();
            var nmbr = rndm.Next(0, ebArray.Length);
            await ctx.Channel.SendMessageAsync(ebArray[nmbr]);
        }

        [Command("user")]
        public async Task User(CommandContext ctx, DiscordUser member = null)
        {
            if (member is null)
            {
                member = ctx.User;
            } 
            var emb = new DiscordEmbedBuilder 
            {
                Color = DiscordColor.Gold,
                Title = "User info of: " + member.Username,
                ImageUrl = member.AvatarUrl,
                Description =
                    member.Mention +
                    "\nUser Name: " + member.Username + "#" + member.Discriminator +
                    "\nUser Created: " + member.CreationTimestamp +
                    "\nID: " + member.Id +
                    "\nVerification: (?) " + member.Verified +
                    "\nPresence: " + member.Presence?.Status + " | `" + member.Presence?.Activity?.Name + "` `" +
                    member.Presence?.Activity?.CustomStatus +
                    " `\nAdditional: (?) `" + member.Presence?.Activity?.RichPresence?.Application?.Name + " `:` " +
                    member.Presence?.Activity?.RichPresence?.Details +
                    " `\nStreaming: (?) " + member.Presence?.Activity?.StreamUrl
            };
        await ctx.Channel.SendMessageAsync(embed: emb);
        }

        [Command("tictactoe")]
        [Aliases("ttt")]

        public async Task TicTacToe(CommandContext ctx, DiscordMember member)
        {
            var circle = DiscordEmoji.FromName(ctx.Client, ":o:");
            var cross = DiscordEmoji.FromName(ctx.Client, ":x:");
            var agree = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var disagree = DiscordEmoji.FromName(ctx.Client, ":-1:"); 
            Embeds.TttEmbed.WithDescription(member.Mention + " react with a 👍 to start game! Or 👎 to cancel.");
            var x = await ctx.RespondAsync(embed: Embeds.TttEmbed);
            await x.CreateReactionAsync(agree).ConfigureAwait(false);
            await x.CreateReactionAsync(disagree).ConfigureAwait(false);

            var msg = x;
            var y = await msg.WaitForReactionAsync(member, TimeSpan.FromSeconds(45));
            var res = y;
            if (res.Result?.Emoji == agree)
            {
                await x.DeleteAllReactionsAsync();
                await x.ModifyAsync(
                    "Let's begin!\nType numbers 1-9 to make a move\n" + 
                    circle + " represents: " + member.Mention + " (This player begins)\n" +
                    cross + " represents: " + ctx.Member.Mention,
                    embed: null);
                await TTT(ctx, member);
            }
            else if (res.Result?.Emoji == disagree)
            {
                await x.DeleteAllReactionsAsync();
                await x.ModifyAsync(embed: Embeds.Cancelled);
            }
            else
            {
                await x.DeleteAllReactionsAsync();
                await x.ModifyAsync(embed: Embeds.TimeOut);
            }
        }

        private async Task TTT(CommandContext ctx, SnowflakeObject member)
        {
            try
            {
                bool isAuthorTurn = false;
                var moves = new List<int>();
                var interactivity = ctx.Client.GetInteractivity();
                var placeholder = DiscordEmoji.FromName(ctx.Client, ":white_large_square:");
                var circle = DiscordEmoji.FromName(ctx.Client, ":o:");
                var cross = DiscordEmoji.FromName(ctx.Client, ":x:");
                var tic = await ctx.Channel.SendMessageAsync(
                    placeholder + placeholder + placeholder + "\n" +
                    placeholder + placeholder + placeholder + "\n" +
                    placeholder + placeholder + placeholder
                );
                object[] arr = new object[tic.Content.Length];
                int i = 0;
                foreach (var chr in tic.Content)
                {
                    arr.SetValue(chr, i);
                    i++;
                }
                await Inputs();
                async Task Inputs()
                {
                    var response = await interactivity.WaitForMessageAsync(
                        x => x.Channel == ctx.Message.Channel &&
                             (x.Author.Id == ctx.Member.Id || x.Author.Id == member.Id) &&
                             x.Content.Length < 2 && 
                             int.TryParse(x.Content, out int nr) && 
                             nr != 0,
                        TimeSpan.FromSeconds(30));
                    int.TryParse(response.Result.Content, out int n);
                    
                    n--;
                    if (n > 2) n++;
                    if (n > 6) n++; // skip newline                    
                    if (moves.Contains(n))
                    {
                        await MoveTaken();
                    }
                    else if (response.Result.Author.Id == member.Id && isAuthorTurn == false) // Tagged
                    {
                        moves.Add(n);
                        await Blue();
                        await response.Result.DeleteAsync();
                        isAuthorTurn = true;
                    }
                    else if (response.Result.Author.Id == ctx.Member.Id && isAuthorTurn) // Author
                    {
                        moves.Add(n);
                        await Green();
                        await response.Result.DeleteAsync();
                        isAuthorTurn = false;
                    }
                    else
                    {
                        await ctx.RespondAsync("It's not your turn!");
                    }
                    await Inputs();
                    
                    async Task Blue()
                    {                  
                        arr[n] = circle;
                        string newmsg = arr.Aggregate("", (current, s) => current + s);
                        await tic.ModifyAsync(newmsg);
                    }

                    async Task Green()
                    {                  
                        arr[n] = cross;
                        string newmsg = arr.Aggregate("", (current, s) => current + s);
                        await tic.ModifyAsync(newmsg);
                    }

                    async Task MoveTaken()
                    {
                        var moveTakenMsg = await ctx.RespondAsync("This move has already been done! " + response.Result.Author.Mention);
                        await Task.Delay(5000);
                        await response.Result.DeleteAsync("Move Taken | TTT");
                        await moveTakenMsg.DeleteAsync($"Move taken | TicTacToe move by {response.Result.Author.Username}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("ping")]
        [Aliases("p")]
        public async Task Ping(CommandContext ctx)
        {
            var msg = await ctx.RespondAsync("Pinging!!!!???"); 
            var info = DiscordEmoji.FromName(ctx.Client, ":information_source:");
            var wsocket = ctx.Client.Ping;
            var messageTime = msg.CreationTimestamp - ctx.Message.CreationTimestamp;
            await msg.ModifyAsync(
                info + " WebSocket ping took " + wsocket + " ms" +
                "\n" + info + " Client ping took " + messageTime.Milliseconds + " ms" 
                );
        }
    }
}