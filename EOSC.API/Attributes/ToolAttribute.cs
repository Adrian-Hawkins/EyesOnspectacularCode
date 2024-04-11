namespace EOSC.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ToolAttribute(string toolName) : Attribute
    {
        public string ToolName { get; private set; } = toolName;
    }
}
