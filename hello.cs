using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharp​Plus.Entities;
using DSharp​Plus.Interactivity;

namespace MayoiBot
{
    public class Commands
    {
        static string GetMethodsUsingReflection()
            {
                MethodInfo[] methodInfos = typeof(Commands).GetMethods();
                    string strarr = "";
                    // writes all the property names                    
                    foreach (var mi in Program.commands.RegisteredCommands)
                    {
                        strarr += config.prefix() + mi.Value + "\n";
                    }
                    return strarr;
            }
        [Command("hi")]
        [Description("Gives a hearty hello!")]
            public async Task Hi(CommandContext ctx)
            {
                await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!\n{ctx.User.AvatarUrl}");
            }   
        [Command("info")]
            public async Task Info(CommandContext ctx)
            {
                string infostring = ctx.Client.CurrentUser.Username + " bot info:";

                var builder = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.White,
                    Title = infostring,
                    Description = "This bot is open source and available [here](https://github.com/cataclym/Mayoi-Onee-san.git 'GitHub')!"
                };
                
                builder.AddField("Language", "C#", true);
                builder.AddField(".NET wrapper", "[D#+](https://github.com/DSharpPlus/DSharpPlus/ 'DSharpPlus')", true);
                builder.AddField("Author", "Cata", true);
                builder.WithImageUrl("https://dsharpplus.emzi0767.com/logo.png");
                builder.WithThumbnailUrl("https://cdn.discordapp.com/attachments/717045059215687691/735462333786095626/C_Sharp_logo.png");
                
                await ctx.RespondAsync(embed: builder);
            }
        [Command("random")]
            public async Task Random(CommandContext ctx, int min, int max)
            {
                var number = new Random();
                await ctx.Channel.SendMessageAsync("Returned: " + number.Next(min, max));
            }
        [Command("cmds")]
        [Aliases("commands", "cmd")]
        [Description("Returns all the available commands")]
            public async Task Cmds(CommandContext ctx)
            {
                await ctx.Channel.SendMessageAsync(GetMethodsUsingReflection());
            }
        static string eitherNR(int x)
                {
                    string[] arr = {" is not very smart!", " is very smart!"};
                    return arr[x];
                }
        [Command("smart")]
            public async Task Smart(CommandContext ctx, string Mention) // Mention isnt any string. Not just mentions. Help.
            {
                var rndm = new Random();
                int YN = rndm.Next(0, 101);
                if (YN >= 50) { YN = 1;}
                else { YN = 0; }  
                await ctx.Channel.SendMessageAsync(Mention + eitherNR(YN));               
            }
        [Command("8ball")]
            public async Task EightBall (CommandContext ctx, string args)
            {
                string[] EBArray = 
                {
                    "It is certainly possible!", "Yes, I believe so!", "Maybe...",
                    "I hope not!", "Yes... Maybe?", "Do you actually think that is possible??",
                    "I pity you for even asking."
                };
                var RNDM = new Random();
                int nmbr = RNDM.Next(0, EBArray.Length);
                await ctx.Channel.SendMessageAsync(EBArray[nmbr]);
            }
        [Command("user")]
        public async Task User (CommandContext ctx, DiscordUser member = null)
        {   
            if (member is null) {member = ctx.User;}
            try 
            {
                var emb = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Gold,
                    Title = "User info of: " + member.Username,
                    ThumbnailUrl = member.AvatarUrl,
                    Description = 
                    member.Mention +
                    "\nUser Name: " + member.Username + " #" + member.Discriminator +
                    "\nUser Created: " + member.CreationTimestamp +
                    "\nID: " + member.Id +
                    "\nVerification: " + member.Verified +
                    "\nPresence: " + member.Presence?.Status + " | " + member.Presence?.Game?.Name + " `" + member.Presence?.Game?.Details + "`"
                };
                await ctx.Channel.SendMessageAsync(embed: emb);
            }
            catch (Exception exc) 
            {
                Console.WriteLine(exc);
            }
        }
    }
    public class Mod_Commands
    {
        [Command("userinfo")]
        [Aliases("uinfo")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task UserInfo (CommandContext ctx, DiscordMember member = null)
        {   
            if (member is null) {member = ctx.Member;}
            try 
            {
                var emb = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Gold,
                    Title = "User info of: " + member.Username,
                    ThumbnailUrl = member.AvatarUrl,
                    Description = 
                    "Displayname: " + member.DisplayName +
                    "\nGuildOwner: " + member.Guild?.IsOwner +
                    "\nJoin Date: " + member.JoinedAt +
                    "\nUser Created: " + member.CreationTimestamp +
                    "\nID: " + member.Id +
                    "\nVerification: " + member.Verified +
                    "\nPresence: " + member.Presence?.Status + " | " + member.Presence?.Game?.Name + " `" + member.Presence?.Game?.Details + "`"
                };
                await ctx.Channel.SendMessageAsync(embed: emb);
            }
            catch (Exception exc) 
            {
                Console.WriteLine(exc);
            }
        }
    }
}