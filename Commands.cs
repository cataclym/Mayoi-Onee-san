using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;

namespace MayoiBot
{
    [Category("Commands")]
    [DSharpPlus.CommandsNext.Attributes.Description("Commands for everyone")]
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
                    Description = "This bot is open source and available [here](https://github.com/cataclym/Mayoi-Onee-san.git 'GitHub')!"
                };
                
                builder.AddField("Language", "C#", true);
                builder.AddField(".NET wrapper", "[D#+](https://github.com/DSharpPlus/DSharpPlus/ 'DSharpPlus')", true);
                builder.AddField("Author", "Cata", true);
                builder.WithImageUrl("https://dsharpplus.emzi0767.com/logo.png");
                builder.WithThumbnail("https://cdn.discordapp.com/attachments/717045059215687691/735462333786095626/C_Sharp_logo.png");
                
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
            public async Task Smart(CommandContext ctx, DiscordMember member) // Mention isnt any string. Not just mentions. Help.
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
                        yn = 0; }
                    await ctx.Channel.SendMessageAsync(member.Mention + EitherNr(yn));          
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }     
            }
        [Command("8ball")]
            public async Task EightBall (CommandContext ctx, [RemainingText]string args)
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
        public async Task User (CommandContext ctx, DiscordUser member = null)
        {   
            if (member is null) { member = ctx.User; }
            try 
            {
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
                    "\nPresence: " + member.Presence?.Status + " | `" + member.Presence?.Activity?.Name + "` `" + member.Presence?.Activity?.CustomStatus +
                    " `\nAdditional: (?) `" + member.Presence?.Activity?.RichPresence?.Application?.Name + " `:` " + member.Presence?.Activity?.RichPresence?.Details +
                    " `\nStreaming: (?) " + member.Presence?.Activity?.StreamUrl
                };
                await ctx.Channel.SendMessageAsync(embed: emb);
            }
            catch (Exception exc) 
            {
                Console.WriteLine(exc);
            }
        }
        [Command("tictactoe")]
        [Aliases("ttt")]
        public async Task TicTacToe(CommandContext ctx, DiscordMember member)
        {
            try
            {
                var interactivity = ctx.Client.GetInteractivity();
                var x = await ctx.RespondAsync(embed: MayoiBot.Embeds.TttEmbed);
                await x.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":eyes:"));
                var y = await interactivity.WaitForReactionAsync(x, ctx.User, TimeSpan.FromSeconds(25));
                await ctx.RespondAsync(y.Result?.Emoji);
            }
            catch (Exception e)
            {
                await ctx.RespondAsync(e.Message + "\n" + e.StackTrace);
                Console.WriteLine(e);
                
            }
        }
    }
}