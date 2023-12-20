using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Dashboard.Utillities.Helper
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpClient;

        public UserService(IHttpContextAccessor httpClient)
        {
            this.httpClient = httpClient;
        }
       // public string GetUserId() => httpClient.HttpContext.User?.FindFirst("userId")?.Value ?? string.Empty;
        public string GetUserId() => httpClient.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    
        public bool isAuthenticated() => httpClient.HttpContext.User.Identity?.IsAuthenticated ?? false;
    }
}
