using System.Text.Json;

namespace CampusChat.Database;

public class jsonReader
{
    //TODO: What happens if the file is not loaded?
    public static List<Profile> LoadDatabase()
    {
        string path = "Database/users.json";
        
        List<Profile> profiles = new();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            profiles = JsonSerializer.Deserialize<List<Profile>>(json) ?? new();
        }

        return profiles;
    }
    
    public static void AddProfileToDatabase(Profile profile)
    {
        string path = "Database/users.json";

        List<Profile> profiles = LoadDatabase();

        profiles.Add(profile);
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(profiles, options);
        File.WriteAllText(path, updatedJson);
    }
    
    public static void EditProfile(Profile profile, Profile editedProfile)
    {
        string path = "Database/users.json";

        List<Profile> profiles = LoadDatabase();

        profiles.Remove(profile);
        profiles.Add(editedProfile);
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string updatedJson = JsonSerializer.Serialize(profiles, options);
        File.WriteAllText(path, updatedJson);
    }

    public static Profile? GetProfile(string username)
    {
        var profiles = LoadDatabase();
        return profiles?.FirstOrDefault(p => p.UserName == username);
    }

    /// <summary>
    /// Returns true if username already exists
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public static bool CheckUsername(string username)
    {
        var profiles = LoadDatabase();
        return profiles.FirstOrDefault(p => string.Equals(
            p.UserName, username, StringComparison.CurrentCultureIgnoreCase)) != null;
    }
}