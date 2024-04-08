namespace EOSC.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ToolAttribute : Attribute
    {
        public string ToolName { get; private set; }
        public ToolAttribute(string toolName)
        {
            ToolName = toolName;
        }
    }
}
