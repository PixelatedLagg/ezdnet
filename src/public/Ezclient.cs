using Discord.Commands;
using Discord.WebSocket;

namespace Ezdnet
{
    public static class Ezclient
    {
        public static (DiscordSocketClient client, CommandService command, IServiceProvider service) StartBot(string token, string keyword, bool debug = false)
        {
            (DiscordSocketClient, CommandService, IServiceProvider) client;
            client = new ClientTemplate().RunBotAsync(token, keyword, debug).Result;
            return client;
        }
    }
}