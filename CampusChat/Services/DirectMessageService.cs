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
    public DirectMessage? SendDm(Profile user, string recipient)
    {
        var dmExist = GetUserDms(user).Where(dm => dm.Participants.Contains(recipient));
        if (dmExist != null) return dmExist.FirstOrDefault();
        
        ImmutableList<string> participants = [user.UserName, recipient];
        List<Message> messages = [];
        DirectMessage directMessage = new DirectMessage(Guid.NewGuid().ToString(), participants, messages);
        UpdateDms(directMessage);
        return directMessage;

    }

    public void SendMessage(string author, string message, DirectMessage dm)
    {
        Message msg = new Message(Guid.NewGuid().ToString(), author, 
            DateTime.Now, message, dm.Id);
        
        dm.Messages.Add(msg);
        
        UpdateDms(dm);
        // Do I need the local _directMessages?
        _directMessages.Add(msg);
    }

    public void DeleteMessage(Message message, DirectMessage dm)
    {
        dm.Messages.Remove(message);
        
        UpdateDms(dm);
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

    private DirectMessage? GetDirectMessage(string id)
    {
        return GetDirectMessages().FirstOrDefault(c => c.Id == id);
    }
    
    private void UpdateDms(DirectMessage dm)
    {
        List<DirectMessage> dms = GetDirectMessages();
        var oldDm = dms.FirstOrDefault(d => d.Id == dm.Id);
        dms.Remove(oldDm);
        dms.Add(dm);
        
        string path = "Database/directmessages.json";
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(dms, options);
        File.WriteAllText(path, updatedJson);
        UpdateDirectMsgs?.Invoke();
    }

    public List<DirectMessage> GetUserDms(Profile user)
    {
        return GetDirectMessages().Where(dm => dm.Participants.Contains(user.UserName)).ToList();
    }
    
}