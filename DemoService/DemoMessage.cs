namespace DemoService
{
    public interface IDemoMessage
    {
        public string Text { get; }
    }
    
    public class DemoMessage : IDemoMessage
    {
        public string Text { get; }

        public DemoMessage(string text)
        {
            Text = text;
        }
    }
}