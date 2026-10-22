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
    
    public void SendMessage(Profile author, string threadId, string message, string? parentPostId)
    {
        Post post = new Post(Guid.NewGuid().ToString(), author.UserName,
                message, DateTime.Now, threadId, parentPostId);
        var thread = GetThread(threadId);
        thread.Posts.Add(post);
        
        UpdateThreads(thread);
        
        ChatMessages.Add(post);
        
        NewMessages?.Invoke();
    }

    public void DeleteMessage(Post post)
    {
        Post updatedPost = post with
        {
            Message = "This message has been deleted"
        };

        var thread = GetThread(post.ChatTreadId);
        
        thread!.Posts.Remove(post);
        thread.Posts.Add(updatedPost);
            
        UpdateThreads(thread);    
        
        NewMessages?.Invoke();
    }
    
    public List<Post> ReadAllMessages(string threadId)
    {
        List<ChatThread> messages = GetThreads();
        return messages.FirstOrDefault(m => m.Id == threadId).Posts;
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
        return GetThreads().FirstOrDefault(t => t.Id == threadId);
    }

    

    private void UpdateThreads(ChatThread thread)
    {
        List<ChatThread> threads = GetThreads();
        
        var oldThread = threads.FirstOrDefault(t => t.Id == thread.Id);
        threads.Remove(oldThread);
        threads.Add(thread);
        
        string path = "Database/threads.json";
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(threads, options);
        File.WriteAllText(path, updatedJson);
    }

    public void JoinThread(Profile user, ChatThread thread)
    {
        thread.JoinedUsers.Add(user.UserName);
        
        UpdateThreads(thread);
        
        ThreadsChanged?.Invoke();
    }

    public void LeaveThread(Profile user, ChatThread thread)
    {
        thread.JoinedUsers.Remove(user.UserName);
        
        UpdateThreads(thread);
        
        ThreadsChanged?.Invoke();
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