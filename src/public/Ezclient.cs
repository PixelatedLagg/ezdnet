using Discord.Commands;
using Discord.WebSocket;

namespace Ezdnet
{
    public static class Ezclient
    {
        public static Tuple<DiscordSocketClient, CommandService, IServiceProvider> StartBot(string token, string keyword, bool debug = false)
        {
            Tuple<DiscordSocketClient, CommandService, IServiceProvider> client;
            client = new ClientTemplate().RunBotAsync(token, keyword, debug).Result;
            return client;
        }
    }
}