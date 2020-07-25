using DSharpPlus;
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;

namespace MayoiBot
{
    class Program
    {
        public static CommandsNextModule commands;
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
            StringPrefix = config.prefix(),
            CaseSensitive = false,
            EnableDms = false
            });

            commands.RegisterCommands<Commands>();
            commands.RegisterCommands<Mod_Commands>();
            
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static InteractivityModule interactivity; 

    }
}