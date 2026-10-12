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
    
public static class Profiles
{
    public static List<Profile> AllProfiles = [];
}