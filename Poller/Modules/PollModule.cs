using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Poller.Modules
{
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient m_Client;

        public PollModule(IServiceProvider _services)
        {
            m_Client = _services.GetRequiredService<DiscordSocketClient>();
        }

        [Command("poll")]
        [Summary("Start a poll!")]
        public async Task PollAsync([Remainder] string _poll)
        {
            SocketUser _user = Context.User;
            SocketUserMessage _message = Context.Message;

            EmbedAuthorBuilder _embedAuthor = new EmbedAuthorBuilder
            {
                Name = _message.Author.ToString(),
                IconUrl = _user.GetAvatarUrl()
            };

            EmbedBuilder _embed = new EmbedBuilder
            {
                Author = _embedAuthor,
                Color = new Color(120, 125, 255),
                Description = _poll,
                Title = "A new poll appeard!"
            };

            await _message.DeleteAsync();
            IUserMessage _reponse = await ReplyAsync(null, false, _embed.Build());
            await _reponse.AddReactionsAsync(new Emoji[]
            {
                new Emoji("✅"),
                new Emoji("❌")
            });
        }
    }
}
