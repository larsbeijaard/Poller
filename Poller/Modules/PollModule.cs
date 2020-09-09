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

            m_Client.ReactionAdded += OnReactionAddedAsync;
        }

        private async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> _user, ISocketMessageChannel _message, SocketReaction _reaction)
        {
            await ReplyAsync("(test) Dont you dare to add reactions!");
            // Todo: make it so when there has been a reaction added to this bots its message, remove the reaction
        }

        [Command("poll")]
        [Summary("Start a poll!")]
        public async Task PollAsync([Remainder]string _poll)
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
