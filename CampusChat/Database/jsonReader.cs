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
}