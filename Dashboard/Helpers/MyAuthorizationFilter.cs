using Hangfire.Dashboard;

namespace Dashboard.Helpers
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
           
            if( httpContext.User.Identity.IsAuthenticated && httpContext.User.IsInRole("Admin"))
                return true;
            return false;
           
         }
        
    }
}
