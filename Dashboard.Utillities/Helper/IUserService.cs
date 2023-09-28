namespace Dashboard.Utillities.Helper
{
    public interface IUserService
    {
        string GetUserId();
        bool isAuthenticated();
    }
}