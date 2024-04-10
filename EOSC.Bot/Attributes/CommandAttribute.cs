namespace EOSC.Bot.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute(string commandName) : Attribute
{
    public string CommandName { get; } = commandName;
}