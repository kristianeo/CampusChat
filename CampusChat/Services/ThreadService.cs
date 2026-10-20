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
    string? ParentPostId = "");

public class ThreadService
{
    private List<Post> ChatMessages { get; set; } = new();
    public event Action? NewMessages;
    public event Action? ThreadsChanged;
    
    public void AddMessage(Post post)
    {
        ChatMessages.Add(post);
        NewMessages?.Invoke();
    }

    public void DeleteMessage(Post post)
    {
        ChatMessages.Remove(post);
        NewMessages?.Invoke();
    }
    
    public List<Post> ReadAllMessages(string threadId)
    {
        List<Post> content = [];
        content.AddRange(ChatMessages.Where(p => p.ChatTreadId == threadId));

        return content;
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

    public Post AddPost(Profile author, ChatThread thread, string message, string parentPostId)
    {
        return new Post(Guid.NewGuid().ToString(), author.UserName,
            message, DateTime.Now, thread.Id, parentPostId);
    }

    public Post? GetPost(string postId)
    {
        return ChatMessages.FirstOrDefault(p => p.Id == postId);
    }

}