namespace CampusChat.Services;

public interface INotify
{
    bool IsRead { get; set; }
}

public record Log(string Id, string threadId, DateTime Time) : IEntity;

public class NotificationService : DbService<Log>
{
    protected override string FilePath =>
        "Database/log.json";
    
    public void LastChecked(string userId, string threadId)
    {
        List<Log> logs = GetUserLogs(userId);
        Log? log = logs.FirstOrDefault(l => l.threadId.Contains(threadId));

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
            Log newLog = new Log(userId, threadId, DateTime.Now);
            Add(newLog);
        }
    }

    public List<Log> GetUserLogs(string id)
    {
        return Load().Where(l => l.Id.Contains(id)).ToList();
    }
    
    public Log? GetThreadLog(string userId, string threadId)
    {
        return GetUserLogs(userId).FirstOrDefault(l => l.threadId.Contains(threadId));
    }
}