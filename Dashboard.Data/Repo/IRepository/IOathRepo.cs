using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repo.IRepository
{
    public interface IOathRepo
    {
        Task<IdentityResult> CreateUserAsync(SignUpDto obj);
        Task<SignInResult> LoginAsync(SignInDto obj);
        Task logout();
        
    }
}
