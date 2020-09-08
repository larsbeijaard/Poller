using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Poller.Service;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Poller
{
    public class Program
    {
        public static void Main(string[] _args) =>
            new Program().StartBot().GetAwaiter().GetResult();

        private async Task StartBot()
        {
            using (var _services = ConfigureServices())
            {
                var _client = _services.GetRequiredService<DiscordSocketClient>();

                _client.Log += LogAsync;
                _services.GetRequiredService<CommandService>();

                // Login with the bot
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
                await _client.StartAsync();

                await _services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                // Blocks the program from closing
                await Task.Delay(Timeout.Infinite);
            }
        }

        private Task LogAsync(LogMessage _arg)
        {
            Console.WriteLine(_arg.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
