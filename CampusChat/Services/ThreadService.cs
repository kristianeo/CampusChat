using System.Text;
using System.Text.Json;
using CampusChat.Database;

namespace CampusChat.Services;

public record ChatThread(
    string Id,
    string Title,
    string AccessCode,
    List<Post> Posts,
    List<string> JoinedUsers,
    string CreatedBy,
    bool IsPrivate = false);

public record Post(
    string Id,
    string Author,
    string Message,
    TimeOnly TimeStamp,
    string ParentPostId,
    string ChatTreadId);

public class ThreadService
{
    private Queue<Post> ChatMessages { get; set; } = new();

    public void CreateThread(ChatThread thread)
    {
        string path = "Database/users.json";

        List<ChatThread> threads = GetThreads();

        threads.Add(thread);
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(threads, options);
        File.WriteAllText(path, updatedJson);
    }
    
    public void AddMessage(Post post)
    {
        ChatMessages.Enqueue(post);
    }
    
    public string ReadAllMessages(string ChatThreadId)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Post p in ChatMessages)
        {
            if (p.ChatTreadId == ChatThreadId)
            {
                sb.Append($"\n{p.Message}");
            }
        }

        return sb.ToString();
    }

    public List<ChatThread> GetThreads()
    {
        string path = "Database/threads.json";
        
        List<ChatThread> threads = new();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            threads = JsonSerializer.Deserialize<List<ChatThread>>(json) ?? new();
        }

        return threads;
    }

    public ChatThread? GetThread(string threadId)
    {
        var threads = GetThreads();
        return threads?.FirstOrDefault(p => p.Id == threadId);
    }

    public void JoinThread()
    {
        
    }

    private void LeaveThread()
    {
        
    }

    private void AddPost()
    {
        
    }

    private void DeletePost()
    {
        
    }
    
    private event Action? Changed;
    
}