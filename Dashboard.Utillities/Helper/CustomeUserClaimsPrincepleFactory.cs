using Dashboard.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Utillities.Helper
{
    public class CustomeUserClaimsPrincepleFactory : UserClaimsPrincipalFactory<CustomeUser, IdentityRole>
    {
        public CustomeUserClaimsPrincepleFactory( UserManager<CustomeUser> userManager, 
                     RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options )
            : base (userManager, roleManager, options )
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(CustomeUser user)
        {
            var identity = await base.GenerateClaimsAsync (user);
            identity.AddClaim(new Claim ("userFirstName" , user.FirstName ?? string.Empty));
            identity.AddClaim(new Claim ("userLastName" , user.LastName ?? string.Empty));
            identity.AddClaim(new Claim ("userId" , user.Id ?? string.Empty));
            identity.AddClaim(new Claim ("userEmail" , user.Email ?? string.Empty));        

            return identity;
        }
    }
}
