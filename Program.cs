using DSharpPlus;
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

namespace MayoiBot
{
    public class Program
    {
        public static CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; } 
        public static DiscordClient Discord { get; private set; }

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Boot finished");
            Discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config.token(),
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
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
            
            await Discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}