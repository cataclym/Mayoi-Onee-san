using DSharpPlus;
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

namespace MayoiBot
{
    class Program
    {
        public static CommandsNextExtension commands;
        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Boot finished");
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config.token(),
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
            StringPrefixes = new[]{"-"},
            CaseSensitive = false,
            EnableDms = false
            });

            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<Moderation>();

            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static InteractivityExtension interactivity; 

    }
}