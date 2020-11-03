using DSharpPlus;
using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;

namespace MayoiBot
{
    public class Program
    {
        private static CommandsNextExtension Commands { get; set; }
        public InteractivityExtension Interactivity { get; private set; }
        private static DiscordClient Discord { get; set; }

        public static void Main(string[] args)
        {
            // since we cannot make the entry method asynchronous,
            // let's pass the execution to asynchronous code
            var prog = new Program();
            prog.MainAsync(args).GetAwaiter().GetResult();
        }

        async Task MainAsync(string[] args)
        {
            Discord = new DiscordClient(new DiscordConfiguration
            {
                MinimumLogLevel = LogLevel.Debug,
                Token = config.Token(),
                TokenType = TokenType.Bot,
            });

            Discord.Ready += Client_Ready;
            
            Commands = Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                UseDefaultCommandHandler = true,
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
                ActivityType = ActivityType.Playing,
                StreamUrl = "https://discord.com/invite/UuASJCD",
            };
            await Discord.ConnectAsync(activity, UserStatus.Idle, DateTimeOffset.Now);
            await Task.Delay(-1);
        }

        private Task Client_Ready(DiscordClient client, ReadyEventArgs e)
        {
            // log client is ready
            Console.WriteLine("Mayoi-Onee-san\nClient is ready to process events.", DateTime.Now);
            Console.WriteLine("Does client have intents: " +
                              ((client.Intents & DiscordIntents.All) != DiscordIntents.All));
            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }
    }
}