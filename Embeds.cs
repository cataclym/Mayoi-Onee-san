using System.Drawing;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace MayoiBot
{
    public static class Embeds
    {
        public static DiscordEmbed TttEmbed = new DiscordEmbedBuilder
            {
                Title = "Starting a new game...",
                Description = "`member.mention` + react with a 👍 to start game! Or 👎 to cancel.",
                Color = new DiscordColor(44, 47, 51),
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "45 seconds to go", IconUrl = "https://cdn.discordapp.com/attachments/717045059215687691/735462333786095626/C_Sharp_logo.png"} 
            };
        public static DiscordEmbed TimeOut = new DiscordEmbedBuilder
        {
            Title = "Timed out",
            Color = DiscordColor.Red
        };
        public static DiscordEmbed Cancelled = new DiscordEmbedBuilder
        {
            Title = "Game was cancelled",
            Color = DiscordColor.Red
        };
    }    
}