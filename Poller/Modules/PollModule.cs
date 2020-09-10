using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Poller.Modules
{
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll")]
        [Summary("Start a poll!")]
        public async Task PollAsync([Remainder] string _poll)
        {
            SocketUser _user = Context.User;
            SocketUserMessage _message = Context.Message;

            EmbedFooterBuilder _footer = new EmbedFooterBuilder()
            {
                Text = $"Poll creatd by {_message.Author}",
                IconUrl = _user.GetAvatarUrl()
            };

            EmbedBuilder _embed = new EmbedBuilder
            {
                Color = new Color(120, 125, 255),
                Description = $"*{_poll}*",
                Title = "A new poll appeard!",
                Footer = _footer
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
