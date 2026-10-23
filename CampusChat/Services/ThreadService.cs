using System.Text.Json;
namespace CampusChat.Services;

public record ChatThread(
    string Id,
    string Title,
    string AccessCode,
    List<Post> Posts,
    List<string> JoinedUsers,
    bool IsPrivate = false) : IEntity;

public record Post(
    string Id,
    string Author,
    string Message,
    DateTime TimeStamp,
    string ChatTreadId,
    string? ParentPostId = "");

public class ThreadService : DbService<ChatThread>
{
    protected override string FilePath =>
        "Database/threads.json";

    public event Action? NewMessages;
    public event Action? ThreadsChanged;

    public List<ChatThread> GetThreads()
    {
        return Load();
    }

    public ChatThread? GetThread(string threadId)
    {
        return Get(threadId);
    }

    public List<Post> ReadAllMessages(string threadId)
    {
        return Get(threadId)?.Posts ?? [];
    }

    public void SendMessage(
        Profile author,
        string threadId,
        string message,
        string? parentPostId)
    {
        ChatThread? thread = Get(threadId);

        if (thread is null)
            return;

        Post post = new(
            Guid.NewGuid().ToString(),
            author.UserName,
            message,
            DateTime.Now,
            threadId,
            parentPostId);

        thread.Posts.Add(post);

        Add(thread);

        NewMessages?.Invoke();
    }

    public void DeleteMessage(Post post)
    {
        ChatThread? thread = Get(post.ChatTreadId);

        if (thread is null)
            return;

        Post deletedPost = post with
        {
            Message = "This message has been deleted"
        };

        int index = thread.Posts.FindIndex(p => p.Id == post.Id);

        if (index >= 0)
        {
            thread.Posts[index] = deletedPost;

            Add(thread);

            NewMessages?.Invoke();
        }
    }

    public void JoinThread(Profile user, ChatThread thread)
    {
        if (!thread.JoinedUsers.Contains(user.UserName))
        {
            thread.JoinedUsers.Add(user.UserName);

            Add(thread);

            ThreadsChanged?.Invoke();
        }
    }

    public void LeaveThread(Profile user, ChatThread thread)
    {
        thread.JoinedUsers.Remove(user.UserName);

        Add(thread);

        ThreadsChanged?.Invoke();
    }
}