using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Poller.Service
{
    public class CommandHandlingService
    {
        private readonly CommandService m_Commands;
        private readonly DiscordSocketClient m_Client;
        private readonly IServiceProvider m_Service;

        public CommandHandlingService(IServiceProvider _service)
        {
            m_Commands = _service.GetRequiredService<CommandService>();
            m_Client = _service.GetRequiredService<DiscordSocketClient>();
            m_Service = _service;

            // Events
            m_Commands.CommandExecuted += CommandExecutedAsync;
            m_Client.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await m_Commands.AddModulesAsync(Assembly.GetEntryAssembly(), m_Service);
        }

        private async Task MessageReceivedAsync(SocketMessage _rawMessage)
        {
            // Make sure the bot only respond to users
            if (!(_rawMessage is SocketUserMessage _message)) return;
            if (_message.Source != MessageSource.User) return;

            // Make sure that the bot will only react to its given prefix
            int _argsPos = 0;
            if (!_message.HasCharPrefix('!', ref _argsPos)) return;

            var _context = new SocketCommandContext(m_Client, _message);

            // Execute the command
            await m_Commands.ExecuteAsync(_context, _argsPos, m_Service);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> _command, ICommandContext _context, IResult _result)
        {
            if (!_command.IsSpecified || _result.IsSuccess) return;

            // Delete the send command
            SocketMessage _message = (SocketMessage)_context.Message;
            await _message.DeleteAsync();

            // If there is an error, let the bot owner know
            SocketUser _userID = m_Client.GetUser(368317619838779393); // Bot owner user ID
            await UserExtensions.SendMessageAsync(_userID, $"error: {_result.ErrorReason}");
        }
    }
}
