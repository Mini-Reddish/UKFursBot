using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using UKFursBot.Context;

namespace UKFursBot.Features.SuspiciousTextActivityMonitor;

public class RemoveOldSuspiciousEdits : IMessageEditedHandler
{
    private readonly SocketMessageChannelManager _messageChannelManager;
    private readonly UKFursBotDbContext _dbContext;

    private readonly Regex _urlRegex = new Regex("(ht|f)tp(s?)");

    public RemoveOldSuspiciousEdits(SocketMessageChannelManager messageChannelManager, UKFursBotDbContext dbContext)
    {
        _messageChannelManager = messageChannelManager;
        _dbContext = dbContext;
    }
    
    public async Task HandleMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
    {
        var botConfig = _dbContext.BotConfigurations.First();
        if (botConfig.MinutesThresholdForMessagesBeforeEditsAreSuspicious <= 0)
            return;
        if (_urlRegex.IsMatch(after.Content) == false)
            return;
        var originalMessage = await before.GetOrDownloadAsync();
        if (originalMessage.CreatedAt + TimeSpan.FromMinutes(botConfig.MinutesThresholdForMessagesBeforeEditsAreSuspicious) > DateTimeOffset.Now)
            return;

        await after.DeleteAsync();
        await _messageChannelManager.SendLoggingWarningMessageAsync($"Deleted a suspicious edit made by the user <@{after.Author.Id}>.  Trying to add a link to an old message.");
    }
}