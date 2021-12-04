using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Ezdnet
{
    internal class ClientTemplate
    {   
        #pragma warning disable CS8602, CS8618
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private bool _debug;
        private string _prefix;
        public async Task<(DiscordSocketClient client, CommandService command, IServiceProvider service)> RunBotAsync(string token, string prefix, bool debug = false)
        {
            this._debug = debug;
            this._prefix = prefix;
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
            return (this._client, this._commands, this._services);
        }
        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (!message.Author.IsBot)
            {
                int argPos = 0;
                if (message.HasStringPrefix(_prefix, ref argPos))
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess && _debug)
                    {
                        Console.WriteLine($"Error: {result.ErrorReason} Message: {message}");
                    }
                }
            }
        }
        #pragma warning restore CS8602, CS8618
        //compiler throws weird null warnings which mean almost nothing
    }
}