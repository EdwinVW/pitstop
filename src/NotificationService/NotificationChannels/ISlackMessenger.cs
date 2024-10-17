using Pitstop.NotificationService.Message;
using Slack.Webhooks;

namespace Pitstop.NotificationService.NotificationChannels;

public interface ISlackMessenger
{
    Task PostMessage(SlackMessage slackMessage);
}

public class SlackMessenger : ISlackMessenger
{
    private readonly SlackClient _slackClient;

    public SlackMessenger(string slackConfig)
    {
        _slackClient = new SlackClient(slackConfig);
    }

    public async Task PostMessage(SlackMessage slackMessage)
    {
         await _slackClient.PostAsync(slackMessage);
    }
}