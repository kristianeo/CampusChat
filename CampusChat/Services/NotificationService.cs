namespace CampusChat.Services;

public interface INotify
{
    bool IsRead { get; set; }
}

public record Log(string Id, string UserId, string ThreadId, DateTime Time) : IEntity;

public class NotificationService : DbService<Log>
{
    protected override string FilePath =>
        "Database/log.json";
    
    public void LastChecked(string userId, string threadId)
    {
        Log? log = GetThreadLog(userId, threadId);

        if (log != null)
        {
            Log newLog = log with
            {
                Time = DateTime.Now
            };
            Add(newLog);
        }
        else
        {
            Log newLog = new Log(Guid.NewGuid().ToString(), userId, threadId, DateTime.Now);
            Add(newLog);
        }
    }

    public List<Log> GetUserLogs(string id)
    {
        return Load().Where(l => l.Id.Contains(id)).ToList();
    }
    
    public Log? GetThreadLog(string userId, string threadId)
    {
        return GetUserLogs(userId).FirstOrDefault(l => l.ThreadId.Contains(threadId));
    }
}