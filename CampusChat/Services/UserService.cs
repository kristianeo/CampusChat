using CampusChat.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CampusChat;

public record Profile(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    Studies Studies,
    int StartYear,
    string Introduction,
    string AvatarColor,
    bool Teacher = false,
    bool Audit = false,
    string School = "Gokstad Akademiet") : IEntity;

public enum Studies
{
    None,
    CyberSecurity, 
    BackendProgramming, 
    FrontendProgramming
}

public class UserService(ProtectedSessionStorage storage) : DbService<Profile>
{
    protected override string FilePath =>
        "Database/users.json";

    private const string StoredUser = "userName";
    private const string StoredThread = "thread";

    public event Action? ProfilesChanged;

    public List<Profile> GetProfiles()
    {
        return Load();
    }

    public Profile? GetProfile(string username)
    {
        return Load().FirstOrDefault(p =>
            string.Equals(p.UserName, username, StringComparison.OrdinalIgnoreCase));
    }

    public bool UsernameExists(string username)
    {
        return Load().Any(p =>
            string.Equals(
                p.UserName,
                username,
                StringComparison.OrdinalIgnoreCase));
    }

    public void SaveProfile(Profile profile)
    {
        Add(profile);

        ProfilesChanged?.Invoke();
    }

    public void DeleteProfile(string profileId)
    {
        Delete(profileId);

        ProfilesChanged?.Invoke();
    }

    public async Task<Profile?> LoadUserAsync()
    {
        try
        {
            var storedUser =
                await storage.GetAsync<Profile>(StoredUser);

            return storedUser.Success
                ? storedUser.Value
                : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"LoadUserAsync error: {ex.Message}");

            return null;
        }
    }

    public async Task SaveUserAsync(Profile user)
    {
        await storage.SetAsync(StoredUser, user);
    }

    public async Task ClearUserAsync()
    {
        await storage.DeleteAsync(StoredUser);
    }

    public async Task<ChatThread?> LoadThreadAsync()
    {
        try
        {
            var storedThread =
                await storage.GetAsync<ChatThread>(StoredThread);

            return storedThread.Success
                ? storedThread.Value
                : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"LoadThreadAsync error: {ex.Message}");

            return null;
        }
    }

    public async Task SaveThreadAsync(ChatThread thread)
    {
        await storage.SetAsync(StoredThread, thread);
    }

    public async Task ClearThreadAsync()
    {
        await storage.DeleteAsync(
            StoredThread);
    }
}