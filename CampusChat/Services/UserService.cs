using System.Text.Json;
using CampusChat.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CampusChat;

public record Profile(
    string UserName,
    string FirstName,
    string LastName,
    Studies Studies,
    int StartYear,
    string Introduction,
    string AvatarColor,
    bool Teacher = false,
    bool Audit = false,
    string School = "Gokstad Akademiet");

public enum Studies
{
    None,
    CyberSecurity, 
    BackendProgramming, 
    FrontendProgramming
};

public class UserService(ProtectedLocalStorage storage)
{
    public List<Profile> AllProfiles = [];
    private const string StoredUser = "userName";
    private const string StoredThread = "thread";
    
    public List<Profile> LoadProfiles()
    {
        string path = "Database/users.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            AllProfiles = JsonSerializer.Deserialize<List<Profile>>(json) ?? new();
        }

        return AllProfiles;
    }
    public Profile? GetProfile(string username)
    {
        var profiles = LoadProfiles();
        return profiles?.FirstOrDefault(p => p.UserName == username);
    }
    public async Task<Profile?> LoadUserAsync()
    {
        ProtectedBrowserStorageResult<Profile> storedUser;

        try
        {
            storedUser = await storage.GetAsync<Profile>(StoredUser);
            return storedUser.Value;
        }
        catch(Exception ex)
        {
            
            Console.WriteLine($"Exception: {ex.Message}");
        }

        return null;
    }

    public async Task SaveUserAsync(Profile user)
    {
        await storage.SetAsync(StoredUser, user);
    }
    
    public async Task<ChatThread?> LoadThreadAsync()
    {
        ProtectedBrowserStorageResult<ChatThread> storedThread;

        try
        {
            storedThread = await storage.GetAsync<ChatThread>(StoredThread);
            return storedThread.Value;
        }
        catch(Exception ex)
        {
            
            Console.WriteLine($"Exception: {ex.Message}");
        }

        return null;
    }

    public async Task SaveThreadAsync(ChatThread thread)
    {
        await storage.SetAsync(StoredThread, thread);
    }
    
    
}