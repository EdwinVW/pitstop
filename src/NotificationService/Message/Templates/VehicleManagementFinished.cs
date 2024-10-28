using Slack.Webhooks;

namespace Pitstop.NotificationService.Message.Templates;

public class VehicleManagementFinished
{
    private ISlackMessageBuilder _slackMessageBuilder;

    public VehicleManagementFinished(string header, string section, List<string> fields)
    {
        _slackMessageBuilder = new SlackMessageBuilder();
        _slackMessageBuilder.AddHeader(header);
        _slackMessageBuilder.AddDivider();
        _slackMessageBuilder.AddSection(section);
        _slackMessageBuilder.AddDivider();
        _slackMessageBuilder.AddField(fields);
    }

    public SlackMessage BuildMessage()
    {
        return _slackMessageBuilder.BuildSlackMessage();
    }
    
}