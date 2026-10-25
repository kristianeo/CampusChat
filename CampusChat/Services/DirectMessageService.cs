using System.Collections.Immutable;
using System.Text.Json;

namespace CampusChat.Services;

public record DirectMessage(
    string Id,
    ImmutableList<string> Participants,
    List<Message> Messages) : IEntity;


public record Message(
    string Id,
    string Author,
    DateTime TimeStamp,
    string Content,
    string DmId,
    bool IsRead = false)
{
    public bool IsRead { get; set; } = IsRead;
}

public class DirectMessageService : DbService<DirectMessage>
{
    protected override string FilePath =>
        "Database/directmessages.json";

    public event Action<string>? UpdateDirectMsgs;

    public List<DirectMessage> GetDirectMessages()
    {
        return Load();
    }

    public DirectMessage? GetDirectMessage(string id)
    {
        return Get(id);
    }

    public List<DirectMessage> GetUserDms(Profile user)
    {
        return Load()
            .Where(dm => dm.Participants.Contains(user.UserName))
            .ToList();
    }

    public List<Message> ReadAllMessages(string dmId)
    {
        return Get(dmId)?.Messages ?? [];
    }

    public DirectMessage? SendDm(Profile user, string recipient)
    {
        DirectMessage? existingDm =
            GetUserDms(user)
                .FirstOrDefault(dm =>
                    dm.Participants.Contains(recipient));

        if (existingDm is not null)
            return existingDm;

        DirectMessage newDm = new(
            Guid.NewGuid().ToString(),
            [user.UserName, recipient],
            []);

        Add(newDm);

        UpdateDirectMsgs?.Invoke(newDm.Id);

        return newDm;
    }

    public void SendMessage(
        string author,
        string message,
        string dmId)
    {
        DirectMessage? dm = Get(dmId);

        if (dm is null)
            return;

        Message msg = new(
            Guid.NewGuid().ToString(),
            author,
            DateTime.Now,
            message,
            dmId);

        dm.Messages.Add(msg);

        Add(dm);

        UpdateDirectMsgs?.Invoke(dm.Id);
    }

    public void UpdateMessage(DirectMessage directMessage)
    {
        Add(directMessage);
    }
}