using System;
using System.Threading.Tasks;
using Discord;
using System.Collections.Generic;
using System.IO;
using Discord.WebSocket;

namespace RandoStandardBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            CommandHandler handler = new CommandHandler(_client, new Discord.Commands.CommandService());
            await handler.InstallCommandsAsync();

            var token = File.ReadAllText(@"..\..\..\token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
