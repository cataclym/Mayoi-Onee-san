using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;

namespace MayoiBot
{
    [Category("Commands")]
    [DSharpPlus.CommandsNext.Attributes.Description("Commands for everyone")]
    [Cooldown(2, 4, CooldownBucketType.Guild)]

    public class Commands : BaseCommandModule
    {
        [Command("hi")]
        [DSharpPlus.CommandsNext.Attributes.Description("Gives a hearty hello!")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
        }

        [Command("info")]
        public async Task Info(CommandContext ctx)
        {
            var builder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.White,
                Title = ctx.Client.CurrentUser.Username + " bot info:",
                Description = "This bot is open source and available on [GitHub](https://github.com/cataclym/Mayoi-Onee-san.git 'GitHub')!",
            };

            builder.AddField("D-API .NET wrapper", "[D#+](https://github.com/DSharpPlus/DSharpPlus/ 'DSharpPlus')", true);
            builder.AddField("Author", "Cata", true);
            builder.WithImageUrl("https://dsharpplus.emzi0767.com/logo.png");

            await ctx.RespondAsync(embed: builder);
        }

        [Command("random")]
        [Aliases("rng")]
        public async Task Random(CommandContext ctx, int min = 0, int max = 100)
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
            Smart(CommandContext ctx, DiscordMember member = null)
        {
            member ??= ctx.Member;  
                var randomNumber = new Random();
                int yn = randomNumber.Next(0, 101);
                yn = yn >= 50 ? 1 : 0;
                await ctx.Channel.SendMessageAsync(member.Mention + EitherNr(yn));
        }

        [Command("8ball")]
        public async Task EightBall(CommandContext ctx, [RemainingText] string arg)
        {
            string[] ebArray =
            {
                "It is certainly possible!", "Yes, I believe so!", "Maybe...",
                "I hope not!", "Yes... Maybe?", "Do you actually think that is possible??",
                "I pity you for even asking."
            };
            var randomNumber = new Random();
            int number = randomNumber.Next(0, ebArray.Length);
            await ctx.Channel.SendMessageAsync(ebArray[number]);
        }
        [Command("userinfo")]
        [Aliases("uinfo")]
        public async Task UserInfo (CommandContext ctx, DiscordMember member = null)
        {   
            member ??= ctx.Member;
            var emb = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Gold,
                Title = "User info of **" + member.Username + "**#" + member.Discriminator,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail {Url = member.AvatarUrl},
            };
            emb.AddField("Displayname", member.DisplayName, true);
            emb.AddField("GuildOwner", $"{member.IsOwner}", true);
            emb.AddField("ID",$"{member.Id}", true);
            emb.AddField("Join Date", $"{member.JoinedAt}", true);
            emb.AddField("User Created", $"{member.CreationTimestamp}", true);
            emb.AddField("Verification", "(?) " + $"{member.Verified}", true);
            emb.AddField("Presence", "(?) " +$"{member.Presence?.Status}\n" +
                                     $"{member.Presence?.Activity?.Name}\n" +
                                     $"{member.Presence?.Activity?.RichPresence?.Application?.Name}\n" +
                                     $"{member.Presence?.Activity?.CustomStatus?.Emoji} {member.Presence?.Activity?.CustomStatus?.Name}", true);
            emb.AddField("RichPresence", "(?) " + $"{member.Presence?.Activity?.RichPresence?.Details}", true);
            emb.AddField("Streaming", "(?) " + $"{member.Presence?.Activity?.StreamUrl}", true);
            await ctx.Channel.SendMessageAsync(embed: emb);
        }

        [Command("tictactoe")]
        [Aliases("ttt")]

        public async Task TicTacToe(CommandContext ctx, DiscordMember member)
        {
            DiscordEmoji circle = DiscordEmoji.FromName(ctx.Client, ":o:");
            DiscordEmoji cross = DiscordEmoji.FromName(ctx.Client, ":x:");
            DiscordEmoji agree = DiscordEmoji.FromName(ctx.Client, ":+1:");
            DiscordEmoji disagree = DiscordEmoji.FromName(ctx.Client, ":-1:"); 
            DiscordMessage x = await ctx.RespondAsync(embed: Embeds.TttEmbed);
            Embeds.TttEmbed.WithDescription(member.Mention + " react with a 👍 to start game! Or 👎 to cancel.");
            await x.CreateReactionAsync(agree).ConfigureAwait(false);
            await x.CreateReactionAsync(disagree).ConfigureAwait(false);

            InteractivityResult<MessageReactionAddEventArgs> y = await x.WaitForReactionAsync(member, TimeSpan.FromSeconds(45));
            if (y.Result?.Emoji == agree)
            {
                await x.DeleteAllReactionsAsync();
                await x.ModifyAsync(
                    "Let's begin!\nType numbers 1-9 to make a move\n" + 
                    circle + " represents: " + member.Mention + " (This player begins)\n" +
                    cross + " represents: " + ctx.Member.Mention,
                    embed: null);
                await TTT(ctx, member);
            }
            else if (y.Result?.Emoji == disagree)
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

        private static async Task TTT(CommandContext ctx, SnowflakeObject member)
        {
            var isAuthorTurn = false;
                var moves = new List<int>();
                InteractivityExtension interactivity = ctx.Client.GetInteractivity();
                DiscordEmoji placeholder = DiscordEmoji.FromName(ctx.Client, ":white_large_square:");
                DiscordEmoji circle = DiscordEmoji.FromName(ctx.Client, ":o:");
                DiscordEmoji cross = DiscordEmoji.FromName(ctx.Client, ":x:");
                DiscordMessage tic = await ctx.Channel.SendMessageAsync(
                    placeholder + placeholder + placeholder + "\n" +
                    placeholder + placeholder + placeholder + "\n" +
                    placeholder + placeholder + placeholder
                );
                var arr = new object[tic.Content.Length];
                var i = 0;
                foreach (char chr in tic.Content)
                {
                    arr.SetValue(chr, i);
                    i++;
                }
                await Inputs();
                async Task Inputs()
                {
                    InteractivityResult<DiscordMessage> response = await interactivity.WaitForMessageAsync(
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
                        string newMsg = arr.Aggregate("", (current, s) => current + s);
                        await tic.ModifyAsync(newMsg);
                    }

                    async Task Green()
                    {                  
                        arr[n] = cross;
                        string newMsg = arr.Aggregate("", (current, s) => current + s);
                        await tic.ModifyAsync(newMsg);
                    }

                    async Task MoveTaken()
                    {
                        DiscordMessage moveTakenMsg = await ctx.RespondAsync("This move has already been done! " + response.Result.Author.Mention);
                        await Task.Delay(5000);
                        await response.Result.DeleteAsync("Move Taken | TTT");
                        await moveTakenMsg.DeleteAsync($"Move taken | TicTacToe move by {response.Result.Author.Username}");
                    }
                }
            }
        
        [Command("ping")]
        [Aliases("p")]
        public async Task Ping(CommandContext ctx)
        {
            DiscordMessage msg = await ctx.RespondAsync("Pinging!!!!???"); 
            DiscordEmoji info = DiscordEmoji.FromName(ctx.Client, ":information_source:");
            int wsocket = ctx.Client.Ping;
            TimeSpan messageTime = msg.CreationTimestamp - ctx.Message.CreationTimestamp;
            DiscordEmbed embed = new DiscordEmbedBuilder()
                .WithDescription(info + " WebSocket ping took " + wsocket + " ms" +
                "\n" + info + " Client ping took " + messageTime.Milliseconds + " ms"
            ).WithColor(DiscordColor.Green);

            await msg.ModifyAsync(null, embed);
        }
    }
}