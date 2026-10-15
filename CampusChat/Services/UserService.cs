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
    public static List<Profile> AllProfiles = [];
    private const string StoredUser = "userName";
    public async Task<Profile?> LoadUserAsync()
    {
        ProtectedBrowserStorageResult<Profile> stored;

        try
        {
            stored = await storage.GetAsync<Profile>(StoredUser);
            return stored.Value;
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
}