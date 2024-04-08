namespace EOSC.API.Service.github_auth;

public interface IGitHubAuth
{
    bool IsValidUser();
    string GetUserName();
}