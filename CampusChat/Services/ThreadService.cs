using System.Text;
using System.Text.Json;
namespace CampusChat.Services;

//TODO: Need to add posts to lists when creating 
public record ChatThread(
    string Id,
    string Title,
    string AccessCode,
    List<Post> Posts,
    List<string> JoinedUsers,
    bool IsPrivate = false);

public record Post(
    string Id,
    string Author,
    string Message,
    DateTime TimeStamp,
    string ChatTreadId,
    string? ParentPostId = null);

public class ThreadService
{
    private Queue<Post> ChatMessages { get; set; } = new();
    public event Action? NewMessages;
    public event Action? ThreadsChanged;
    
    public void AddMessage(Post post)
    {
        ChatMessages.Enqueue(post);
        NewMessages?.Invoke();
    }
    
    //TODO: Format this differently for the chat?
    public string ReadAllMessages(string threadId)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Post p in ChatMessages)
        {
            if (p.ChatTreadId == threadId)
            {
                sb.Append($"\n{p.Author}: {p.Message} -{p.TimeStamp:g}");
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

    

    private void UpdateThread(List<ChatThread> threads)
    {
        string path = "Database/threads.json";
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(threads, options);
        File.WriteAllText(path, updatedJson);
        ThreadsChanged?.Invoke();
    }

    public void JoinThread(Profile user, ChatThread thread)
    {
        List<ChatThread> threads = GetThreads();
        
        var oldThread = threads.FirstOrDefault(t => t.Id == thread.Id);
        threads.Remove(oldThread);
        
        thread.JoinedUsers.Add(user.UserName);
        threads.Add(thread);
        
        UpdateThread(threads);
    }

    public void LeaveThread(Profile user, ChatThread thread)
    {
        List<ChatThread> threads = GetThreads();
        var oldThread = threads.FirstOrDefault(t => t.Id == thread.Id);
        threads.Remove(oldThread);
        
        thread.JoinedUsers.Remove(user.UserName);
        threads.Add(thread);
        
        UpdateThread(threads);
    }

    public Post AddPost(Profile author, ChatThread thread, string message)
    {
        Console.WriteLine("test");
        return new Post(Guid.NewGuid().ToString(), author.UserName,
            message, DateTime.Now, thread.Id);
    }

    private void DeletePost()
    {
        
    }
    
    private event Action? Changed;

}