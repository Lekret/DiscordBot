using System;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Source.Services;
using DiscordBot.Source.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Source
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var token = args[0];
            return new Program(token).Start();
        }
        
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly string _token;

        private Program(string token)
        {
            _token = token;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                GatewayIntents = GatewayIntents.All
            });
            
            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            _client.Log += Logging.Log;
            _commands.Log += Logging.Log;
            var services = ConfigureServices();
            _commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            var commandHandler = new CommandHandler(_client, _commands, services);
            _client.MessageReceived += commandHandler.HandleCommandAsync;
        }

        private IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands);
            return map.BuildServiceProvider();
        }

        private async Task Start()
        {
            Console.WriteLine($"Start with token: {_token}");
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.DownloadUsersAsync(_client.Guilds);
            await Task.Delay(Timeout.Infinite);
        }
    }
}