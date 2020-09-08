using Discord.Commands;
using System.Threading.Tasks;

namespace Poller.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        // Sample command
        [Command("ping")]
        public Task PingAsync()
            => ReplyAsync("pong!");
    }
}
