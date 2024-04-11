namespace EOSC.API.Service.github_auth;

public class GitHubAuth : IGitHubAuth
{
    public bool IsValidUser()
    {
        
        
        return true;
    }

    public string GetUserName()
    {
        return "Test";
    }
}