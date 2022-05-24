using Domain;
using Domain.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByLoginAsync(string login);
        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<ApplicationUser?> GetUserAsync(Guid id, string login = default, bool isRevokedIgnore = default);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> ValidateAsync(ApplicationUser user, string newPassword);
        Task<int> DeleteUserAsync(ApplicationUser user);
        Task<bool> AnyAsync(string login);
    }
}
