namespace CampusChat;

public record Profile(
    string UserName,
    string FirstName,
    string LastName,
    Studies Studies,
    int StartYear,
    string Introduction,
    bool Teacher = false,
    bool Audit = false,
    string School = "Gokstad Akademiet");

public enum Studies
{
    CyberSecurity, 
    BackendProgramming, 
    FrontendProgramming
};
    
public static class Profiles
{
    public static List<Profile> AllProfiles =
    [
        new("Brukernavn", "Kristiane", "Olsen", Studies.BackendProgramming, 2025,"Hei")
    ];
}