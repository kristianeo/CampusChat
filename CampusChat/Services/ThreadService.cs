using System.Text;

namespace CampusChat.Services;

public class ThreadService
{
    private Queue<string> ChatMessages { get; set; } = new Queue<string>();
    
    public void AddMessage(string s)
    {
        ChatMessages.Enqueue(s);
    }
    
    public string ReadAllMessages()
    {
        StringBuilder sb = new StringBuilder();
        foreach (string s in ChatMessages)
        {
            sb.Append($"\n{s}");
        }

        return sb.ToString();
    }
}