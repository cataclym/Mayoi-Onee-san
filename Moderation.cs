using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Net.Serialization;

namespace MayoiBot 
    {
        public class Moderation : BaseCommandModule
        {
            [Command("bean")]
            [Aliases("ban")]
            [RequireBotPermissions(Permissions.BanMembers)]
            [RequireUserPermissions(Permissions.BanMembers)]
            public async Task Bean (CommandContext ctx, DiscordMember member, [RemainingText]string reason = null)
            {
                var authorRole = ctx.Member.Roles.FirstOrDefault();

                if (member.Id == ctx.Guild.Owner.Id && (member.Roles.FirstOrDefault()?.Position.CompareTo(authorRole?.Position) < 0 )) {
                    await ctx.Channel.SendMessageAsync(embed: new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.Red)
                        .WithDescription("You don't have permissions to ban this member.")
                    );
                }
                await member.BanAsync(0, reason);
                await ctx.Channel.SendMessageAsync(member.Username + " was beaned!");

            }
            [Command("kick")]
            [Aliases("k")]
            [RequireBotPermissions(Permissions.KickMembers)]
            [RequireUserPermissions(Permissions.KickMembers)]
            public async Task Kick (CommandContext ctx, DiscordMember member, [RemainingText]string reason = null)
            {
                var authorRole = ctx.Member.Roles.FirstOrDefault();

                if (member.Id == ctx.Guild.Owner.Id && (member.Roles.FirstOrDefault()?.Position.CompareTo(authorRole?.Position) < 0 )) {
                    await ctx.Channel.SendMessageAsync(embed: new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.Red)
                        .WithDescription("You don't have permissions to kick this member.")
                    );
                }
                await member.RemoveAsync(reason);
                await ctx.Channel.SendMessageAsync(member.Username + " was kicked!");
            }

            [Command("destroy")]
            [RequireBotPermissions(Permissions.ManageRoles)]
            [RequireUserPermissions(Permissions.ManageRoles)]
            public async Task Destroy(CommandContext ctx, DiscordRole dRole, [RemainingText]string reason = null)
            {
                await dRole.DeleteAsync(reason);
                await ctx.Channel.SendMessageAsync($"{dRole.Name} just got obliterated!");
            }

            [Command("destroy")]
            [RequireBotPermissions(Permissions.ManageChannels)]
            [RequireUserPermissions(Permissions.ManageChannels)]
            public async Task Destroy(CommandContext ctx, DiscordChannel dChannel, [RemainingText]string reason = null)
            {
                await dChannel.DeleteAsync(reason);
                await ctx.Channel.SendMessageAsync($"{dChannel.Name} just got obliterated!");
            }
        }
    }
