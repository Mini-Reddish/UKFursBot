using System.Text;

namespace UKFursBot;

public class RichTextBuilder
{
    private readonly StringBuilder _sb = new();

    public RichTextBuilder AddHeading1(string text)
    {
        _sb.AppendLine("# " + text);
        return this;
    }
    public RichTextBuilder AddHeading2(string text)
    {
        _sb.AppendLine("## " + text);
        return this;
    }
    public RichTextBuilder AddHeading3(string text)
    {
        _sb.AppendLine("### " + text);
        return this;
    }

    public string Build()
    {
        return _sb.ToString();
    }

    public RichTextBuilder AddText(string message)
    {
        _sb.AppendLine(message);
        return this;
    }

    public RichTextBuilder AddBulletedListItems<T, TItemType>(T items, Func<TItemType, string> textToDisplayPerItem) where T : IEnumerable<TItemType> 
    {
        foreach (var item in items)
        {
            _sb.AppendLine($"- {textToDisplayPerItem(item)}");
        }
        return this;
    }
}