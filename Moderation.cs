using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace MayoiBot 
    {
        [Category("Moderation")]
        [DSharpPlus.CommandsNext.Attributes.Description("Commands related to modding and administrating a server")]
        public class Moderation : BaseCommandModule
        {
            [Command("bean")]
            [Aliases("ban")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Bean (CommandContext ctx, DiscordMember member, [RemainingText]string reason = null)
            {
                if (member.Id == ctx.Guild.Owner.Id) return;
                const int arg = 0;
                await member.BanAsync(arg, reason);
                await ctx.Channel.SendMessageAsync(member.Username + " was beaned!");

            }
            [Command("kick")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Kick (CommandContext ctx, DiscordMember member, [RemainingText]string reason = null)
            {
                if (member.Id == ctx.Guild.Owner.Id) return;
                await member.RemoveAsync(reason);
                await ctx.Channel.SendMessageAsync(member.Username + " was kicked!");
            }

            [Command("destroy")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Destroy(CommandContext ctx, DiscordRole dRole, [RemainingText]string reason = null)
            {
                await dRole.DeleteAsync(reason);
                await ctx.Channel.SendMessageAsync($"{dRole.Name} just got obliterated!");
            }

            [Command("destroy")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Destroy(CommandContext ctx, DiscordChannel dChannel, [RemainingText]string reason = null)
            {
                await dChannel.DeleteAsync(reason);
                await ctx.Channel.SendMessageAsync($"{dChannel.Name} just got obliterated!");
            }
        }
    }
