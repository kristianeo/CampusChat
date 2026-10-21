using System.Collections.Immutable;
using System.Text.Json;

namespace CampusChat.Services;

public record DirectMessage(
    string Id,
    ImmutableList<string> Participants,
    List<Message> Messages
    );

public record Message( 
    string Id,
    string Author,
    DateTime TimeStamp,
    string Content,
    string DmId);

public class DirectMessageService
{
    public event Action? UpdateDirectMsgs;
    private List<Message> _directMessages = [];
    public DirectMessage CreateNewDm(string receiverUserName, string userName)
    {
        ImmutableList<string> participants = [userName, receiverUserName];
        List<Message> messages = [];
        DirectMessage directMessage = new DirectMessage(Guid.NewGuid().ToString(), participants, messages);
        return directMessage;
    }

    public Message SendMessage(string author, string message, string dmId)
    {
        Message msg = new Message(Guid.NewGuid().ToString(), author, 
            DateTime.Now, message, dmId);
        
        List<DirectMessage> chats = GetDirectMessages();
        DirectMessage? chat = chats.FirstOrDefault(c => c.Id == dmId);
        chat?.Messages.Add(msg);
        UpdateDms(chats);
        _directMessages.Add(msg);
        
        return msg;
    }

    public void DeleteMessage(Message message)
    {
        List<DirectMessage> chats = GetDirectMessages();
        DirectMessage? chat = chats.FirstOrDefault(c => c.Id == message.DmId);
        chat?.Messages.Remove(message);
        UpdateDms(chats);
        _directMessages.Remove(message);
    }

    public List<Message> ReadAllMessages(string dmId)
    {
        List<DirectMessage> dms = GetDirectMessages();
        return dms.FirstOrDefault(dm => dm.Id == dmId).Messages;
    }
    private List<DirectMessage> GetDirectMessages()
    {
        string path = "Database/directmessages.json";
        
        List<DirectMessage> dms = new();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            dms = JsonSerializer.Deserialize<List<DirectMessage>>(json) ?? new();
        }

        return dms;
    }
    
    private void UpdateDms(List<DirectMessage> dms)
    {
        string path = "Database/directmessages.json";
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(dms, options);
        File.WriteAllText(path, updatedJson);
        UpdateDirectMsgs?.Invoke();
    }

    public List<DirectMessage> GetUserDms(string username)
    {
        return GetDirectMessages().Where(dm => dm.Participants.Contains(username)).ToList();
    }
    
}