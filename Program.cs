using DSharpPlus;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace MayoiBot
{
    public class Program
    {
        private static CommandsNextExtension Commands { get; set; }
        public InteractivityExtension Interactivity { get; private set; }
        private static DiscordClient Discord { get; set; }

        private static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Boot finished");
            Discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config.Token(),
                TokenType = TokenType.Bot,
            });
            Commands = Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[]{"-"},
                CaseSensitive = false,
                EnableDms = false
            });

            Commands.RegisterCommands<Commands>();
            Commands.RegisterCommands<Moderation>();

            Discord.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });
            
            var activity = new DiscordActivity
            {
                Name = "Ara Ara~",
                ActivityType = ActivityType.Streaming,
                StreamUrl = "https://discord.com/invite/UuASJCD",
            };
            await Discord.ConnectAsync(activity, UserStatus.Idle, DateTimeOffset.Now);
            await Task.Delay(-1);
        }
    }
}