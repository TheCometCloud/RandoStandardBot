using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandoStandardBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        // Retrieve client and CommandService instance via ctor
        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;

            IServiceProvider BuildServiceProvider() => new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(GetSets())
                .AddSingleton(new Random())
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();

            _services = BuildServiceProvider();
        }

        public async Task InstallCommandsAsync()
        {

            _commands.AddTypeReader(typeof(bool), new BooleanTypeReader());
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }

        private List<Set> GetSets()
        {
            List<Set> sets = new List<Set>();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"Sets.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] tokens = line.Split('\t');
                Set set = new Set(DateTime.Parse(tokens[0].Trim(' ')), tokens[1].Trim(' '), tokens[3].Trim(' '), Set.SetType.Box, tokens[5].Trim(' '));
                set.Type = (tokens[4].Trim(' ')) switch
                {
                    "Supplemental set" => Set.SetType.Supplemental,
                    "Box set" => Set.SetType.Box,
                    "Expansion set" => Set.SetType.Expansion,
                    "Core set" => Set.SetType.Core,
                    "Compilation set" => Set.SetType.Compilation,
                    "Special Edition" => Set.SetType.Special_Edition,
                    "Starter" => Set.SetType.Starter,
                    _ => Set.SetType.Un,
                };
                sets.Add(set);
            }
            file.Close();

            return sets;
        }
    }
}
