using System;
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
        [Group("Mod-Commands")]
        [Description("Commands related to modding and administrating a server")]
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
                        "\nPresence: " + member.Presence?.Status + " | " + member.Presence?.Game?.Name + " `" + member.Presence?.Game?.Details + " " +
                        member.Presence?.Game?.Instance + "`"
                    };
                    await ctx.Channel.SendMessageAsync(embed: emb);
                }
                catch (Exception exc) 
                {
                    Console.WriteLine(exc);
                }
            }
            [Command("bean")]
            [Aliases("ban")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Bean (CommandContext ctx, DiscordMember member, [RemainingText]string reason = null)
            {
                try
                {
                    int arg = 0;
                    await member.BanAsync(arg, reason);
                    await ctx.Channel.SendMessageAsync(member.Username + " was beaned!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            [Command("kick")]
            public async Task Kick (CommandContext ctx, DiscordMember member, [RemainingText]string reason = null)
            {
                await member.RemoveAsync(reason);
                await ctx.Channel.SendMessageAsync(member.Username + " was kicked!");
            }
        }
    }
