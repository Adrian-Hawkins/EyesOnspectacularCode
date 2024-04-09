namespace EOSC.Bot.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public CommandAttribute(string commandName)
    {
        CommandName = commandName;
    }

    public string CommandName { get; }
}