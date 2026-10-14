namespace CampusChat.Services;

public class AppState
{
    public string ActiveUser { get; set; }
    public event Action? OnChange;
    //TODO: Do I need the action?
    public void Login(string username)
    {
        ActiveUser = username;
        OnChange?.Invoke();
    }
}