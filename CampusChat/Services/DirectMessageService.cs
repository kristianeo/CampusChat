using System.Collections.Immutable;

namespace CampusChat.Services;

public record Dm(
    string Id,
    ImmutableList<string> Participants
    );

public record Message( 
    string Id,
    string Author,
    DateTime TimeStamp,
    string Content,
    string DmId);

public class DirectMessageService
{
    public event Action? NewMessage;
    private List<Dm> _allDms = [];
    private List<Message> _allMessages = [];
    public Dm CreateNewDm(string receiverUserName, string userName)
    {
        ImmutableList<string> participants = [userName, receiverUserName];
        Dm dm = new Dm(Guid.NewGuid().ToString(), participants);
        _allDms.Add(dm);
        return dm;
    }

    public void AddMessage(Message message)
    {
        _allMessages.Add(message);
        NewMessage?.Invoke();
    }

    public void DeleteMessage(Message message)
    {
        _allMessages.Remove(message);
        NewMessage?.Invoke();
    }

    public List<Message> ReadAllMessages(string dmId)
    {
        List<Message> content = [];
        content.AddRange(_allMessages.Where(message => message.DmId == dmId));
        return content;
    }
}