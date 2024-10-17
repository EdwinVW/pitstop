using Slack.Webhooks;
using Slack.Webhooks.Blocks;
using Slack.Webhooks.Elements;

namespace Pitstop.NotificationService.Message;

public interface ISlackMessageBuilder
{
    ISlackMessageBuilder Add(List<string> fields);
    
    ISlackMessageBuilder AddDivider();
    
    SlackMessage BuildSlackMessage();
}

public class SlackMessageBuilder : ISlackMessageBuilder
{
    private readonly IList<Block> _blocks = new List<Block>();

    public SlackMessageBuilder Add(string text)
    {
        var section = new Section
        {
            Text = new TextObject
            {
                Text = text,
                Type = TextObject.TextType.Markdown

            }
        };
        _blocks.Add(section);
        return this;
    }

    public ISlackMessageBuilder Add(List<string> fields)
    {
        var section = new Section
        {
            Fields = new List<TextObject>()
        };

        foreach (var field in fields)
        {
            section.Fields.Add(new TextObject
            {
                Text = field,
                Type = TextObject.TextType.Markdown

            });
        }

        _blocks.Add(section);
        return this;
    }

    public ISlackMessageBuilder AddDivider()
    {
        _blocks.Add(new Divider());
        return this;
    }

    public SlackMessage BuildSlackMessage()
    {
        return new SlackMessage
        {
            Blocks = _blocks.ToList()
        };
    }
}