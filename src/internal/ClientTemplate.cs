using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Ezdnet
{
    internal class ClientTemplate
    {   
        #pragma warning disable
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private bool debug;
        private string keyword;
        public async Task<Tuple<DiscordSocketClient, CommandService, IServiceProvider>> RunBotAsync(string token, string keyword, bool debug = false)
        {
            this.debug = debug;
            this.keyword = keyword;
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client).AddSingleton(_commands).BuildServiceProvider();
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
            return new Tuple<DiscordSocketClient, CommandService, IServiceProvider>(this._client, this._commands, this._services);
        }
        public async Task RegisterCommandsAsync()
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
                if (message.HasStringPrefix(keyword, ref argPos))
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess && debug)
                    {
                        Console.WriteLine($"Error: {result.ErrorReason} Message: {message}");
                    }
                }
            }
        }
        #pragma warning restore
        //compiler is picky when it comes to potential null references and ensuring no null references, when we can just ignore them
    }
}