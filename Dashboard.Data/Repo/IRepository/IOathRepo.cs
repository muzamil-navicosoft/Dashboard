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
        Task<IdentityResult> ConfirmEmail(string Id, string token);
        Task<SignInResult> LoginAsync(SignInDto obj);
        Task<CustomeUser?> GetUser(string Id);
        Task<IdentityResult> ChangePassword(CustomeUser user, string oldPass, string newpass);
        Task logout();
        Task<IdentityResult> CreateRoleAsync(RoleDto obj);
        IList<IdentityRole> GetRoles();
        Task<IdentityResult> DeleteRole(string roleId);
        IList<CustomeUser> GetUsers();
        Task<IList<string>> GetUserRoles(string Id);
        Task<IdentityResult> AddUserInRole(string id, IList<string> role);
        Task<IdentityResult> RemoveUserRole(string id, IList<string> role);
        Task<IList<String>> GetUserRolesByUser(CustomeUser user);
        Task<IList<CustomeUser>> GetUsersInSpecificRole(string role);


    }
}
