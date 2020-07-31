using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace MayoiBot
{
    internal static class Embeds
    {
        public static DiscordEmbedBuilder TttEmbed = new DiscordEmbedBuilder
            {
                Title = "Starting a new game...",
                Color = new DiscordColor(44, 47, 51),
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "45 seconds to go", IconUrl = "https://cdn.discordapp.com/attachments/717045059215687691/735462333786095626/C_Sharp_logo.png"} 
            };
        public static readonly DiscordEmbed TimeOut = new DiscordEmbedBuilder
        {
            Title = "Timed out",
            Color = DiscordColor.Red
        };
        public static readonly DiscordEmbed Cancelled = new DiscordEmbedBuilder
        {
            Title = "Game was cancelled",
            Color = DiscordColor.Red
        };
    }    
}